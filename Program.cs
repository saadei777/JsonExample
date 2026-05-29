// ============================================================
// JsonExample - C# NuGet + JSON Assignment
// Tasks: JSON read/write, deserialization, inheritance
// v4.0 - Added Task 4: Inheritance with specialized user types
// ============================================================

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

// ============================================================
// TASK 3 & 4: User class hierarchy (inheritance)
// Base class - matches the theory example
// ============================================================

/// <summary>
/// Base User class with core properties (encapsulated via C# auto-properties).
/// Demonstrates get/set as described in the theory.
/// </summary>
public class User
{
    public string Name { get; set; } = string.Empty;
    public int Age  { get; set; }
    public string City { get; set; } = string.Empty;

    /// <summary>Prints the user's basic info to the console.</summary>
    public virtual void PrintInfo()
    {
        Console.WriteLine($"  Name : {Name}");
        Console.WriteLine($"  Age  : {Age}");
        Console.WriteLine($"  City : {City}");
    }
}

// ============================================================
// TASK 4: Specialized user types via inheritance
// ============================================================

/// <summary>
/// Admin extends User with IT-specific fields.
/// </summary>
public class Admin : User
{
    public int    AccessLevel { get; set; }
    public string Department  { get; set; } = string.Empty;

    public override void PrintInfo()
    {
        base.PrintInfo();
        Console.WriteLine($"  Access Level : {AccessLevel}");
        Console.WriteLine($"  Department   : {Department}");
    }
}

/// <summary>
/// RegularUser extends User with subscription details.
/// </summary>
public class RegularUser : User
{
    public string Subscription { get; set; } = string.Empty;
    public string LastLogin    { get; set; } = string.Empty;

    public override void PrintInfo()
    {
        base.PrintInfo();
        Console.WriteLine($"  Subscription : {Subscription}");
        Console.WriteLine($"  Last Login   : {LastLogin}");
    }
}

/// <summary>
/// Moderator extends User with moderation statistics.
/// </summary>
public class Moderator : User
{
    public string ModeratedSection { get; set; } = string.Empty;
    public int    WarningsIssued   { get; set; }

    public override void PrintInfo()
    {
        base.PrintInfo();
        Console.WriteLine($"  Section          : {ModeratedSection}");
        Console.WriteLine($"  Warnings Issued  : {WarningsIssued}");
    }
}

// ============================================================
// Main entry point
// ============================================================
class Program
{
    static void Main(string[] args)
    {
        PrintHeader("C# NuGet + JSON Assignment");

        RunTask1();
        RunTask2();
        RunTask3();
        RunTask4();

        Console.WriteLine();
        Console.WriteLine("=== All tasks completed ===");
    }

    // ----------------------------------------------------------
    // TASK 1: Create a manual JSON file and read it (like the
    //         theory XML-reader example, but for JSON).
    //         Then add a new entry to the JSON object.
    // ----------------------------------------------------------
    static void RunTask1()
    {
        PrintSection("TASK 1 - Create JSON file & add new entry");

        // 1a. Create JSON manually (as shown in theory)
        string filePath = "user.json";

        string initialJson = "{\n  \"Name\": \"John Doe\",\n  \"Age\": 30,\n  \"City\": \"New York\"\n}";
        File.WriteAllText(filePath, initialJson);
        Console.WriteLine($"[1a] Created '{filePath}' with initial data.");

        // 1b. Read the file back and deserialize (mirrors the theory example)
        string jsonText = File.ReadAllText(filePath);
        User user = JsonConvert.DeserializeObject<User>(jsonText)!;

        Console.WriteLine("[1b] Deserialized user from file:");
        Console.WriteLine($"     Name: {user.Name}  |  Age: {user.Age}  |  City: {user.City}");

        // 1c. Add a new entry to the JSON object using JObject
        JObject jsonObj = JObject.Parse(jsonText);
        jsonObj["Email"] = "john.doe@example.com";   // new field added
        File.WriteAllText(filePath, jsonObj.ToString());
        Console.WriteLine("[1c] Added 'Email' field to the JSON object.");
        Console.WriteLine("[1c] Updated JSON:");
        Console.WriteLine(jsonObj.ToString());
    }

