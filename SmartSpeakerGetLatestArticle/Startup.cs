using Line.Messaging;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using TechSummit2018.ServerlessSmartSpeaker.Services;
using XPlat.VUI;

[assembly: FunctionsStartup(typeof(SmartSpeakerGetLatestArticle.Startup))]
namespace SmartSpeakerGetLatestArticle
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .AddSingleton<ILineMessagingClient, LineMessagingClient>(_ =>
                     new LineMessagingClient(Environment.GetEnvironmentVariable("LineMessagingApiSecret")))
                .AddSingleton<ChomadoBlogService>()
                .AddAssistant<IAssistant, BlogAssistant>();
        }
    }
}