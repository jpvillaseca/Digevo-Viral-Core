using Digevo.Viral.Gateway.Models;
using Digevo.Viral.Gateway.Models.Entities;
using Digevo.Viral.Gateway.Models.Entities.Landing;
using Digevo.Viral.Gateway.Models.Infrastructure.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Digevo.Viral.Gateway.Controllers
{
    public class LandingController : ApiController
    {
        private readonly ViralDataContext context = new ViralDataContext();

        // POST api/landing?campaignId=2
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<HttpResponseMessage> Post(int campaignId, [FromBody]string jsonUserData)
        {
            dynamic rawUserData = JsonConvert.DeserializeObject(jsonUserData);

            var userData = new UserData() { Name = rawUserData.Name, 
                Phone = rawUserData.Phone, 
                Birthday = rawUserData.Birthday, 
                Email = rawUserData.Email, 
                CreationDate = DateTimeOffset.Now };

            LogExtensions.Log.DebugCall(() => userData);

            var campaign = context.Campaigns.Include("OnShareConversionTriggers").FirstOrDefault(x => x.ID == campaignId);
            if (campaign == null)
                throw new ArgumentException("No campaign was found with an ID: " + campaignId);

            var landingUser = context.LandingUsers.FirstOrDefault(user => user.Phone == userData.Phone || (user.Email == userData.Email && user.Phone == null));
            if (landingUser == null)
            {
                landingUser = context.LandingUsers.Add(userData);
            }
            else
            {
                landingUser.Phone = string.IsNullOrWhiteSpace(userData.Phone) ? landingUser.Phone : userData.Phone;
                landingUser.Email = string.IsNullOrWhiteSpace(userData.Email) ? landingUser.Email : userData.Email;
                landingUser.Birthday = (userData.Birthday == null) ? landingUser.Birthday : userData.Birthday;
            }
            
            context.SaveChanges();

            await TriggerCampaignTriggersAsync(campaign, landingUser, rawUserData);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private async Task TriggerCampaignTriggersAsync(Campaign campaign, UserData user, dynamic rawUserData)
        {
            //Run all triggers
            var tasks = new List<Task>();
            foreach (var trigger in campaign.OnShareConversionTriggers)
            {
                if (trigger.IsOneTimeOnly && context.TriggerClaims.Any(tc => tc.TriggerID == trigger.ID && tc.UserHandle == user.Phone))
                    continue;

                await trigger.Execute(rawUserData);
                context.TriggerClaims.Add(new TriggerClaim() { UserHandle = user.Phone, Trigger = trigger, Campaign = campaign });
            }
            await Task.WhenAll(tasks);

            await context.SaveChangesAsync();
        }

    }
}
