using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintLimiterApi.Configuration.Exceptions
{
    internal class ConfigurationException : Exception
    {

        public ConfigurationException() { }

        public ConfigurationException(string message) : base(message) { }

        public ConfigurationException(string message,Exception innerException) : base(message, innerException) { }
    }
}
