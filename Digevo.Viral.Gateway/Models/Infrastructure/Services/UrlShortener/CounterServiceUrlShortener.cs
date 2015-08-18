using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Digevo.Viral.Gateway.Models.Infrastructure.Extensions;
using System.Web;

namespace Digevo.Viral.Gateway.Models.Infrastructure.Services.UrlShortener
{
    public class CounterServiceUrlShortener : IUrlShortener
    {
        static readonly string ShortenBaseUrl = "http://viral.wml.cl/{Alias}";
        static readonly string AliasService = "http://146.82.89.99:9090/CounterService/Redirect?Operation=register&WapUrl={BaseUrl}&WebUrl={BaseUrl}&ObjectID={Prefix}{Alias}";

        /// <summary>
        /// Shortens and returns a url with the specified prefix, if any
        /// </summary>
        /// <param name="url">Url to shorten</param>
        /// <param name="prefix">Prefix to prepend to the shortened url</param>
        /// <param name="postfix">Postfix to append to the shortened url</param>
        /// <returns>Shortened url</returns>
        public async Task<string> ShortenUrlAsync(string url, string prefix = "", string alias = "")
        {
            var serviceCall = AliasService.FormatWith(
                new { BaseUrl = url, Prefix = prefix, Alias = alias });

            var request = (HttpWebRequest)WebRequest.Create(serviceCall);
            var response = (HttpWebResponse)await request.GetResponseAsync();

            if (!new StreamReader(response.GetResponseStream()).ReadToEnd().Contains(url))
                throw new HttpUnhandledException("Unable to shorten the url with the alias service: " + serviceCall);

            return ShortenBaseUrl.FormatWith(new { Alias = prefix + alias });
        }
    }
}