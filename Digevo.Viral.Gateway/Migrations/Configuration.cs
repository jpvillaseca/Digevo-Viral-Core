namespace Digevo.Viral.Gateway.Migrations
{
    using Digevo.Viral.Gateway.Models.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;
    using Digevo.Viral.Gateway.Models.Infrastructure;
    using Digevo.Viral.Gateway.Models.Infrastructure.Triggers;

    internal sealed class Configuration : DbMigrationsConfiguration<Digevo.Viral.Gateway.Models.ViralDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Digevo.Viral.Gateway.Models.ViralDataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //

            var campaign = new Campaign { 
                  Name = "DebugCampaign",
                  IsActive = true,
                  StartDate = DateTimeOffset.Now,
                  TargetAddress = "http://famososenlamira.com.tempdomain.com"
              };

            context.Campaigns.AddOrUpdate(
              c => c.Name,
              campaign
            );

            campaign.OnShareIntentTriggers = new List<Trigger>();

            AddPointsTriggerFactory factory = new AddPointsTriggerFactory(Models.Enums.TriggerType.AddPoint, "http://146.82.89.99:9090","ViralCorePointsDebug",13);

            campaign.OnShareIntentTriggers.Add(factory.CreateTrigger(false));

            context.SaveChanges();

            //context.ShareIntents.AddOrUpdate(si => si.SeedUserHandle,
            //    new ShareIntent()
            //    {
            //        Campaign = campaign,
            //        SeedUserHandle = "jvillaseca",
            //        Timestamp = DateTimeOffset.Now,
            //        ViralMedium = "DebugMedium"
            //    }
            //    );
        }
    }
}
