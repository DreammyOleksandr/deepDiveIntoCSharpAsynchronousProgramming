﻿using Continuations;
using Continuations.Services;

string firstLocalDbPath = @"/Users/bondarenkooleksandr/LocalDatabase.json";
string secondLocalDbPath = @"/Users/bondarenkooleksandr/SecondDataBase.json";

Service<Customer> service = new();

await service.GetParseLocalJSON(firstLocalDbPath)
    .ContinueWith(taskResult =>
    {
        return (taskResult.Status == TaskStatus.Faulted)
            ? throw new Exception("Something went wrong")
            : taskResult.Result.Where(x => x.Email.Contains("ol"));
    })
    .ContinueWith(taskResult =>
    {
        IEnumerable<Customer> customers = taskResult.Result;
        foreach (var customer in customers)
            Console.WriteLine(customer.ToString());
    });

Console.WriteLine();

await service.GetParseLocalJSON(secondLocalDbPath)
    .ContinueWith(taskResult =>
    {
        return (taskResult.Status == TaskStatus.Faulted)
            ? throw new Exception("Something went wrong")
            : taskResult.Result.Where(x => x.Email.Contains("ol"));
    })
    .ContinueWith(taskResult =>
    {
        IEnumerable<Customer> customers = taskResult.Result;
        foreach (var customer in customers)
            Console.WriteLine(customer.ToString());
    });