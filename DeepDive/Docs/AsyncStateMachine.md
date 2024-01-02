# Asynchronous State Machine | [Code](../DeepDiveCodeExamples/AsyncStateMachine/)

- [Asynchronous State Machine | Code](#asynchronous-state-machine--code)
  - [Theory and definitions](#theory-and-definitions)
  - [Code Example](#code-example)
  - [Decompiled Code Review | Code](#decompiled-code-review--code)
  - [Decompiled Code Execution | Code](#decompiled-code-execution--code)
  - [Conclusion](#conclusion)

## Theory and definitions

This particular part presents you one of the main C# Asynchronous programming concepts: **Asynchronous State Machine**. This exact code structure is hidden behind the `async/await` keywords and all of the _magic_ is done by it.

C# _**Asynchronous State Machine**_ is a code abstraction provided by C# that manages the asynchronous operations in your code and is created in _Low-level C#_ when we use `async/await` keywords.

In Asynchronous _State_ Machine we can notice the word _State_. Let's see what _States_ of Asynchronous State Machine exist:

```C#
enum State
{
    Completed = -2,
    Created = -1,
    Awaiting = 0,
}
```

## Code Example

For these app you need a **[Newtonsoft.Json](https://www.newtonsoft.com/json)** NuGet Package to be installed.

Keep moving forward and let us navigate you through our code example (which was used in [Task.WhenAll()](../../Basics/BasicsCodeExamples/TaskWhenAll/TaskWhenAll/) section) by showing executable functions and calling code:

The [IService](../DeepDiveCodeExamples/AsyncStateMachine/AsyncStateMachine/Services/IService.cs) interface which is implemented by Service class:

```C#
namespace AsyncStateMachine.Services;

public interface IService
{
    Task<object> GetParseLocalJSONAsync();
    object GetParseLocalJSON();
}
```

[Service](../DeepDiveCodeExamples/AsyncStateMachine/AsyncStateMachine/Services/Service.cs) Class with executable functions:

```C#
using Newtonsoft.Json;

namespace AsyncStateMachine.Services;

public class Service : IService
{
    private readonly string _jsonPath;
    public Service(string jsonPath)
    {
        _jsonPath = jsonPath;
    }

    public object GetParseLocalJSON() =>
        JsonConvert.DeserializeObject(File.ReadAllText(_jsonPath));

    public async Task<object> GetParseLocalJSONAsync() =>
        JsonConvert.DeserializeObject(await File.ReadAllTextAsync(_jsonPath));
}
```

We can see that `Service` defines the functions of implemented `IService` interface. `GetParseLocalJSON()` and `GetParseLocalJSONAsync()` return us parsed JSON objects synchronously and and asynchronously respectively.

Now let's take a look at our calling code in [Program](../DeepDiveCodeExamples/AsyncStateMachine/AsyncStateMachine/Program.cs) class:

```C#
using AsyncStateMachine.Services;

string jsonPathLocal = @"/Users/bondarenkooleksandr/LocalDatabase.json";
IService service = new Service(jsonPathLocal);

Console.WriteLine($"{await service.GetParseLocalJSONAsync()}\n{service.GetParseLocalJSON()}");
```

Here we define our JSON local path then instantiate our `Service` with this path and after that we output the execution of `GetParseLocalJSON()` and `GetParseLocalJSONAsync()` methods. Now we can execute our code to see if it actually works:

```console
[
  {
    "id": 1,
    "first_name": "Piper",
    "last_name": "Durdy",
    "email": "pdurdy0@nytimes.com"
  },
  {
    "id": 2,
    "first_name": "Elva",
    "last_name": "Stearns",
    "email": "estearns1@japanpost.jp"
  },

...

  {
    "id": 299,
    "first_name": "Mozes",
    "last_name": "Oen",
    "email": "moen8a@marriott.com"
  },
  {
    "id": 300,
    "first_name": "Kessiah",
    "last_name": "Mustill",
    "email": "kmustill8b@vimeo.com"
  }
]
[
  {
    "id": 1,
    "first_name": "Piper",
    "last_name": "Durdy",
    "email": "pdurdy0@nytimes.com"
  },
  {
    "id": 2,
    "first_name": "Elva",
    "last_name": "Stearns",
    "email": "estearns1@japanpost.jp"
  },

...

  {
    "id": 299,
    "first_name": "Mozes",
    "last_name": "Oen",
    "email": "moen8a@marriott.com"
  },
  {
    "id": 300,
    "first_name": "Kessiah",
    "last_name": "Mustill",
    "email": "kmustill8b@vimeo.com"
  }
]
```

Yep, our functions have outputted in the same way. App works perfectly.

## Decompiled Code Review | [Code](../DeepDiveCodeExamples/AsyncStateMachine/ServiceDecompiled/)

_Notice that if you use the IL-Viewer to decompile your code then you will see strange naming of props or functions like `<>d__3`, `t__1` or something like this, also you will see some unusual constructions, which are not common for typical C# coding. It happens because decompiler does not write code for human and it does not understand naming conventions. Also, such naming in code is not allowed in High-level C#, so when you first see the decompiled code you can be a little confused, but we rewrote these parts of this decompiled code to help you understand what happens._

Firstly, for better further understanding we have to show you the _IAsyncStateMachine_ interface and its methods:

```C#
namespace System.Runtime.CompilerServices
{
    public interface IAsyncStateMachine
    {
        void MoveNext();
        void SetStateMachine(IAsyncStateMachine stateMachine);
    }
}
```

Here we have:

- `MoveNext()` method which executes the body of asynchronous methods.
- `SetStateMachine(IAsyncStateMachine stateMachine)` method (used by compiler if your assembly is on Release mode) is used for work with `Struct`s to put Finite-State Machine on Heap.

We decompiled our code using [IL-Viewer](https://www.jetbrains.com/help/rider/Viewing_Intermediate_Language.html) integrated to JetBrains [Rider](https://www.jetbrains.com/rider/) IDE.

Now we can find out what exactly hidden behind `async/await` keywords by decompiling our code to _Low-level C#_.

For this instance the [Service](../DeepDiveCodeExamples/AsyncStateMachine/AsyncStateMachine/Services/Service.cs) class is decompiled. Let's take a look at this code step by step to keep you in touch:

```C#
  public class Service :
  IService
  {
    private readonly string _jsonPath;

    public Service(string jsonPath)
    {
      this._jsonPath = jsonPath;
    }
```

The beginning of `Service` class declaration is pretty straight forward. We can see that this class implements the `IService` interface and has the same `_jsonPath` field as in main code example. Keep moving forward and take a look at `GetParseLocalJSON()` method declaration:

```C#
    public object GetParseLocalJSON()
    {
      return JsonConvert.DeserializeObject(File.ReadAllText(this._jsonPath));
    }
```

Basically this method stayed absolutely the same, but was decompiled as a regular method instead of lambda expression. Now let's take a look at the asynchronous variation of our method:

```C#
    [AsyncStateMachine(typeof (GetParseJSONAsyncStruct))]
    [DebuggerStepThrough]
    public Task<object> GetParseLocalJSONAsync()
    {
      GetParseJSONAsyncStruct stateMachine = new();
      stateMachine.builder = AsyncTaskMethodBuilder<object>.Create();
      stateMachine.state = -1;
      stateMachine.service = this;
      stateMachine.builder.Start(ref stateMachine);
      return stateMachine.builder.Task;
    }
```

We can see that we do not have the `async` modifier. Instead, our method now has the `[AsyncStateMachine]` attribute where we pass the `GetParseJSONAsyncStruct` that will execute all asynchronous logic. In body method we create the _State Machine_ of `GetParseJSONAsyncStruct` type and then we fill its fields { builder, state(created), builder(Of asynchronous methods)} and at the end we call `Start()` method of builder which accepts the stateMachine that we created. This method is responsible for calling `MoveNext()` method that executes our asynchronous code.

Move on and let's take a look at the `GetParseJSONAsyncStruct` fields that we were filling previously:

```C#
    [CompilerGenerated]
    [StructLayout(LayoutKind.Sequential)]
    private struct GetParseJSONAsyncStruct :

      IAsyncStateMachine
    {
      public int state;
      public AsyncTaskMethodBuilder<object> builder;
      public Service service;
      private string receivedObject;
      private TaskAwaiter<string> awaiter;
```

The exact same fields that we were filling, but we also have new private fields. To see what they are meant for let's go on and see the declaration of `MoveNext()` method:

```C#
void IAsyncStateMachine.MoveNext()
      {
        int num1 = state;
        object result;
        try
        {
          TaskAwaiter<string> awaiter;
          int num2;
          if (num1 != 0)
          {
            awaiter = File.ReadAllTextAsync(service._jsonPath, new CancellationToken()).GetAwaiter();
            if (!awaiter.IsCompleted)
            {
              state = num2 = 0;
              this.awaiter = awaiter;
              GetParseJSONAsyncStruct stateMachine = this;
              builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, GetParseJSONAsyncStruct>(ref awaiter, ref stateMachine);
              return;
            }
          }
          else
          {
            awaiter = this.awaiter;
            this.awaiter = new TaskAwaiter<string>();
            state = num2 = -1;
          }
          receivedObject = awaiter.GetResult();
          result = JsonConvert.DeserializeObject(receivedObject);
        }
        catch (Exception ex)
        {
          state = -2;
          builder.SetException(ex);
          return;
        }
        state = -2;
        builder.SetResult(result);
      }
```

Let's take a look at this method line-by-line:

```C#
        int num1 = state;
        object result;
```

Here we assign our state to the `num1` variable and it will be operated by method to check the state of execution. `result` variable stores the result of the `MoveNext()` execution further in code. Let's move on and figure out what happens in `try{}` block:

```C#
 try
        {
          TaskAwaiter<string> awaiter;
          int num2;
          if (num1 != 0)
          {
            awaiter = File.ReadAllTextAsync(service._jsonPath, new CancellationToken()).GetAwaiter();
            if (!awaiter.IsCompleted)
            {
              state = num2 = 0;
              this.awaiter = awaiter;
              builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, GetParseJSONAsyncStruct>(ref awaiter, ref this);
              return;
            }
          }
          else
          {
            awaiter = this.awaiter;
            this.awaiter = new TaskAwaiter<string>();
            state = num2 = -1;
          }
          receivedObject = awaiter.GetResult();
          result = JsonConvert.DeserializeObject(receivedObject);
        }
```

In `try{}` block we have the body of our asynchronous method (Indeed with some changes). Here we can see that compiler generated the `awaiter` variable of `TaskAwaiter` type. We need it to store the object that will wait for the execution of asynchronous operation and generator also created the `num2` variable (To be honest, this variable does not contain any useful information, so do not give it attention). On the next line we check if our state (`num1`) is _NOT_ equal to 0 (Awaiting). If it is true we are going into the if-block and than we assign awaiter of our main operation (Reading text from file) to the `awaiter` variable with `GetAwaiter()` method. Next we check if the execution is _NOT_ completed. If it is true (The task is not completed) we assign 0 (Awaiting) state to our `state` variable. Then we assign our awaiter to the awaiter field to store there all of the received data. On the next line we move to the main method which releases the asynchronous functionality:

```C#
  builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, GetParseJSONAsyncStruct>(ref awaiter, ref this);
```

This method creates the continuation-delegate and with passed awaiter sets the continuation for the current task. After this we end our method with `return` operator. Lets go further and check the else-block and code after it:

```C#
          else
          {
            awaiter = this.awaiter;
            this.awaiter = new TaskAwaiter<string>();
            state = num2 = -1;
          }
          receivedObject = awaiter.GetResult();
          result = JsonConvert.DeserializeObject(receivedObject);
```

So if our state is not awaiting (!= 0) we go into the else block where we write our state to the `awaiter` variable and create new instance of `awaiter` field. After that we set our state to -1 (Created). After the else-block we assign the result of awaiter to our object and after we do the Deserialization of the `receivedObject`.

In the catch-block:

```C#
catch (Exception ex)
        {
          state = -2;
          builder.SetException(ex);
          return;
        }
```

We just set the state of execution to -2 (Completed) and right after we set the exception which possibly could be thrown while task execution. Right after this we:

```C#
        state = -2;
        builder.SetResult(result);
```

Set the state to -2 (Completed) and Set the result of our execution.

## Decompiled Code Execution | [Code](../DeepDiveCodeExamples/AsyncStateMachine/AsyncStateMachine_Decompiled)

We can understand that the decompiled code is the true code which is hidden behind `async/await`. So, we can put instead of our `Service` firstly written code the decompiled one and expect the same result, so lets do it:

```console
[
  {
    "id": 1,
    "first_name": "Piper",
    "last_name": "Durdy",
    "email": "pdurdy0@nytimes.com"
  },
  {
    "id": 2,
    "first_name": "Elva",
    "last_name": "Stearns",
    "email": "estearns1@japanpost.jp"
  },

...

  {
    "id": 299,
    "first_name": "Mozes",
    "last_name": "Oen",
    "email": "moen8a@marriott.com"
  },
  {
    "id": 300,
    "first_name": "Kessiah",
    "last_name": "Mustill",
    "email": "kmustill8b@vimeo.com"
  }
]
[
  {
    "id": 1,
    "first_name": "Piper",
    "last_name": "Durdy",
    "email": "pdurdy0@nytimes.com"
  },
  {
    "id": 2,
    "first_name": "Elva",
    "last_name": "Stearns",
    "email": "estearns1@japanpost.jp"
  },

...

  {
    "id": 299,
    "first_name": "Mozes",
    "last_name": "Oen",
    "email": "moen8a@marriott.com"
  },
  {
    "id": 300,
    "first_name": "Kessiah",
    "last_name": "Mustill",
    "email": "kmustill8b@vimeo.com"
  }
]
```

Yep, the results absolutely the same. We can see that our code executed as we expected. Now lets take a look at the _Deep_ execution of await operator on Decompiled level:

<p>
  <img src="./AsyncStateMachine Images/AsyncStateMachineInside.png" alt="Ooops..."/>
</p>

## Conclusion

We found out that there is a difficult code structure hidden behind `async/await` keywords called **Asynchronous State Machine**. This state machine describes the asynchronous functionality of your code and provides deep abstraction for high-level C# in case to help developers to minimize code work and to be more productive while writing the asynchronous code.
