using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digevo.Viral.Gateway.Models.Infrastructure.Services.UrlShortener
{
    internal interface IUrlShortener
    {
        Task<string> ShortenUrlAsync(string url, string prefix = "", string alias = "");
    }
}
