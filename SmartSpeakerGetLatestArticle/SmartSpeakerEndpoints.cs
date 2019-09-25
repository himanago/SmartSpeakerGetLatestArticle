using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using XPlat.VUI;
using XPlat.VUI.Models;

namespace TechSummit2018.ServerlessSmartSpeaker
{
    public class SmartSpeakerEndpoints
    {
        private IAssistant Assistant { get; }

        public SmartSpeakerEndpoints(IAssistant assistant)
        {
            Assistant = assistant;
        }

        [FunctionName("Line")]
        public async Task<IActionResult> Line(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req, ILogger log)
        {
            var response = await Assistant.RespondAsync(req, Platform.Clova);
            return new OkObjectResult(response.ToClovaResponse());
        }

        [FunctionName("GoogleHome")]
        public async Task<IActionResult> GoogleHome(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req, ILogger log)
        {
            var response = await Assistant.RespondAsync(req, Platform.GoogleAssistant);
            return new OkObjectResult(response.ToGoogleAssistantResponse());
        }

        [FunctionName("Alexa")]
        public async Task<IActionResult> Alexa(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req, ILogger log)
        {
            var response = await Assistant.RespondAsync(req, Platform.Alexa);
            return new OkObjectResult(response.ToAlexaResponse());
        }
    }
}
