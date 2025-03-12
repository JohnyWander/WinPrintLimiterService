using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PrintLimiterApi.Configuration.ConfigConstraints
{
    internal class BooleanConstraint : ConstraintBase
    {

        private string _FailMessage = "Boolean Constraint Failed for value: @val ";
        internal override bool ConstraintCheck(string data)
        {
            Boolean.TryParse(data, out bool result);
            return result;
        }

     

        internal override string ConstraintFailMessage { get { return _FailMessage; } }


    }
}
