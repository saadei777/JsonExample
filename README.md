# JsonExample – C# NuGet + JSON Assignment

A C# console application demonstrating JSON file handling, deserialization with Newtonsoft.Json, and object-oriented inheritance.

---

## Version History (Commit Guide)

| Version | Tag | What was done |
|---------|-----|---------------|
| v1.0 | `task1-json-file` | Created `user.json` manually, read it back, deserialized into `User` object, added new `Email` field using `JObject` |
| v2.0 | `task2-add-entries` | Loaded `users.json` array, added a new user entry (`David Lee`) using `JArray` and saved back to disk |
| v3.0 | `task3-loop-deserialize` | Deserialized entire `users.json` into `List<User>` and printed all entries to console using a `for` loop |
| v4.0 | `task4-inheritance` | Introduced `Admin`, `RegularUser`, and `Moderator` subclasses extending `User`; created `user_types.json`; deserialized using a factory switch pattern and printed via virtual `PrintInfo()` |

---

## Project Structure

```
JsonExample/
├── Program.cs          # Main program – all 4 tasks
├── users.json          # Array of basic users (Tasks 2 & 3)
├── user_types.json     # Specialized user types (Task 4)
├── user.json           # Single user file (generated at runtime by Task 1)
├── lib/
│   └── Newtonsoft.Json.dll   # Local NuGet package reference
└── JsonExample.csproj
```

---

## Tasks Explained

### Task 1 – Create JSON File & Add New Entry
- Writes `user.json` with `Name`, `Age`, `City` fields (matches theory example)
- Reads the file and deserializes it into a `User` C# object
- Uses `JObject` to **add a new field** (`Email`) to the existing JSON object
- Saves the updated JSON back to disk

### Task 2 – Add New Entries to a JSON Array
- Loads `users.json` (existing 3 users)
- Creates a new `JObject` for user "David Lee"
- Appends it to the `JArray` and saves

### Task 3 – Deserialize All Entries with a Loop
- Reads `users.json` (now 4 users after Task 2)
- Deserializes with `JsonConvert.DeserializeObject<List<User>>()`
- Iterates using a `for` loop and calls `PrintInfo()` on each

### Task 4 – Inheritance and Specialized User Types
- `Admin`, `RegularUser`, and `Moderator` all **inherit** from `User`
- Each subclass has extra fields and overrides `virtual PrintInfo()`
- `user_types.json` stores the typed entries with a `"UserType"` discriminator
- A **factory switch expression** maps the string type to the correct class
- All entries printed via polymorphic `PrintInfo()` calls

---

## How to Run

```bash
# Clone the repo
git clone https://github.com/YOUR_USERNAME/JsonExample.git
cd JsonExample

# Build (no NuGet restore needed – dll included locally)
dotnet build --no-restore

# Run
dotnet run --no-build
```

---

## Technologies Used

- **C# / .NET 8**
- **Newtonsoft.Json** (NuGet) – JSON serialization/deserialization
- `JObject` / `JArray` – dynamic JSON manipulation
- OOP: classes, inheritance, virtual methods, auto-properties (`get; set;`)
