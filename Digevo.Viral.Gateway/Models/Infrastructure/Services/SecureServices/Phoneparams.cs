using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digevo.Viral.Gateway.Models.Infrastructure.Services.SecureServices
{
    public class PhoneParams
    {
        public PhoneParams(string ani, string op)
        {
            this.ANI = ani;
            this.Op = op;
        }
        public string ANI { get; set; }
        public string Op { get; set; }
    }
}