using Digevo.Viral.Gateway.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Digevo.Viral.Gateway.Models
{
    public class ViralDataContext : DbContext
    {
        public ViralDataContext()
            : base("DefaultConnection")
            {

            }

        public DbSet<Campaign> Campaigns { get; set; }

        public DbSet<ShareIntent> ShareIntents { get; set; }

        public DbSet<ShareCallback> ShareCallbacks { get; set; }

        public DbSet<TriggerClaim> TriggerClaims { get; set; }
    }
}