# AspNetCore.TestStartup

[![NuGet Version](https://img.shields.io/nuget/vpre/AspNetCore.TestStartup.svg?style=flat-square)](https://www.nuget.org/packages/AspNetCore.TestStartup)
[![NuGet Downloads](https://img.shields.io/nuget/dt/AspNetCore.TestStartup.svg?style=flat-square)](https://www.nuget.org/packages/AspNetCore.TestStartup)
[![AppVeyor Build Status](https://img.shields.io/appveyor/ci/justeattech/aspnetcore-teststartup/master.svg?style=flat-square)](https://ci.appveyor.com/project/justeattech/aspnetcore-teststartup)


## Example Usage

```c#
//Ensure that Startup implements : IStartup

var server = new TestServer(new WebHostBuilder()
                                .UseTestStartup<Startup>(services =>
                                {
                                    services.AddTransient(p => MoqSomethingService.Object);
                                }));

var httpClient = server.CreateClient();

// Do Testing here
```
