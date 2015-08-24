using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Digevo.Viral.Gateway.Models.Infrastructure;
using Digevo.Viral.Gateway.Models.Infrastructure.Extensions;
using System.Web.WebPages;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Digevo.Viral.Gateway.Models.Entities
{
    /// <summary>
    /// An action that executes when a Campaign is configured based on new ShareIntents or ShareCallbacks
    /// </summary>
    public class Trigger
    {
        public int ID
        {
            get;
            set;
        }

        public Enums.TriggerType ActionType
        {
            get;
            set;
        }

        public string TargetAddress
        {
            get;
            set;
        }

        public bool IsOneTimeOnly
        {
            get;
            set;
        }

        private string _httpMethod;

        [MaxLength(30)]
        public string HttpMethod
        {
            get { return string.IsNullOrWhiteSpace(_httpMethod) ? "GET" : _httpMethod; }
            set { _httpMethod = value; }
        }

        /// <summary>
        /// Executes a trigger interpolating every included parameter in the TargetAddress, such as {UserHandle}.  
        /// </summary>
        /// <param name="UserHandle">User handle (username, phone)</param>
        /// <returns>True if succesful, false othwerwise</returns>
        public async Task<bool> Execute(string UserHandle, string SeedUserHandle = "")
        {
            dynamic triggerParameters = new
            {
                UserHandle = UserHandle,
                SeedUserHandle = SeedUserHandle
            };

            return await Execute(triggerParameters);
        }
        
        /// <summary>
        /// Executes a trigger interpolating every included parameter in the TargetAddress, such as {UserHandle}.  
        /// </summary>
        /// <param name="triggerParameters">Parameters to interpolate in the trigger service call</param>
        /// <returns>True if succesful, false othwerwise</returns>
        public async Task<bool> Execute(dynamic triggerParameters)
        {
            try
            {
                var interpolatedTriggerAddress = StringFormatExtensions.FormatWith(TargetAddress, triggerParameters);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(interpolatedTriggerAddress);
                request.Method = HttpMethod;

                if (HttpMethod == "POST")
                {
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = 0;
                }
                    
                var response = (HttpWebResponse)await request.GetResponseAsync();

                LogExtensions.Log.InfoCall(() => new { Operaton = "Executing trigger with params", triggerParameters });

                return (int)response.StatusCode >= 200 && (int)response.StatusCode <= 399;
            }
            catch (Exception ex)
            {
                LogExtensions.Log.ErrorCall(ex, () => new { triggerParameters, ID, TargetAddress });

                return false;
            }
        }
    }
}