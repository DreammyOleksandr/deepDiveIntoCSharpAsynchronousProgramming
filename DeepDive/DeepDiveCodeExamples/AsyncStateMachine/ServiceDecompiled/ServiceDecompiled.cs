// Decompiled with JetBrains decompiler
// Type: AsyncStateMachine.Services.Service
// Assembly: AsyncStateMachine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 56636470-8303-4478-B495-BD7CD016B54A
// Assembly location: /Users/bondarenkooleksandr/C#Projects/DeepDiveIntoСSharpAsynchroniusProgramming/DeepDive/DeepDiveCodeExamples/AsyncStateMachine/AsyncStateMachine/bin/Debug/net7.0/AsyncStateMachine.dll
// Local variable names from /users/bondarenkooleksandr/c#projects/deepdiveintoсsharpasynchroniusprogramming/deepdive/deepdivecodeexamples/asyncstatemachine/asyncstatemachine/bin/debug/net7.0/asyncstatemachine.pdb
// Compiler-generated code is shown

// using Newtonsoft.Json;
// using System;
// using System.Diagnostics;
// using System.IO;
// using System.Runtime.CompilerServices;
// using System.Threading;
// using System.Threading.Tasks;
//
// namespace AsyncStateMachine.Services
// {
//   [NullableContext(1)]
//   [Nullable(0)]
//   public class Service : 
//   /*[Nullable(0)]*/
//   IService
//   {
//     private readonly string _jsonPath;
//
//     public Service(string jsonPath)
//     {
//       base..ctor();
//       this._jsonPath = jsonPath;
//     }
//
//     public object GetParseLocalJSON()
//     {
//       return JsonConvert.DeserializeObject(File.ReadAllText(this._jsonPath));
//     }
//
//     [AsyncStateMachine(typeof (Service.<GetParseLocalJSONAsync>d__3))]
//     [DebuggerStepThrough]
//     public Task<object> GetParseLocalJSONAsync()
//     {
//       Service.<GetParseLocalJSONAsync>d__3 stateMachine = new Service.<GetParseLocalJSONAsync>d__3();
//       stateMachine.<>t__builder = AsyncTaskMethodBuilder<object>.Create();
//       stateMachine.<>4__this = this;
//       stateMachine.<>1__state = -1;
//       stateMachine.<>t__builder.Start<Service.<GetParseLocalJSONAsync>d__3>(ref stateMachine);
//       return stateMachine.<>t__builder.Task;
//     }
//
//     [CompilerGenerated]
//     private sealed class <GetParseLocalJSONAsync>d__3 : 
//     /*[Nullable(0)]*/
//     IAsyncStateMachine
//     {
//       public int <>1__state;
//       [Nullable(0)]
//       public AsyncTaskMethodBuilder<object> <>t__builder;
//       [Nullable(0)]
//       public Service <>4__this;
//       [Nullable(0)]
//       private string <>s__1;
//       [Nullable(new byte[] {0, 1})]
//       private TaskAwaiter<string> <>u__1;
//
//       public <GetParseLocalJSONAsync>d__3()
//       {
//         base..ctor();
//       }
//
//       void IAsyncStateMachine.MoveNext()
//       {
//         int num1 = this.<>1__state;
//         object result;
//         try
//         {
//           TaskAwaiter<string> awaiter;
//           int num2;
//           if (num1 != 0)
//           {
//             awaiter = File.ReadAllTextAsync(this.<>4__this._jsonPath, new CancellationToken()).GetAwaiter();
//             if (!awaiter.IsCompleted)
//             {
//               this.<>1__state = num2 = 0;
//               this.<>u__1 = awaiter;
//               Service.<GetParseLocalJSONAsync>d__3 stateMachine = this;
//               this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, Service.<GetParseLocalJSONAsync>d__3>(ref awaiter, ref stateMachine);
//               return;
//             }
//           }
//           else
//           {
//             awaiter = this.<>u__1;
//             this.<>u__1 = new TaskAwaiter<string>();
//             this.<>1__state = num2 = -1;
//           }
//           this.<>s__1 = awaiter.GetResult();
//           result = JsonConvert.DeserializeObject(this.<>s__1);
//         }
//         catch (Exception ex)
//         {
//           this.<>1__state = -2;
//           this.<>t__builder.SetException(ex);
//           return;
//         }
//         this.<>1__state = -2;
//         this.<>t__builder.SetResult(result);
//       }
//
//       [DebuggerHidden]
//       void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
//       {
//       }
//     }
//   }
// }
