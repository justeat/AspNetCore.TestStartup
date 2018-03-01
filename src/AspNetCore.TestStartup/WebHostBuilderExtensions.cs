using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.TestStartup
{
     public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseTestStartup<TStartup>(this IWebHostBuilder webHostBuilder, Action<IServiceCollection> configureServicesCallback)
            where TStartup : IStartup
        {
            webHostBuilder
                .UseStartup(typeof(TStartup)) // Ensure correct initial setup.
                .ConfigureServices(services =>
                {
                    services.AddSingleton(typeof(TStartup));
                    services.AddSingleton<IStartup, TestStartup>(s =>
                    {
                        var baseStartup = s.GetService<TStartup>();
                        var testStartup = new TestStartup(baseStartup);

                        testStartup.ConfigureServices(configureServicesCallback);

                        return testStartup;
                    });
                });

            return webHostBuilder;
        }

        private class TestStartup : IStartup
        {
            private readonly IStartup _baseStartup;

            private Action<IServiceCollection> _configureServicesCallback;

            public TestStartup(IStartup baseStartup)
            {
                _baseStartup = baseStartup;
            }

            public IServiceProvider ConfigureServices(IServiceCollection services)
            {
                _baseStartup.ConfigureServices(services);
                _configureServicesCallback?.Invoke(services);

                return services.BuildServiceProvider();
            }

            public void Configure(IApplicationBuilder app)
            {
                _baseStartup.Configure(app);
            }

            public void ConfigureServices(Action<IServiceCollection> configureServicesCallback)
            {
                _configureServicesCallback = configureServicesCallback;
            }
        }
    }
}
