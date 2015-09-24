using Digevo.Viral.Gateway.Models;
using Digevo.Viral.Gateway.Models.Entities;
using Digevo.Viral.Gateway.Models.Infrastructure.Extensions;
using Digevo.Viral.Gateway.Models.Infrastructure.Services.UrlShortener;
using Digevo.Viral.Gateway.Models.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Digevo.Viral.Gateway.Controllers
{
    public class ShortenUrlController : ApiController
    {
        private readonly ViralDataContext context = new ViralDataContext();

        // GET url.com/campaign/alias
        [HttpGet]
        public HttpResponseMessage Get(int campaignId = 0, string alias = "")
        {
            string targetUrl = ConfigurationSettings.DefaultErrorPage;

            try
            {
                long phone = NumericCompressionAlgorithm.Decompress(alias);

                var campaign = context.Campaigns.FirstOrDefault(camp => camp.ID == campaignId);

                if (campaign == null)
                {
                    Exception ex = new Exception("No se pudo encontrar la url corta para campaña: " + campaignId);
                    LogExtensions.Log.ErrorCall(ex, () => new { alias, campaignId });
                    throw ex;
                }

                var ua = Request.Headers.UserAgent.ToString().ToLower();
                var mobileDevice = ua.Contains("iphone") ? MobileDevice.iOS : ua.Contains("android") ? MobileDevice.Android : ua.Contains("windows phone") ? MobileDevice.WindowsPhone : MobileDevice.Other;

                if (!campaign.IsActive || (campaign.EndDate.HasValue && DateTimeOffset.Now > campaign.EndDate.Value))
                    targetUrl = campaign.TargetAddressInactiveCampaign;
                else if (mobileDevice == MobileDevice.iOS && !string.IsNullOrWhiteSpace(campaign.TargetAddressIOS))
                    targetUrl = campaign.TargetAddressIOS;
                else if (mobileDevice == MobileDevice.Android && !string.IsNullOrWhiteSpace(campaign.TargetAddressAndroid))
                    targetUrl = campaign.TargetAddressAndroid;
                else if (mobileDevice == MobileDevice.WindowsPhone && !string.IsNullOrWhiteSpace(campaign.TargetAddressWindowsPhone))
                    targetUrl = campaign.TargetAddressWindowsPhone;
                else targetUrl = campaign.TargetAddress;

                targetUrl = targetUrl.FormatWith(new { phone = phone, alias = alias, device = mobileDevice });

                context.ShareCallbacks.Add(new ShareCallback()
                {
                    Campaign = campaign,
                    CallbackUserHandle = phone.ToString(),
                    Timestamp = DateTimeOffset.Now,
                    IP = HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : "",
                    UserAgent = Request.Headers.UserAgent.ToString(),
                    IsConversion = false,
                    OperatingSystem = mobileDevice.ToString(),
                    TargetUrlRedirection = targetUrl
                });

                context.SaveChanges();
                LogExtensions.Log.InfoCall(() => new { Controller = "ShortenUrlController", alias, phone });

            }
            catch (Exception ex)
            {
                LogExtensions.Log.ErrorCall(ex, () => new { alias, campaignId, ex });
            }

            var response = new HttpResponseMessage(HttpStatusCode.Redirect);
            response.Headers.Location = new Uri(targetUrl);
            return response;
        }
    }

    public enum MobileDevice
    {
        iOS,
        Android,
        WindowsPhone,
        Other
    }
}
