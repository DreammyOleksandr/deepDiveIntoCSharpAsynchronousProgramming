using Continuations;
using Continuations.Services;

string firstLocalDbPath = @"/Users/bondarenkooleksandr/LocalDatabase.json";
string secondLocalDbPath = @"/Users/bondarenkooleksandr/SecondDataBase.json";

Service<Customer> service = new();

service.GetParseLocalJSON(firstLocalDbPath)
    .ContinueWith(customers => customers.Result.Where(x => x.Email.Contains("ol")))
    .ContinueWith(sortedCustomers =>
    {
        IEnumerable<Customer> customers = sortedCustomers.Result;
        foreach (var customer in customers)
            Console.WriteLine(customer.ToString());
    });

await service.GetParseLocalJSON(secondLocalDbPath)
    .ContinueWith(customers => customers.Result.Where(x => x.Email.Contains("ol")))
    .ContinueWith(sortedCustomers =>
    {
        IEnumerable<Customer> customers = sortedCustomers.Result;
        foreach (var customer in customers)
            Console.WriteLine(customer.ToString());
    });
    