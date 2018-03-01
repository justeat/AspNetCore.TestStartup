using System;
using AspNetCore.TestStartup.UnitTests.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Xunit;


namespace AspNetCore.TestStartup.UnitTests
{
    public class UseTestStartupTests
    {
        private class Startup : IStartup
        {
            public IServiceProvider ConfigureServices(IServiceCollection services)
            {
                services.AddTransient<IInterface, OriginalImplementation>();

                return services.BuildServiceProvider();
            }

            public void Configure(IApplicationBuilder app)
            {

            }
        }
        
        [Fact]
        public void ConfigureServices_WithOverride_ReturnsOriginalImplementation()
        {
            var webHost = new WebHostBuilder()
                .UseEnvironment("Test")
                .UseServer(new FakeServer())
                .UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    services.AddTransient<IInterface, TestImplementation>();
                })
                .Build();

            var implementation = webHost.Services.GetRequiredService<IInterface>();

            Assert.Equal(implementation.GetType(), typeof(OriginalImplementation));
        }

        [Fact]
        public void VanillaStartup_ReturnsOriginalImplementation()
        {
            var webHost = new WebHostBuilder()
                .UseEnvironment("Test")
                .UseServer(new FakeServer())
                .UseStartup<Startup>()
                .Build();

            var implementation = webHost.Services.GetRequiredService<IInterface>();

            Assert.Equal(implementation.GetType(), typeof(OriginalImplementation));
        }

        [Fact]
        public void TestStartup_ReturnsTestImplementation()
        {
            var webHost = new WebHostBuilder()
                .UseEnvironment("Test")
                .UseServer(new FakeServer())
                .UseTestStartup<Startup>(services =>
                {
                    services.AddTransient<IInterface, TestImplementation>();
                })
                .Build();

            var implementation = webHost.Services.GetRequiredService<IInterface>();

            Assert.Equal(implementation.GetType(), typeof(TestImplementation));
        }
    }
}
