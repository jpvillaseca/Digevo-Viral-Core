using Digevo.Viral.Gateway.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digevo.Viral.Gateway.Models.Infrastructure.Extensions
{
    public static class CampaignExtensions
    {
        public static  async Task<ShareIntent> TriggerCampaignIntentAsync(this Campaign campaign, ViralDataContext context, int id, string user, string medium)
        {
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
    }
}
