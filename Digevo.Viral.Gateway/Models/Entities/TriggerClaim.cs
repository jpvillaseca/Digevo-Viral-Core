using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Digevo.Viral.Gateway.Models.Entities
{
    public class TriggerClaim
    {
        public int ID { get; set; }

        public int CampaignID { get; set; }

        public Campaign Campaign { get; set; }

        public int TriggerID { get; set; }
        
        public Trigger Trigger { get; set; }

        [MaxLength(250)]
        public string UserHandle { get; set; }
    }
}