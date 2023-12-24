# 📚 Deep Dive

It is the main folder of the whole repository. Here you will learn about the fundamentals of the C# Asynchronous programming and find out about the hidden processes behind `async/await` constructions.

We want to start our _Deep Dive_ by providing you with more concrete definition of C# Asynchronous programming: C# gives a user such thing as 🔭 **[Task asynchronous programming model or TAP](https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/task-asynchronous-programming-model)** 🔭 which is a wrapper for internal processes, which are hidden behind `async/await` keywords. The compiler does the tough work that the developer used to do. Low-level C# does not have any `async/await`, instead of that compiler rewrites the high-level C# code written by programmer in a sequence of statements that contain such thing as [Async State Machine](). As a result, you will get all of the advantages of asynchronous programming with less effort.

✅ Contents:

- [Awaitable Methods and How does Await work](./Docs/Awaitable.md)
