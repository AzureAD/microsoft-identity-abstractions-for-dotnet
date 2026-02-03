// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Identity.Abstractions.AotTests;

class Program
{
    private const string ApiName = "Api1";

    static void Main()
    {
        // For each change, you need to run the following
        // run `dotnet publish --runtime win-x64 -f net10.0`
        // and then `.\bin\Release\net10.0\win-x64\publish\Microsoft.Identity.Abstractions.AotTests.exe`
        IHostBuilder hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                var azureAdSection = context.Configuration.GetSection("AzureAd");
                services.Configure<MicrosoftIdentityApplicationOptions>("scheme", options => azureAdSection.Bind(options));
                services.Configure<DownstreamApiOptions>(ApiName,
                    options => context.Configuration.GetSection($"DownstreamApis:{ApiName}").Bind(options)
                    );
            });


        IHost host = hostBuilder.Build();

        IOptionsMonitor<MicrosoftIdentityApplicationOptions> options = host.Services.GetRequiredService<IOptionsMonitor<MicrosoftIdentityApplicationOptions>>();

        MicrosoftIdentityApplicationOptions optionsInstance = options.Get("scheme");

        if (string.IsNullOrEmpty(optionsInstance?.ClientId))
        {
            throw new InvalidOperationException("could not bind client id");
        }
        else
        {
            Console.WriteLine($"ClientId: {optionsInstance.ClientId}");
            Console.WriteLine($"ClientCredentials: {optionsInstance.ClientCredentials!.First().Id}");
        }

        IOptionsMonitor<DownstreamApiOptions> downstreamApisOptions = host.Services.GetRequiredService<IOptionsMonitor<DownstreamApiOptions>>();
        var option = downstreamApisOptions.Get(ApiName);
        Console.WriteLine($"DownstreamApi: {option.BaseUrl}");

        Console.WriteLine("AOT test completed successfully!");
    }
}
