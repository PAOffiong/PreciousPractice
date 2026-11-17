namespace PreciousPractice.Helpers;

public class DefaultAccountBalance 
{
    public static decimal Savings { get; set; } = 200000m;
    public static decimal Current { get; set; } = 100000m;

    public static string SelectedAccount { get; set; } = "Savings";

    public static decimal SelectedBalance =>
        SelectedAccount == "Current" ? Current : Savings;

    public static string Format(decimal amount) => $"N{amount:N2}";

    public static void Deduct(decimal amount)
    {
        if (SelectedAccount == "Current")
            Current -= amount;
        else
            Savings -= amount;
    }
}
