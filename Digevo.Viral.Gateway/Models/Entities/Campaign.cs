using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Digevo.Viral.Gateway.Models.Entities
{
    /// <summary>
    /// Defines the triggers that execute while the target is shared accros different intents while the Campaign is active
    /// </summary>
    public class Campaign
    {
        public int ID
        {
            get;
            set;
        }

        [MaxLength(250)]
        public string Name
        {
            get;
            set;
        }

        public DateTimeOffset StartDate
        {
            get;
            set;
        }

        public DateTimeOffset? EndDate
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }

        public List<Trigger> OnShareIntentTriggers
        {
            get;
            set;
        }

        public List<Trigger> OnShareCallbackTriggers
        {
            get;
            set;
        }

        public List<Trigger> OnShareConversionTriggers
        {
            get;
            set;
        }

        public string TargetAddress
        {
            get;
            set;
        }

        public List<ShareIntent> ShareIntents
        {
            get;
            set;
        }
    }
}