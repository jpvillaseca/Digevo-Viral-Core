using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digevo.Viral.Gateway.Models.Infrastructure.Settings
{
    public static class ConfigurationSettings
    {
        public static string DefaultErrorPage
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DefaultErrorPage"];
            }
        }
    }
}