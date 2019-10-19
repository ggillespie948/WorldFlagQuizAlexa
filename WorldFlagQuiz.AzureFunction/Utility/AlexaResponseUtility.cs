using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.APL;

namespace WorldFlagQuiz.AzureFunction.Utility
{
    public static class AlexaResponseUtility
    {
        public static ObjectResult BuildResponse(string responseSpeach, bool endSession, Session sessionAttributes, Reprompt reprompt)
        {
            SkillResponse skillResponse = new SkillResponse();

            skillResponse = ResponseBuilder.Ask(
                new PlainTextOutputSpeech
                {
                    Text = responseSpeach,
                },
                reprompt,
                sessionAttributes
            );

            skillResponse.Response.ShouldEndSession = endSession;

            return new OkObjectResult(skillResponse);
        }

        public static ObjectResult BuildResponse(string responseSpeach)
        {
            SkillResponse skillResponse = new SkillResponse();
            skillResponse = ResponseBuilder.Tell(new PlainTextOutputSpeech()
            {
                Text = responseSpeach,
            });

            skillResponse.Response.ShouldEndSession = true;

            return new OkObjectResult(skillResponse);
        }
    }
}
