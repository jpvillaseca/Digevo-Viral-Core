using Digevo.Viral.Gateway.Models;
using Digevo.Viral.Gateway.Models.Entities;
using Digevo.Viral.Gateway.Models.Infrastructure.Extensions;
using Digevo.Viral.Gateway.Models.Infrastructure.Settings;
using Digevo.Viral.Gateway.Models.Infrastructure.UrlShortener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Digevo.Viral.Gateway.Controllers
{
    public class ViewIntentController : ApiController
    {
        private readonly ViralDataContext context = new ViralDataContext();

        // GET api/viewintent?alias=7aqC&conversion=true
        [HttpGet]
        public async Task<HttpResponseMessage> Get(string alias, bool isConversion = false)
        {
            string targetUrl = ConfigurationSettings.DefaultErrorPage;

            try
            {
                long intentId = NumericCompressionAlgorithm.Decompress(alias);
                var intent = context.ShareIntents
                    .Include("Campaign")
                    .Include("Campaign.OnShareConversionTriggers")
                    .Include("Campaign.OnShareCallbackTriggers")
                    .FirstOrDefault(s => s.ID == intentId);

                if (intent == null)
                {
                    Exception ex = new Exception("No se pudo encontrar la url corta con alias: " + alias);
                    LogExtensions.Log.ErrorCall(ex, () => new { alias, isConversion });
                    throw ex;
                }

                string user = ""; //TODO: Add HTTP parsing logic somewhere to get user
                await TriggerCampaignCallbackAsync(intent, user, isConversion);

                targetUrl = intent.TargetUrl;
            }
            catch (Exception ex)
            {
                LogExtensions.Log.ErrorCall(ex, () => new { alias, isConversion, ex });
            }

            var response = new HttpResponseMessage(HttpStatusCode.Redirect);
            response.Headers.Location = new Uri(targetUrl);
            return response;
        }

        private async Task<ShareIntent> TriggerCampaignCallbackAsync(ShareIntent intent, string user, bool isConversion)
        {
            //Run all triggers
            var tasks = new List<Task>();

            var triggers = isConversion ? intent.Campaign.OnShareConversionTriggers : intent.Campaign.OnShareCallbackTriggers;

            foreach (var callback in triggers)
            {
                if (callback.IsOneTimeOnly && context.TriggerClaims.Any(tc => tc.TriggerID == callback.ID && tc.UserHandle == user))
                    continue;

                tasks.Add(callback.Execute(user));
                context.TriggerClaims.Add(new TriggerClaim() { UserHandle = user, Trigger = callback, Campaign = intent.Campaign });
            }

            await Task.WhenAll(tasks);

            var shareCallback = new ShareCallback()
            {
                CallbackUserHandle = user,
                Campaign = intent.Campaign,
                IsConversion = isConversion, //TODO: Check if it is conversion
                ReferalUserHandle = intent.SeedUserHandle,
                ShareIntent = intent,
                Timestamp = DateTimeOffset.Now
            };

            context.ShareCallbacks.Add(shareCallback);
            await context.SaveChangesAsync();

            return intent;
        }
    }
}
