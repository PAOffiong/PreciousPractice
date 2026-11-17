namespace PreciousPractice.Helpers;

public enum DataPlanCategory //fixed  set of choices and user can pick anyone
{
    All,
    Daily,
    Weekly,
    Monthly
}
public class DataPlan
{
    public string Network {  get; set; } = string.Empty;
    public DataPlanCategory Category { get; set; }
    public string PlanDescription { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public string PlanPrice =>$"N{Price:N0}";
    public string ServiceText => $"Network: {Network}";
}
public static class DataPlans
{
    public static List<DataPlan> All { get; } = new()
    {
        Plan("MTN",DataPlanCategory.Daily, "20MB Facebook Daily",      25m),
        Plan("MTN",DataPlanCategory.Daily, "20MB WhatsApp Daily",      25m),
        Plan("MTN",DataPlanCategory.Daily, "150MB TikTok Daily",      50m),
        Plan("MTN",DataPlanCategory.Weekly, "40MB WhatsApp Weekly",      50m),
        Plan("MTN",DataPlanCategory.Weekly, "470MB All Social Weekly",      200m),
        Plan("MTN",DataPlanCategory.Weekly, "2GB TikTok Weekly",      400m),
        Plan("MTN",DataPlanCategory.Daily, "200MB All Social Daily Plan",      100m),
        Plan("MTN",DataPlanCategory.Weekly, "1GB Weekly Plan",      800m),
        Plan("MTN",DataPlanCategory.Weekly, "6GB Weekly Plan",      2500m),
        Plan("MTN",DataPlanCategory.Monthly, "120MB Facebook Monthly",      150m),
        Plan("MTN",DataPlanCategory.Monthly, "2.7GB+2mins Monthly Plan",      2000m),
        Plan("MTN",DataPlanCategory.Monthly, "3.5GB+5mins Monthly Plan",      2500m),
        Plan("MTN",DataPlanCategory.Daily, "110MB Daily Plan",      110m),
        Plan("MTN",DataPlanCategory.Daily, "1GB Daily Plan + 1.5mins",      500m),
        Plan("MTN",DataPlanCategory.Daily, "2.5GB Daily Plan/750 Daily",      750m),
        Plan("MTN",DataPlanCategory.Weekly, "11GB Weekly Bundle",      3500m),
        Plan("MTN",DataPlanCategory.Monthly, "7GB Monthly Plan",      3500m),
        Plan("MTN",DataPlanCategory.Monthly, "150GB 2-Month Plan",      40000m),
        Plan("MTN",DataPlanCategory.Weekly, "20GB Weekly Plan-5000",      5000m),
        Plan("MTN",DataPlanCategory.Monthly, "450GB 3-Month Plan Broadband Plan_30days",      75000m),

        //Airtel plan
        
        Plan("Airtel",DataPlanCategory.Daily, "75MB 1day",      75m),
        Plan("Airtel",DataPlanCategory.Daily, "200MB-2days Social Plan",      100m),
        Plan("Airtel",DataPlanCategory.Daily, "300MB-3days Daily Plan",      50m),
        Plan("Airtel",DataPlanCategory.Weekly, "500MB-7days Weekly Plan",      50m),
        Plan("Airtel",DataPlanCategory.Weekly, "470MB All Social Weekly",      200m),
        Plan("Airtel",DataPlanCategory.Weekly, "2GB TikTok Weekly",      400m),
        Plan("Airtel",DataPlanCategory.Daily, "1GB-3days Social Plan",      300m),
        Plan("Airtel",DataPlanCategory.Weekly, "1GB Weekly Plan",      800m),
        Plan("Airtel",DataPlanCategory.Weekly, "6GB Weekly Plan",      2500m),
        Plan("Airtel",DataPlanCategory.Monthly, "8GB-30days + 2GB YouTube Nights",      3000m),
        Plan("Airtel",DataPlanCategory.Monthly, "4GB-30days +2GB Youtube Night",      2500m),
        Plan("Airtel",DataPlanCategory.Monthly, "10GB-30days +2GB YouTube Night",      4000m),
        Plan("Airtel",DataPlanCategory.Daily, "500MB-2days",      350m),
        Plan("Airtel",DataPlanCategory.Daily, "Binge 1GB-1day",      500m),
        Plan("Airtel",DataPlanCategory.Daily, "Talkmore-5days,30mins",      300m),
        Plan("Airtel",DataPlanCategory.Weekly, "11GB Weekly Bundle",      3500m),
        Plan("Airtel",DataPlanCategory.Monthly, "2GB-30days +2GB",      1500m),
        Plan("Airtel",DataPlanCategory.Monthly, "3GB-30days + 2GB YouTube",      2000m),
        Plan("Airtel",DataPlanCategory.Weekly, "20GB Weekly Plan-5000",      5000m),

        //glo
        Plan("Glo",DataPlanCategory.Daily, "Oneoff_40MB +5MB Night-1 day",      50m),
        Plan("Glo",DataPlanCategory.Daily, "Oneoff_135MB Socials",      50m),
        Plan("Glo",DataPlanCategory.Daily, "Oneoff_1GB-2HRS",      300m),
        Plan("Glo",DataPlanCategory.Weekly, "Oneoff_17GB + 2GB Night-7days",      1000m),
        Plan("Glo",DataPlanCategory.Weekly, "Oneoff_6.5GB +2.5GB Night-7days",      2000m),
        Plan("Glo",DataPlanCategory.Weekly, "Oneoff_22GB + 2GB Night-7days",      5000m),
        Plan("Glo",DataPlanCategory.Daily, "Special 3.1GB + 2GB night-2days",      1000m),
        Plan("Glo",DataPlanCategory.Weekly, "1GB Weekly Plan",      800m),
        Plan("Glo",DataPlanCategory.Weekly, "6GB Weekly Plan",      2500m),
        Plan("Glo",DataPlanCategory.Monthly, "119GB-30days",      180000m),
        Plan("Glo",DataPlanCategory.Monthly, "Mega Plan_220GB-30days",      40000m),
        Plan("Glo",DataPlanCategory.Monthly, "Oneoff_26GB + 2GB Night_30days",      8000m),
        Plan("Glo",DataPlanCategory.Daily, "Youtube Special OneOff-14HRS-3GB-2 days",      600m),
        Plan("Glo",DataPlanCategory.Daily, "1.55GB + 2GB Night-2days",      600m),
        Plan("Glo",DataPlanCategory.Monthly, "Oneoff_18,5GB + 2GB Night_30days",      6000m),
        Plan("Glo",DataPlanCategory.Weekly, "11GB Weekly Bundle",      3500m),
        Plan("Glo",DataPlanCategory.Monthly, "Oneoff_62GB + 2GB Night_30days",      15000m),
        Plan("Glo",DataPlanCategory.Monthly, "Oneoff_38GB + 2GB Night_30days",      10000m),
        Plan("Glo",DataPlanCategory.Weekly, "20GB Weekly Plan-5000",      5000m),
        Plan("Glo",DataPlanCategory.Monthly, "Oneoff_14.5GB + 2.5GB Night_30days",      5000m),

        //9mobile
        Plan("9mobile",DataPlanCategory.Daily, "40MB daily",      50m),
        Plan("9mobile",DataPlanCategory.Daily, "83MB + 50MB Socials daily",      100m),
        Plan("9mobile",DataPlanCategory.Daily, "150MB +100MB Nights",      150m),
        Plan("9mobile",DataPlanCategory.Weekly, "650MB +100MB Socials",      500m),
        Plan("9mobile",DataPlanCategory.Weekly, "3.4GB Special Data",      1500m),
        Plan("9mobile",DataPlanCategory.Monthly, "2GB Anytime Data-30days",      1000m),
        Plan("9mobile",DataPlanCategory.Monthly, "2.3GB Anytime Data-30days",      1200m),
        Plan("9mobile",DataPlanCategory.Monthly, "4.5GB Anytime Data-30days",         2000m),
        Plan("9mobile",DataPlanCategory.Daily,   "250MB Anytime data",      200m),
        Plan("9mobile",DataPlanCategory.Monthly, "6.2GB Anytime Data-30days",      3000m),
        Plan("9mobile",DataPlanCategory.Monthly, "8.4GB Anytime Data-30days",           4000m),
        Plan("9mobile",DataPlanCategory.Monthly, "11.4GB Anytime Data-30days",           15000m),
        Plan("9mobile",DataPlanCategory.Monthly, "23GB-30days",           10000m),
        Plan("9mobile",DataPlanCategory.Monthly, "35GB-30days",           15000m),
        Plan("9mobile",DataPlanCategory.Monthly, "47GB-30days",           20000m),
        Plan("9mobile",DataPlanCategory.Monthly, "118GB-30days",           50000m),
    };
    private static DataPlan Plan(string network, DataPlanCategory category, string name, decimal price)
    {
        return new DataPlan
        {
            Network = network,
            Category = category,
            PlanDescription = name,
            Price = price
        };
    }
}
