using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataContract;

namespace Logger
{
    public class Logger : API.ILogger 
    {
        public void Trace(string message)
        {
            throw new NotImplementedException();
        }
    }
}
