using Digevo.Viral.Gateway.Models;
using Digevo.Viral.Gateway.Models.Infrastructure.Extensions;
using Digevo.Viral.Gateway.Models.Infrastructure.Services.SecureServices;
using Digevo.Viral.Gateway.Models.Infrastructure.Settings;
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
        [Route("api/FuenteDeseo/Message")]
        public void Post(string phone, int arcane)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return;

            try
            {
                var text = System.Configuration.ConfigurationManager.AppSettings["FuenteDeseo.Arcane." + arcane.ToString()];
                var settings = Settings;

                Random r = new Random();
                var rand = (new Random()).Next(int.MaxValue);
                LogExtensions.Log.InfoCall(() => new { phone, smsText = text, transactionId = rand });
                SecureServicesMt.SendMT(new LoginParams(rand, settings.Credentials.Login, settings.Credentials.Password, Settings.ServiceEndpoint), new MtParams(text, settings.MT.NC), new PhoneParams(phone.Replace("+", string.Empty).Trim(), settings.MT.Op));
            }
            catch (Exception ex)
            {
                LogExtensions.Log.ErrorCall(ex, () => new { phone, arcane });
            }
        }

        private SecureServicesMtSection Settings
        {
            get
            {
                return (SecureServicesMtSection)System.Configuration.ConfigurationManager.GetSection(
                    "secureServices/fuenteDeseo.secureServicesMt");
            }
        }
    }
}
