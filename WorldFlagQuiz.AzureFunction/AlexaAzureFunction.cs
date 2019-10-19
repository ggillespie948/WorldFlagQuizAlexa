using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Alexa.NET;
using Microsoft.IdentityModel.Protocols;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Alexa.NET.Security.Functions;
using WorldFlagQuiz.AzureFunction.Utility;
using Alexa.NET.APL;
using Alexa.NET.Response.APL;
using Alexa.NET.APL.Components;

namespace WorldFlagQuiz.AzureFunction
{
    public class AlexaAzureFunction : SkillResponse
    {
        //TEMPPPPPPP
        static string scotlandFlagURL = "https://en.wikipedia.org/wiki/Flag_of_Scotland#/media/File:Flag_of_Scotland.svg";

        [FunctionName("Alexa")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
                    HttpRequest req,
                   ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed an alexa post request.");

            //deserialize skill request
            string json = await req.ReadAsStringAsync();
            var skillRequest = JsonConvert.DeserializeObject<APLSkillRequest>(json);

            //Validate Request
            // Verifies that the request is a valid request from Amazon Alexa 
            var isValid = await skillRequest.ValidateRequestAsync(req, log);
            if (!isValid)
                return new BadRequestResult();
            
            RenderDocumentDirective.AddSupport();
            ExecuteCommandsDirective.AddSupport();

            //init respoonse and get current user session
            SkillResponse response = null;
            Session session = skillRequest.Session;

            //check for APL support
            if (skillRequest.APLSupported())
            {
                //add support for APL Commands +  Directives
                new UserEventRequestHandler().AddToRequestConverter();
                skillRequest = JsonConvert.DeserializeObject<APLSkillRequest>(json);
                var aplDocumentVersion = skillRequest.APLInterfaceDetails().Runtime.MaxVersion;
                //add apl layout
                var document = new APLDocument();
                document.AddResponsiveDesign();

                document.MainTemplate = new Layout(
                    new Container(
                        new Text("APL in C# TEST!") { FontSize = "24vh", TextAlign = "Center" },
                        new Image(scotlandFlagURL) { Width = "100vw", Height = "100vw", Scale = Scale.BestFit }
                    )
                    { Direction = "row", Position = "absolute" })
                .AsMain();

                var directive = new RenderDocumentDirective
                {
                    Token = "randomToken",
                    Document = new APLDocument
                    {
                        MainTemplate = new Layout(new[]
                        {
                        new Container(new APLComponent[]{
                            new Text("APL in C#"){FontSize = "24dp",TextAlign= "Center"},
                            new Image("https://images.example.com/photos/2143/lights-party-dancing-music.jpg?cs=srgb&dl=cheerful-club-concert-2143.jpg&fm=jpg"){Width = 400,Height=400}
                        }){Direction = "row"}
                    })
                    }
                };

                response.Response.Directives.Add(directive);
            }


            switch (skillRequest.Request)
            {
                case LaunchRequest launchRequest:
                    return HandleLaunch(launchRequest, log);
                case IntentRequest intentRequest:
                    switch (intentRequest.Intent.Name)
                    {
                        case "NewQuizIntent":
                            return NewQuizIntent();

                        default:
                            return HandleLaunch(null, log);
                    }
            }

            return new OkObjectResult(response);
        }



        private static ObjectResult HandleLaunch(LaunchRequest launchRequest, ILogger log)
        {
            log.LogInformation("Handle launch request");
            var responseSpeach = "";
            responseSpeach += "Welcome to the World Flag Quiz Alexa skill!";

            Session session = new Session();

            if (session.Attributes == null)
                session.Attributes = new Dictionary<string, object>();

            Reprompt reprompt = new Reprompt(responseSpeach);


            return AlexaResponseUtility.BuildResponse(responseSpeach, true, session, reprompt);
        }

        private static ObjectResult NewQuizIntent()
        {
            throw new NotImplementedException();
        }

        private static ObjectResult PremiumServiceInfoIntent()
        {
            throw new NotImplementedException();
        }

        private static ObjectResult AnswerIntent()
        {
            throw new NotImplementedException();
        }

        private static ObjectResult StopIntent()
        {
            throw new NotImplementedException();
        }

        private static ObjectResult PauseIntent()
        {
            throw new NotImplementedException();
        }


        #region Generic
        private static string ConvertTimeTo24Hour(string raceTime, ref ILogger log)
        {
            //check if numbers left of colon parsed are less than 12
            try
            {
                log.LogInformation("trying to parse:   " + raceTime.Substring(0, 2));
                int givenTime = int.Parse(raceTime.Substring(0, 2), NumberStyles.Any);

                if (givenTime < 12)
                    givenTime = givenTime + 12;
                else
                    return raceTime;

                string newRaceTime = givenTime.ToString() + ":" + raceTime.Substring(3, 2);
                log.LogInformation("Converted race time string >" + newRaceTime + "<");

                return newRaceTime;

            }
            catch (Exception e)
            {
                log.LogError("Error Line Time Conversion: Could not parse time value ");
                return raceTime;
            }
        }




        #endregion
    }
}
