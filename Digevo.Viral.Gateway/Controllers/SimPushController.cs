using Digevo.Viral.Gateway.Models;
using Digevo.Viral.Gateway.Models.Entities;
using Digevo.Viral.Gateway.Models.Infrastructure.Extensions;
using Digevo.Viral.Gateway.Models.Infrastructure.Services.UrlShortener;
using Digevo.Viral.Gateway.Models.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Digevo.Viral.Gateway.Controllers
{
    public class SimPushController : ApiController
    {
        private readonly ViralDataContext context = new ViralDataContext();

        // GET ?p=56998875899&c=2
        [HttpGet]
        public async Task<HttpResponseMessage> Get(string wildcard, string p = "", int c = 0 )
        {
            string alias = p;
            string targetUrl = ConfigurationSettings.DefaultErrorPage;

            c = c == 0 ? 7 : c;

            try
            {
                long phone = NumericCompressionAlgorithm.Decompress(alias);
                int campaignId = c;


                var campaign = context.Campaigns.FirstOrDefault(camp => camp.ID == campaignId);

                if (campaign == null)
                {
                    Exception ex = new Exception("No se pudo encontrar la url corta para campaña: " + campaignId);
                    LogExtensions.Log.ErrorCall(ex, () => new { alias, campaignId });
                    throw ex;
                }

                targetUrl = campaign.TargetAddress.FormatWith(new { phone = phone, alias = alias });

                context.ShareCallbacks.Add(new ShareCallback()
                {
                    Campaign = campaign,
                    CallbackUserHandle = phone.ToString(),
                    Timestamp = DateTimeOffset.Now
                });

                context.SaveChanges();
                LogExtensions.Log.InfoCall(() => new { Controller = "SimPushController", alias, phone });

            }
            catch (Exception ex)
            {
                LogExtensions.Log.ErrorCall(ex, () => new { alias, c, p, ex });
            }

            var response = new HttpResponseMessage(HttpStatusCode.Redirect);
            response.Headers.Location = new Uri(targetUrl);
            return response;
        }


        private async Task<ShareIntent> TriggerCampaignIntentAsync(int id, string user, string medium)
        {
            var campaign = context.Campaigns.Include("OnShareIntentTriggers").FirstOrDefault(x => x.ID == id);
            if (campaign == null)
                throw new ArgumentException("No campaign was found with an ID: " + id);

            //Run all triggers
            var tasks = new List<Task>();
            foreach (var triggerIntent in campaign.OnShareIntentTriggers)
            {
                if (triggerIntent.IsOneTimeOnly && context.TriggerClaims.Any(tc => tc.TriggerID == triggerIntent.ID && tc.UserHandle == user))
                    continue;

                tasks.Add(triggerIntent.Execute(user));
                context.TriggerClaims.Add(new TriggerClaim() { UserHandle = user, Trigger = triggerIntent, Campaign = campaign });
            }
            await Task.WhenAll(tasks);

            var intent = new ShareIntent()
            {
                SeedUserHandle = user,
                Timestamp = DateTimeOffset.Now,
                ViralMedium = medium,
                Campaign = campaign
            };

            context.ShareIntents.Add(intent);
            context.SaveChanges();

            return intent;
        }

        private async Task<string> ShortenUrl(string url, ShareIntent shareIntent)
        {
            IUrlShortener service = new WmlUrlShortener();
            shareIntent.ShortenedUrl = await service.ShortenUrlAsync(url, "", NumericCompressionAlgorithm.Compress(shareIntent.ID));
            shareIntent.TargetUrl = url;
            context.SaveChanges();

            LogExtensions.Log.DebugCall(() => new { url, shareIntent.ShortenedUrl });

            return shareIntent.ShortenedUrl;
        }
    }
}
