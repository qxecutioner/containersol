namespace WebApplication1.Models
{
    public class AppSettings : Options
    {
        public string App1_Service { get; set; }

        public override string OptionsName => "AppSettings";
    }
}
