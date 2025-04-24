namespace WebApplication4.Models
{
    public class AppSettings : Options
    {
        public string App1_Service { get; set; }


        public string Redis_Password { get; set; }

        public override string OptionsName => "AppSettings";
    }
}
