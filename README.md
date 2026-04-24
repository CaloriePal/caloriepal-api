# CaloriePal API

REST API for [CaloriePal](https://caloriepal-web.vercel.app) — a fitness RPG that turns workout and nutrition tracking into a game. Players earn XP, level up, maintain streaks, and complete daily quests.

**Live:** `https://caloriepal-api-production.up.railway.app`  
**Frontend repo:** [caloriepal-web](https://github.com/tonymocanu97/caloriepal-web)

---

## Tech stack

| | |
|---|---|
| Framework | ASP.NET Core 10 |
| Language | C# (.NET 10) |
| Architecture | Clean Architecture |
| Pattern | CQRS via MediatR 14 |
| ORM | Entity Framework Core 10 |
| Database | PostgreSQL (Npgsql) |
| Auth | JWT Bearer — Supabase ES256 tokens validated via JWKS |
| Deployment | Railway |

---

## Architecture

The solution follows Clean Architecture with strict layer separation:

```
CaloriePal.API              → Controllers, Program.cs, HTTP concerns
CaloriePal.Application      → Use cases (CQRS commands/queries via MediatR)
CaloriePal.Domain           → Entities, domain services, business rules
CaloriePal.Infrastructure   → EF Core context, migrations, service implementations
CaloriePal.Tests            → xUnit test project
```

Dependencies point inward — `Domain` has no dependencies, `Application` depends only on `Domain`, `Infrastructure` implements `Application` interfaces.

---

## Domain model

### PlayerProfile
The central aggregate. Encapsulates all state transitions as methods — no public setters.

```
PlayerProfile
├── AddXp(amount, levelingService) → levelsGained
├── AddCoins(amount)
├── SpendCoins(amount)             → throws if insufficient
├── UpdateStreak(today)            → StreakUpdateResult
└── GrantStreakFreeze(count)
```

**Streak logic** in `UpdateStreak`:
- Same day → `AlreadyLoggedToday` (idempotent)
- 1 day gap → `StreakExtended`
- 2 day gap + freeze available → `FreezedConsumed` (consumes one freeze, extends streak)
- Gap > 2 days (or 2 days with no freeze) → `StreakBroken`

### Leveling formula
```
XpRequiredForNextLevel(level) = 100 × level^1.5
```
Level 1→2: 100 XP, Level 5→6: ~559 XP, Level 10→11: ~1581 XP. Implemented in `LevelingService` as a singleton.

### Other entities
`Quest` (Daily type, Training/Nutrition/Mindset categories, XP+coin rewards), `PlayerQuestLog`, `XpEvent`, `FoodItem`, `MealLog` (Breakfast/Lunch/Dinner/Snack), `Exercise` (Strength/Cardio), `WorkoutSession`, `WorkoutExerciseLog`

---

## API reference

All endpoints require `Authorization: Bearer <supabase-jwt>` except where noted.

### Auth — `/api/auth`
| Method | Path | Description |
|--------|------|-------------|
| POST | `/sync-profile` | Upsert player profile from OAuth user data. Called by frontend after login. |

### Players — `/api/players`
| Method | Path | Description |
|--------|------|-------------|
| GET | `/me/stats` | Player stats — level, XP, coins, streaks, freeze count |
| GET | `/me/activity-log` | Recent quest completions with XP awarded |

### Quests — `/api/quests`
| Method | Path | Description |
|--------|------|-------------|
| GET | `/daily` | Today's daily quests with completion status |
| POST | `/{questId}/complete` | Complete a quest — awards XP, coins, updates streak |

### Nutrition — `/api/Nutrition`
| Method | Path | Description |
|--------|------|-------------|
| GET | `/daily` | Daily macro summary + meal list |
| POST | `/meals` | Log a meal (from food database or manual macros) |
| GET | `/foods/search?term=` | Search food database |

### Workouts — `/api/Workouts`
| Method | Path | Description |
|--------|------|-------------|
| GET | `/stats` | Weekly goal progress, total XP, time trained, session count |
| POST | `/sessions` | Log a workout session with exercises |
| GET | `/exercises/search?term=` | Search exercise database |

### Shop — `/api/Shop`
| Method | Path | Description |
|--------|------|-------------|
| POST | `/purchase-streak-freeze` | Spend 400 coins to gain 1 streak freeze |

### Gamification — `/api/gamification`
| Method | Path | Description |
|--------|------|-------------|
| POST | `/xp` | Award XP to player |
| POST | `/streak` | Trigger streak update for today |
| POST | `/streak/freeze` | Grant streak freezes (admin/internal) |

---

## CQRS handlers

Each use case is a self-contained MediatR command or query handler:

```
Application/
├── Auth/SyncProfile/
├── Players/GetPlayerStats/ GetActivityLog/
├── Quests/GetDailyQuests/ CompleteQuest/
├── Nutrition/GetDailyNutrition/ LogMeal/ SearchFoodItems/
├── Workouts/GetWorkoutStats/ LogWorkout/ SearchExercises/
├── Streaks/UpdateStreak/ PurchaseStreakFreeze/ GrantStreakFreeze/
└── XpEvents/AddXp/
```

`CompleteQuestCommandHandler` orchestrates the full quest completion flow: mark quest complete → add XP (with level-up detection) → add coins → update streak → persist.

---

## Auth

Supabase issues **ES256** (elliptic curve) JWTs. The backend fetches Supabase's JWKS at startup and configures JWT Bearer validation with the EC public keys directly — no shared secret required, no OIDC discovery dependency at request time.

```csharp
var jwksJson = await http.GetStringAsync($"{supabaseUrl}/auth/v1/.well-known/jwks.json");
var signingKeys = new JsonWebKeySet(jwksJson).GetSigningKeys();
// ValidIssuer = "{supabaseUrl}/auth/v1"
// ValidAudience = "authenticated"
```

The current user's `UserId` is extracted from the JWT `sub` claim in `CurrentUserService` and injected via `ICurrentUserService` into every handler that needs it.

---

## Local setup

### Prerequisites
- .NET 10 SDK
- PostgreSQL (local or connection string to Railway)
- A Supabase project (for JWT validation)

### Configuration

`appsettings.Development.json` or User Secrets:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=caloriepal;Username=postgres;Password=yourpassword"
  },
  "Supabase": {
    "Url": "https://your-project.supabase.co"
  }
}
```

### Run

```bash
dotnet restore
dotnet ef database update --project CaloriePal.Infrastructure --startup-project CaloriePal.API
dotnet run --project CaloriePal.API
```

Swagger UI available at `https://localhost:7066/swagger` in development.

### Railway environment variables

```
ConnectionStrings__DefaultConnection   PostgreSQL connection string
Supabase__Url                          https://your-project.supabase.co
```

---

## Database migrations

```bash
# Add a new migration
dotnet ef migrations add MigrationName \
  --project CaloriePal.Infrastructure \
  --startup-project CaloriePal.API

# Apply migrations
dotnet ef database update \
  --project CaloriePal.Infrastructure \
  --startup-project CaloriePal.API
```

---

## Roadmap

- Achievement system with badge unlocks
- Weekly and Epic quest types
- Global leaderboard endpoint
- Progress history (XP over time, streak calendar data)
- Rate limiting on quest completion
