using Digevo.Viral.Gateway.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Digevo.Viral.Gateway.Models
{
    /// <summary>
    /// The event when a user reacts based on a ShareIntent is registered on a ShareCallback
    /// </summary>
    public class ShareCallback
    {
        /// <summary>
        /// ID that identifies callback
        /// </summary>
        public int ID
        {
            get;
            set;
        }

        public Campaign Campaign
        {
            get;
            set;
        }

        public ShareIntent ShareIntent
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
        public string ReferalUserHandle
        {
            get;
            set;
        }

        public bool IsConversion
        {
            get;
            set;
        }

        [MaxLength(250)]
        public string CallbackUserHandle
        {
            get;
            set;
        }
    }
}