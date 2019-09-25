using Line.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechSummit2018.ServerlessSmartSpeaker.Services;
using XPlat.VUI;
using XPlat.VUI.Models;

namespace SmartSpeakerGetLatestArticle
{
    public class BlogAssistant : AssistantBase
    {
        private static string IntroductionMessage { get; } = "こんにちは、LINEデベロッパー・デイのデモアプリです。最新記事を教えてと聞いてください。";
        private static string HelloMessage { get; } = "こんにちは、ちょまどさん！";
        private static string ErrorMessage { get; } = "すみません、わかりませんでした！";

        private ChomadoBlogService Service { get; }
        private ILineMessagingClient MessagingClient { get; }

        public BlogAssistant(ChomadoBlogService service, ILineMessagingClient messagingClient)
        {
            Service = service;
            MessagingClient = messagingClient;
        }

        protected override Task OnLaunchRequestAsync(Dictionary<string, object> session, CancellationToken cancellationToken)
        {
            Response
                .Speak(IntroductionMessage)
                .KeepListening();
            return Task.CompletedTask;
        }

        protected override  async Task OnIntentRequestAsync(string intent, Dictionary<string, object> slots, Dictionary<string, object> session, CancellationToken cancellationToken)
        {
            switch (intent)
            {
                case "HelloIntent":
                    Response.Speak(HelloMessage);
                    break;

                case "AskLatestBlogTitleIntent":
                    var blog = await Service.GetLatestBlogAsync();

                    if (blog != null)
                    {
                        Response.Speak($"ちょまどさんのブログの最新記事は {blog.Title} です。");

                        // Clova の場合は LINE にプッシュ通知する
                        if (Request.CurrentPlatform == Platform.Clova)
                        {
                            _ = MessagingClient.PushMessageAsync(
                                to: Request.UserId,
                                messages: new List<ISendMessage>
                                {
                                    new TextMessage($"ちょまどさんの最新記事はこちら！"),
                                    new TextMessage($@"タイトル『{blog.Title}』
{blog.Url}"),
                                });
                        }
                    }
                    else
                    {
                        Response.Speak("ちょまどさんのブログの最新記事は、わかりませんでした。");
                    }
                    break;

                default:
                    Response.Speak(ErrorMessage);
                    break;
            }
        }
    }
}
