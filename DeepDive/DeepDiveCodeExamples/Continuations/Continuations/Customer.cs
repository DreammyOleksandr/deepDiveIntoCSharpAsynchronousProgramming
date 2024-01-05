namespace Continuations;

public class Customer
{
    public int Id { get; set; }
    public string first_Name { get; set; }
    public string last_Name { get; set; }
    public string Email { get; set; }

    public override string ToString() => $"Id: {Id}; First name: {first_Name}, Last Name: {last_Name}, Email: {Email}";
}