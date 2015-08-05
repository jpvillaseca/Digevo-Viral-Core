using Digevo.Viral.Gateway.Models.Entities;
using Digevo.Viral.Gateway.Models.Enums;
using Digevo.Viral.Gateway.Models.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digevo.Viral.Gateway.Models.Infrastructure.Triggers
{
    internal class AddPointsTriggerFactory : AbstractTriggerFactory
    {
        private const string AddPointsService = "/CounterService/Config?Operation=AddValueByANI&ani={UserHandle}&AttributeName={PointsConfigurationName}&AttributeValue={PointsToAdd}&AttributeType=N";

        private string PointsConfigurationName { get; set; }
        
        private int PointsToAdd { get; set; }

        internal AddPointsTriggerFactory(TriggerType triggerType, string serviceEndpoint, string pointsConfigurationName, int pointsToAdd) : base(triggerType, serviceEndpoint)
        {
            PointsConfigurationName = pointsConfigurationName;
            PointsToAdd = pointsToAdd;
        }

        internal override Trigger CreateTrigger(bool isOneTimeOnly)
        {
            Trigger trigger = new Trigger();
            trigger.ActionType = base.TriggerType;
            trigger.IsOneTimeOnly = isOneTimeOnly;
            var builder = new UriBuilder(new Uri(EndpointUri,
                AddPointsService.FormatWith(new { UserHandle = "{UserHandle}", PointsConfigurationName = PointsConfigurationName, PointsToAdd = PointsToAdd })));

            trigger.TargetAddress = builder.Uri.AbsoluteUri;
            return trigger;
        }
    }
}