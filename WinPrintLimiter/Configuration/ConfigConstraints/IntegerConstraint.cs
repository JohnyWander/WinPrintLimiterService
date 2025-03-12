using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace WinPrintLimiter.Configuration.ConfigConstraints
{
    internal class IntegerConstraint : ConstraintBase
    {
        private string _FailMessage = "Integer constraint failed for value: @val it must be a valid number!";
        internal override bool ConstraintCheck(string data)
        {
            return int.TryParse(data, out _);
        }

        internal override string ConstraintFailMessage { get { return _FailMessage;  }   }

    }
}
