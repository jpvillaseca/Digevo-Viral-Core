using Digevo.Viral.Gateway.Models;
using Digevo.Viral.Gateway.Models.Entities;
using Digevo.Viral.Gateway.Models.Infrastructure.Extensions;
using Digevo.Viral.Gateway.Models.Infrastructure.Services.UrlShortener;
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
    public class ShareLinkController : ApiController
    {
        private readonly ViralDataContext context = new ViralDataContext();

        // GET api/sharelink/1?user=jvillaseca&medium=Facebook&url=http%3A%2F%2Ffamososenlamira.com.tempdomain.com
        [HttpGet]
        public async Task<HttpResponseMessage> Get(int id, string url, string user, string medium, string shareUrl)
        {
            string shortenedUrl = url; //Url is not shortened yet, but in case of an exception it's best to return the original url
            try
            {
                //Invoke any related triggers and shorten the url
                var shareIntent = await TriggerCampaignIntentAsync(id, user, medium);
                shortenedUrl = await ShortenUrl(shareUrl, shareIntent);
            }catch(Exception ex)
            {
                LogExtensions.Log.ErrorCall(ex, () => new { id, url, user, medium });
            }

            var response = new HttpResponseMessage(HttpStatusCode.Redirect);
            response.Headers.Location = new Uri(url.FormatWith(new { shareurl = HttpUtility.UrlEncode(shortenedUrl) }));
            return response;
        }


        private async Task<ShareIntent> TriggerCampaignIntentAsync(int id, string user, string medium)
        {
            var campaign = context.Campaigns.Include("OnShareIntentTriggers").FirstOrDefault(x => x.ID == id);
            if (campaign == null)
                throw new ArgumentException("No campaign was found with an ID: " + id);

            //Run all triggers
            var tasks = new List<Task>();
            foreach(var triggerIntent in campaign.OnShareIntentTriggers)
            {
                if (triggerIntent.IsOneTimeOnly && context.TriggerClaims.Any(tc => tc.TriggerID == triggerIntent.ID && tc.UserHandle == user))
                    continue;

                tasks.Add(triggerIntent.Execute(user));
                context.TriggerClaims.Add(new TriggerClaim() { UserHandle = user, Trigger = triggerIntent, Campaign = campaign });
            }
            await Task.WhenAll(tasks);

            var intent = new ShareIntent() { 
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
