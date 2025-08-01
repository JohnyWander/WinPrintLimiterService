﻿namespace WinPrintLimiter.Configuration.ConfigConstraints
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
