namespace CalculatorLibrary;
public sealed class ValueSamples
{
    public string FullName = "Mehmet Can ÜNALDI";

    public int Age = 34;

    public User user = new()
    {
        FullName = "Mehmet Can ÜNALDI",
        Age = 34,
        DateOfBirth = new(1989, 09, 03)
    };
}

public sealed class User
{
    public string FullName { get; set; } = string.Empty;
    public int Age { get; set; }
    public DateOnly DateOfBirth { get; set; }
}