    // ----------------------------------------------------------
    // TASK 2: Add multiple users to users.json (new entries)
    //         to demonstrate adding to a JSON array.
    // ----------------------------------------------------------
    static void RunTask2()
    {
        PrintSection("TASK 2 - Add new entries to users.json");

        string filePath = "users.json";

        // Read existing array
        string jsonText  = File.ReadAllText(filePath);
        JArray usersArray = JArray.Parse(jsonText);

        Console.WriteLine($"[2a] Loaded {usersArray.Count} existing users from '{filePath}'.");

        // Create new user object and append it
        JObject newUser = new JObject
        {
            ["Name"] = "David Lee",
            ["Age"]  = 31,
            ["City"] = "Panevezys"
        };

        usersArray.Add(newUser);
        File.WriteAllText(filePath, usersArray.ToString());

        Console.WriteLine("[2b] Added new user 'David Lee' to the array.");
        Console.WriteLine($"     Array now contains {usersArray.Count} users.");
    }

    // ----------------------------------------------------------
    // TASK 3: Deserialize ALL entries (with loop) and output
    //         each user's data to the console.
    // ----------------------------------------------------------
    static void RunTask3()
    {
        PrintSection("TASK 3 - Deserialize all users (loop) and print");

        string filePath = "users.json";
        string jsonText = File.ReadAllText(filePath);

        // Deserialize into a List<User> - entire array at once
        List<User> users = JsonConvert.DeserializeObject<List<User>>(jsonText)!;

        Console.WriteLine($"[3] Deserialized {users.Count} users. Printing each:\n");

        // Loop through every user and output to console
        for (int i = 0; i < users.Count; i++)
        {
            Console.WriteLine($"  --- User #{i + 1} ---");
            users[i].PrintInfo();
            Console.WriteLine();
        }
    }

    // ----------------------------------------------------------
    // TASK 4: Inheritance - deserialize user_types.json into
    //         the correct subclass based on "UserType" field,
    //         then output each to the console.
    // ----------------------------------------------------------
    static void RunTask4()
    {
        PrintSection("TASK 4 - Inheritance: deserialize specialized user types");

        string filePath = "user_types.json";
        string jsonText = File.ReadAllText(filePath);

        // Parse as a generic array first so we can inspect "UserType"
        JArray rawArray = JArray.Parse(jsonText);

        // Collect deserialized User-derived objects
        List<User> typedUsers = new List<User>();

        foreach (JObject item in rawArray)
        {
            string userType = item["UserType"]?.ToString() ?? "User";

            // Factory pattern: pick the right subclass based on the "UserType" field
            User typed = userType switch
            {
                "Admin"       => item.ToObject<Admin>()!,
                "RegularUser" => item.ToObject<RegularUser>()!,
                "Moderator"   => item.ToObject<Moderator>()!,
                _             => item.ToObject<User>()!
            };

            typedUsers.Add(typed);
        }

        Console.WriteLine($"[4] Loaded {typedUsers.Count} typed users. Printing each:\n");

        foreach (User u in typedUsers)
        {
            // GetType().Name gives us "Admin", "RegularUser", etc.
            Console.WriteLine($"  === [{u.GetType().Name}] ===");
            u.PrintInfo();    // virtual - calls the correct override
            Console.WriteLine();
        }
    }

    // ----------------------------------------------------------
    // Helpers for formatted console output
    // ----------------------------------------------------------
    static void PrintHeader(string title)
    {
        Console.WriteLine(new string('=', 55));
        Console.WriteLine($"  {title}");
        Console.WriteLine(new string('=', 55));
        Console.WriteLine();
    }

    static void PrintSection(string title)
    {
        Console.WriteLine();
        Console.WriteLine(new string('-', 55));
        Console.WriteLine($"  {title}");
        Console.WriteLine(new string('-', 55));
    }
}
