namespace PreciousPractice.Helpers;

public class SubscriptionPlan : ContentPage
{
	public string Provider { get; set; } = string.Empty;
	public string PlanDescription { get; set; } = string.Empty;
    public string Validity { get; set; } = string.Empty;
    public decimal Price { get; set; }

	public string PlanPrice => $"N{Price:N0}";
}
public static class SubscriptionPlans
{
	public static List<SubscriptionPlan> All { get; } = new()
	{
		Plans("GOTV", "GOtv Smallie", 1900m),
		Plans("GOTV", "GOtv Jinja", 3900m),
		Plans("GOTV", "GOtv Jolli", 5800m),
		Plans("GOTV", "GOtv Max", 8500m),
		Plans("GOTV", "GOtv Supa", 11400m),
		Plans("GOTV", "GOtv Supa Plus", 16800m),
		Plans("StarTimes","StarTimes Nova",2100m),
		Plans("StarTimes","StarTimes Basic",4000m),
		Plans("StarTimes","StarTimes Smart",5100m),
		Plans("StarTimes","StarTimes Classic",6000m),
		Plans("StarTimes","StarTimes Special",7400m),
		Plans("StarTimes","StarTimes Super",9800m),
		Plans("DSTV","DStv Padi",4400m),
		Plans("DSTV","DStv Yanga",6000m),
		Plans("DSTV","DStv Confam",11000m),
		Plans("DSTV","DStv Compact",19000m),
		Plans("DSTV","DStv Compact Plus",30000m),
		Plans("DSTV","DStv Premium",44500m),
	};
	private static SubscriptionPlan Plans(string provider, string name, decimal price, string validity = "1 Month")
		=> new() { Provider = provider, PlanDescription = name, Price = price, Validity = validity };
}