using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digevo.Viral.Gateway.Models.Infrastructure.Services.SecureServices
{
    public class LoginParams
    {
        public LoginParams(int transactionID, string login, string password, string serviceURL)
        {
            this.TransactionID = transactionID;
            this.Login = login;
            this.Password = password;
            this.ServiceURL = serviceURL;
        }
        public int TransactionID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string ServiceURL { get; set; }
    }
}