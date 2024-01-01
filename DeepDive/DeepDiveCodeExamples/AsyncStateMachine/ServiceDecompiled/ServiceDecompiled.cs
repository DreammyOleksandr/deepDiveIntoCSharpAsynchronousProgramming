// Decompiled with JetBrains decompiler
// Type: AsyncStateMachine.Services.Service
// Assembly: AsyncStateMachine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E52D0AEB-E88B-46E5-B73E-4950494606E3
// Assembly location: /Users/bondarenkooleksandr/C#Projects/DeepDiveIntoСSharpAsynchroniusProgramming/DeepDive/DeepDiveCodeExamples/AsyncStateMachine/AsyncStateMachine/bin/Debug/net7.0/AsyncStateMachine.dll
// Local variable names from /users/bondarenkooleksandr/c#projects/deepdiveintoсsharpasynchroniusprogramming/deepdive/deepdivecodeexamples/asyncstatemachine/asyncstatemachine/bin/debug/net7.0/asyncstatemachine.pdb
// Compiler-generated code is shown

using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncStateMachine.Services
{
  public class Service : 
  IService
  {
    private readonly string _jsonPath;

    public Service(string jsonPath)
    {
      this._jsonPath = jsonPath;
    }

    public object GetParseLocalJSON()
    {
      return JsonConvert.DeserializeObject(File.ReadAllText(this._jsonPath));
    }

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

      [DebuggerHidden]
      void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
      {
      }
    }
  }
}
