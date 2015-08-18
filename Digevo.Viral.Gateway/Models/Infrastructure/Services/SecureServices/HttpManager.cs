using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Digevo.Viral.Gateway.Models.Infrastructure.Services.SecureServices
{
    public class HttpManager
    {
        /// <summary>
        /// Invoca una url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="metodo"></param>
        /// <returns></returns>
        public static string RequestUrl(string url, string data, string metodo)
        {
            string respuesta;
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = metodo;
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; MS-RTC LM 8)";

                Encoding encoding = Encoding.GetEncoding("utf-8");
                if (!string.IsNullOrEmpty(data))
                {
                    byte[] bytes = encoding.GetBytes(data);
                    req.ContentType = "text/xml";
                    req.ContentLength = bytes.Length;
                    Stream dataStream = req.GetRequestStream();
                    dataStream.Write(bytes, 0, bytes.Length);
                    dataStream.Close();
                }

                using (var resp = (HttpWebResponse)req.GetResponse())
                {
                    using (var sr = new StreamReader(resp.GetResponseStream(), encoding))
                    {
                        respuesta = sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta = ex.Message;
            }

            return respuesta;
        }


        /// <summary>
        /// Arma una url encodeando los parámetros
        /// </summary>
        /// <param name="urlBase"></param>
        /// <param name="prms"></param>
        /// <param name="urlConParametros"></param>
        /// <returns></returns>
        public static string BuildUrl(string urlBase, Dictionary<string, string> prms, bool urlConParametros)
        {
            var url = new StringBuilder(urlBase);

            url.Append(urlConParametros ? '&' : '?');

            foreach (KeyValuePair<string, string> entry in prms)
            {
                url.AppendFormat("{0}={1}&", entry.Key, HttpUtility.UrlEncode(entry.Value));
            }

            return url.ToString(0, url.Length - 1);
        }
    }
}