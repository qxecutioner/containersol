namespace WebApplication1.Models
{
    public class AppSettings : Options
    {
        public string UrlPath { get; set; }

        public override string OptionsName => "AppSettings";
    }
}
