# Awaitable | [Code]()

## Awaitable Methods

Are asynchronous methods the execution of which can be waited if you need their result at the moment. You should use operator `await` to wait for execution of such methods, but we have

## Other ways of awaiting

- Result prop.
- Methods: .Wait(), .WaitAll(), .WaitAny().
- Method .GetResult() from Method .GetAwaiter().

_Notice that .GetAwaiter() is not recommended for use of programmer. .GetAwaiter() is a method of TaskAwaiter structure which is more preferred to be used by the C# compiler rather than developer._
