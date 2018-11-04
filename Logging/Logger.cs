using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataContract.API;

namespace Logging
{
        public class Logger : ILogger
        {
            public void Trace(string message)
            {
                System.Diagnostics.Trace.WriteLine(DateTime.Now + " | " + message);
            }
        }
}
