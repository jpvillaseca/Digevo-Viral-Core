using Digevo.Viral.Gateway.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Digevo.Viral.Gateway.Models.Entities
{
    /// <summary>
    /// A ShareIntent is created when a user decides to share a Campaign
    /// </summary>
    public class ShareIntent
    {
        public int ID
        {
            get;
            set;
        }

        public DateTimeOffset Timestamp
        {
            get;
            set;
        }

        [MaxLength(250)]
        public string SeedUserHandle
        {
            get;
            set;
        }

        public string ViralMedium
        {
            get;
            set;
        }

        public List<ShareCallback> ShareCallbacks
        {
            get;
            set;
        }

        public Campaign Campaign
        {
            get;
            set;
        }

        public string ShortenedUrl { get; set; }

        public string TargetUrl { get; set; }

    }
}