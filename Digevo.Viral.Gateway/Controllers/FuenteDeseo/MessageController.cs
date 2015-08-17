using Digevo.Viral.Gateway.Models;
using Digevo.Viral.Gateway.Models.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Digevo.Viral.Gateway.Controllers.FuenteDeseo
{
    public class MessageController : ApiController
    {
        private readonly ViralDataContext context = new ViralDataContext();

        // POST api/message?phone=3123123&arcane=7
        [HttpPost]
        public void Post(string phone, int arcane)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return;
            
            //TODO: Enviar SMS
            LogExtensions.Log.DebugCall(() => new { phone, arcane });

            var text = System.Configuration.ConfigurationManager.AppSettings["FuenteDeseo.Arcane." + arcane.ToString()];

            LogExtensions.Log.DebugCall(() => new { phone, text = "SMS text: " + text });

        }
    }
}
