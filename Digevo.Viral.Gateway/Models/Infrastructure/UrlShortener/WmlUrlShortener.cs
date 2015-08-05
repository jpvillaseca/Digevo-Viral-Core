using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digevo.Viral.Gateway.Models.Infrastructure.Extensions;
using System.Web;

namespace Digevo.Viral.Gateway.Models.Infrastructure.UrlShortener
{
    public class WmlUrlShortener : IUrlShortener
    {
        //static readonly string ShortenBaseUrl = "http://viral.wml.cl/{Alias}";
        static readonly string ShortenBaseUrl = "http://localhost:10273/api/viewintent?alias={Alias}";

        public async Task<string> ShortenUrlAsync(string url, string prefix = "", string alias = "")
        {
            return await Task.Run(() => ShortenBaseUrl.FormatWith(new { Alias = prefix + alias }));
        }
    }
}