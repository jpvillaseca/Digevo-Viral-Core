using Digevo.Viral.Gateway.Models.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Digevo.Viral.Gateway.Models.Infrastructure.Services.SecureServices
{
    public class SecureServicesMt
    {
        public static void SendMT(LoginParams secureParams, MtParams mtParams, PhoneParams phoneParams)
        {
            var doc = new XmlOutput();
            doc.XmlDeclaration().Node("request").Within()
                .Node("transaction").InnerText(secureParams.TransactionID.ToString())
                .Node("user").Within()
                    .Node("login").InnerText(secureParams.Login)
                    .Node("pwd").InnerText(secureParams.Password)
                .EndWithin()
                .Node("msg").Within()
                    .Node("format").InnerText(mtParams.Format)
                    .Node("text").InnerText(mtParams.Text, true)
                    .Node("url").InnerText(mtParams.URL)
                    .Node("nc").InnerText(mtParams.NumeroCorto)
                .EndWithin()
                .Node("phone").Within()
                    .Node("ani").InnerText(phoneParams.ANI)
                    .Node("op").InnerText(phoneParams.Op)
                .EndWithin()
            .EndWithin();

            SecureRequest(secureParams, doc.GetOuterXml());
        }

        public static decimal SecureRequest(LoginParams secureParams, string request)
        {
            try
            {
                string response = HttpManager.RequestUrl(secureParams.ServiceURL, request, "POST");
                //respuesta ="<response><code>0</code><description>Ok</description><transaction>411305926</transaction></response>";

                var document = XDocument.Parse(response);
                return decimal.Parse(document.Element("response").Element("transaction").Value);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}