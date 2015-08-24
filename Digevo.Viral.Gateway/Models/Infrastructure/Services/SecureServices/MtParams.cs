using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digevo.Viral.Gateway.Models.Infrastructure.Services.SecureServices
{
    public class MtParams
    {
        public MtParams(string text, string numeroCorto)
        {
            this.URL = string.Empty;
            this.Format = "0";
            this.Text = text;
            this.NumeroCorto = numeroCorto;
        }
        public string Format { get; set; }
        public string Text { get; set; }
        public string URL { get; set; }
        public string NumeroCorto { get; set; }
    }
}