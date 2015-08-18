using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Digevo.Viral.Gateway.Models.Infrastructure.Settings
{
    public class SecureServicesMtSection : ConfigurationSection
    {
        // Create a "remoteOnly" attribute.
        [ConfigurationProperty("serviceEndpoint", DefaultValue = "https://secure.3gmotion.com/Services/MT", IsRequired = false)]
        public String ServiceEndpoint
        {
            get
            {
                return (String)this["serviceEndpoint"];
            }
            set
            {
                this["serviceEndpoint"] = value;
            }
        }

        [ConfigurationProperty("credentials", IsRequired = true)]
        public SecureServicesCredentialsElement Credentials
        {
            get
            {
                return (SecureServicesCredentialsElement)this["credentials"];
            }
            set
            { this["credentials"] = value; }
        }

        [ConfigurationProperty("mt", IsRequired = true)]
        public SecureServicesMtElement MT
        {
            get
            {
                return (SecureServicesMtElement)this["mt"];
            }
            set
            { this["mt"] = value; }
        }

    }

    public class SecureServicesCredentialsElement : ConfigurationElement
    {
        [ConfigurationProperty("login", IsRequired = true)]
        public String Login
        {
            get
            {
                return (String)this["login"];
            }
            set
            {
                this["login"] = value;
            }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public String Password
        {
            get
            {
                return (String)this["password"];
            }
            set
            {
                this["password"] = value;
            }
        }
    }

    public class SecureServicesMtElement : ConfigurationElement
    {
        [ConfigurationProperty("nc", IsRequired = true)]
        public String NC
        {
            get
            {
                return (String)this["nc"];
            }
            set
            {
                this["nc"] = value;
            }
        }

        [ConfigurationProperty("op", IsRequired = true)]
        public String Op
        {
            get
            {
                return (String)this["op"];
            }
            set
            {
                this["op"] = value;
            }
        }
    }
}