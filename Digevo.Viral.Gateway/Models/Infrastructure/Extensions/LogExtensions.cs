using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;

namespace Digevo.Viral.Gateway.Models.Infrastructure.Extensions
{
    public static class LogExtensions
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static ILog Log { get { return log; } }

        public static void InfoCall(this ILog log, Func<object> args = null, [CallerMemberName] string name = "", [CallerFilePath] string path = "")
        {
            if (log.IsInfoEnabled)
                log.Info(new Message(name, path,(args != null) ? args() : null));
        }

        public static void DebugCall(this ILog log, Func<object> args = null, [CallerMemberName] string name = "", [CallerFilePath] string path = "")
        {
            if (log.IsDebugEnabled)
                log.Debug(new Message(name, path, (args != null) ? args() : null));
        }

        public static void ErrorCall(this ILog log, Exception ex, Func<object> args = null, [CallerMemberName] string name = "", [CallerFilePath] string path = "")
        {
            if (log.IsErrorEnabled)
                log.Error(new Message(name, path,(args != null) ? args() : null), ex);
        }

        private class Message
        {
            private readonly object _args;
            private readonly string _name;
            private readonly string _path;

            public Message(string name, string path, object args)
            {
                _name = name;
                _args = args;
                _path = path;
            }

            public override string ToString()
            {
                var sb = new StringBuilder("Call");
                sb.AppendFormat(" {0}(", _name);
                if (_args != null)
                {
                    bool addComma = false;
                    foreach (PropertyDescriptor a in TypeDescriptor.GetProperties(_args))
                    {
                        sb.AppendFormat(addComma ? ", {0}: '{1}'" : "{0}: '{1}'", a.Name, a.GetValue(_args));
                        addComma = true;
                    }
                }
                sb.AppendFormat(");{0}", _path);
                return sb.ToString();
            }
        }
    }
}