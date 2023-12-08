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