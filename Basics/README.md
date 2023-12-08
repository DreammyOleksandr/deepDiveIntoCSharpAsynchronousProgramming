# Asynchronous programming basics

This guide is aimed at any sort of programmer (from beginner to professional), so I can't avoid telling you about basics. If you already know about basics (definition, syntax, return types, default use cases), you can start [deep diving]() already in case you don't want waste your time.

## What is asynchronous programming?

Asynchronous programming is a programming paradigm that allows you to perform non-blocking operations, making it possible to write more responsive and scalable apps by using not only single (Main) thread, but others secondary.

## Syntax | [Code](./BasicsCodeExample/Syntax/)

Asynchronous programming in C# is introduced with:

`async` modifier which specifies that the asynchronous behavior of the method.\
`await` operator that instructs the compiler to create the [state machine](#) which will provide the execution of asynchronous method.

These keywords allow you to make your methods execute asynchronously, which enable you to write asynchronous code more easily (The compiler will take responsibilities of managing threads on its own, while you use `async/await`, so without C# compiler all _magic_ of these keywords will be lost).

For instance, we want to make a function, which will do some evaluations based on the housework and estimated time to do this work:

```csharp
async Task HouseMaintenance(string housework, int estimatedTime)
{
    await Task.Run(() =>
    {
        for (int secondsAmount = 1; secondsAmount < estimatedTime; secondsAmount++)
        {
            Console.WriteLine($"Doing {housework} for {secondsAmount} seconds");
        }
    });
}
```

_For the moment we are just looking at the syntax of the async function in C#, so the realization and results are not so important at this stage._

Here you can see how we used `async` modifier to declare function which can be executed asynchronously and this method contains the `await` keyword with `Task.Run()`, which indicates a point where the method can pass control to the calling code back while waiting for an asynchronous operation to complete.

Questions you may ask are: **Why is return type of this function is _Task_?**

Next [Paragraph](#return-types-of-async-functions-in-c) will answer these questions.

## Return Types of async functions in C# | [Code](./BasicsCodeExample/ReturnTypes/)

So, the question I promised you to answer is: **Why is return type of this function is _Task_?**

Lets take a brief overview on types which can be returned by asynchronous function:

- void - used for async event handlers, that strictly require this return type.
- Task - for async methods, that return void (Synchronous - void, Asynchronous - Task).
- Task\<TResult> - for async methods that return specific non-void types (Synchronous - string, Asynchronous - Task\<string>; Synchronous - YourDataType, Asynchronous - Task\<YourDataType>).
- ValueTask - for async methods that return void types and are described by structures (Will be allocated in Stack).
- ValueTask\<TResult> - for async methods that return specific non-void data types (Will be allocated in Stack).

_Honestly saying, Task and Task\<TResult> are mostly used return types that cover 90% of programmer needs and that's why I used it in previous example. Of course, you can use ValueTask and ValueTask\<TResult>, but only when you know that it will make your app run faster._

Next Instance won't have any **_awaitable_** points in asynchronous functions. It is used only to demonstrate the **return types** of functions.

```csharp
async void ReturningVoidAsync(){ }
async Task TaskVoidAsync() { }
async Task<string> TaskStringAsync() => default; //null
async ValueTask<string> ValueTaskStringAsync() => default; //null
async Task<int> TaskIntAsync() => default; //0
async ValueTask<int> ValueTaskIntAsync() => default; //0
async Task<MyTestClass> TaskMyTestClassAsync() => new MyTestClass();
async ValueTask<MyTestClass> ValueTaskMyTestClassAsync() => new MyTestClass();

List<Delegate> AsyncFuncs =
    new() { ReturningVoidAsync, TaskVoidAsync, TaskStringAsync, ValueTaskStringAsync, TaskIntAsync, ValueTaskIntAsync, TaskMyTestClassAsync, ValueTaskMyTestClassAsync, };

foreach (var asyncFunc in AsyncFuncs) Console.WriteLine(asyncFunc.GetType());

class MyTestClass { }
```

Here we can see set of functions with obvious return types. After declaration of these functions we store them in a list and after that in a `foreach` loop we write their types in console.

Let's see results of this code:

```console
System.Action
System.Func`1[System.Threading.Tasks.Task]
System.Func`1[System.Threading.Tasks.Task`1[System.String]]
System.Func`1[System.Threading.Tasks.ValueTask`1[System.String]]
System.Func`1[System.Threading.Tasks.Task`1[System.Int32]]
System.Func`1[System.Threading.Tasks.ValueTask`1[System.Int32]]
System.Func`1[System.Threading.Tasks.Task`1[MyTestClass]]
System.Func`1[System.Threading.Tasks.ValueTask`1[MyTestClass]]

Process finished with exit code 0.
```

Here we have it! I hope you expected same results. If you want to test this code by yourself, you can easily copy it from the documentation, or [Code Repository](./BasicsCodeExample/ReturnTypes/Program.cs).

_You should notice that asynchronous methods are named with `Async` word at the end due to programmers convention, but this rule is not always applicable (for more details read [Asynchronous methods naming]())_

Now you know what asynchronous programming is and how it's declared in C#!

**_Reminder: Take a break, don't forget that you are mostly productive during 1-1,5 hours of learning._**

When you ready, let's take a look at [default use cases](./Docs/DefaultUseCases.md), which will explain to you how asynchronous programming is executed in C#.
