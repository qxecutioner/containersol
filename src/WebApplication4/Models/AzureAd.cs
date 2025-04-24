using WebApplication4.Models;

namespace WebApplication4.Models
{
    public class AzureAd : Options
    {
        public override string OptionsName => nameof(AzureAd);

        public string IssuerKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}
