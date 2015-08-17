using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digevo.Viral.Gateway.Models.Entities.Landing
{
    public class UserData
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        //public string Arcane { get; set; }

        //public string ArcaneQuery { get; set; }

        public DateTimeOffset? Birthday { get; set; }

        public DateTimeOffset CreationDate { get; set; }
    }
}