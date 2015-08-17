using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Digevo.Viral.Gateway.Models.Infrastructure;
using Digevo.Viral.Gateway.Models.Infrastructure.Extensions;
using System.Web.WebPages;

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

        /// <summary>
        /// Executes a trigger interpolating every included parameter in the TargetAddress, such as {UserHandle}.  
        /// </summary>
        /// <param name="UserHandle">User handle (username, phone)</param>
        /// <returns>True if succesful, false othwerwise</returns>
        public async Task<bool> Execute(string UserHandle, string SeedUserHandle = "")
        {
            try
            {
                var interpolatedTriggerAddress = TargetAddress.FormatWith(new
                {
                    UserHandle = UserHandle,
                    SeedUserHandle = SeedUserHandle
                });

                var request = (HttpWebRequest)WebRequest.Create(interpolatedTriggerAddress);
                var response = (HttpWebResponse)await request.GetResponseAsync();

                LogExtensions.Log.InfoCall(() => new { Operaton = "Executing trigger", UserHandle, interpolatedTriggerAddress, response.StatusCode });

                return (int)response.StatusCode >= 200 && (int)response.StatusCode <= 399;
            }
            catch(Exception ex)
            {
                LogExtensions.Log.ErrorCall(ex, () => new { UserHandle, ID, TargetAddress });

                return false;
            }
        }

        public async Task<bool> Execute(dynamic triggerParameters)
        {
            try
            {
                // TODO: Agregar verbo http como parámetro al trigger, porque ahora no hace envíos POSTs.

                var interpolatedTriggerAddress = StringFormatExtensions.FormatWith(TargetAddress, triggerParameters);

                var request = (HttpWebRequest)WebRequest.Create(interpolatedTriggerAddress);
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