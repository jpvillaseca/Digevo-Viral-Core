using Digevo.Viral.Gateway.Models.Entities;
using Digevo.Viral.Gateway.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digevo.Viral.Gateway.Models.Infrastructure.Triggers
{
    internal abstract class AbstractTriggerFactory
    {
        protected internal TriggerType TriggerType { get; set; }
        
        protected internal Uri EndpointUri { get; set; }

        protected internal AbstractTriggerFactory(TriggerType triggerType, string endpoint)
        {
            this.TriggerType = triggerType;
            this.EndpointUri = new Uri(endpoint);
        }

        internal abstract Trigger CreateTrigger(bool isOneTimeOnly);
    }
}