using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinPrintLimiter.Configuration.Exceptions;
namespace WinPrintLimiter.Configuration.ConfigConstraints
{
    internal abstract class ConstraintBase
    {
        internal abstract bool ConstraintCheck(string data);
        internal abstract string ConstraintFailMessage { get;  }

        
        internal void ThrowError(string value)
        {
            throw new ConfigurationException(ConstraintFailMessage.Replace("@val",value));
        }


    }
}
