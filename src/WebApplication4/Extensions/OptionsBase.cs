
using WebApplication4.Models;

namespace WebApplication4.Extensions
{
    public static class OptionsBase
    {
        public static IServiceCollection AddOption<TOptions>(this IServiceCollection service) where TOptions : Options
        {
            service.AddOptions<TOptions>()
                .Configure<IConfiguration>((settings, configuration) => {
                    configuration.GetSection(settings.OptionsName).Bind(settings);
                });

            return service;
        }
    }
}
