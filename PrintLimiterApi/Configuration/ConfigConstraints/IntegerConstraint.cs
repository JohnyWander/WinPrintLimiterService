using PrintLimiterApi.Configuration.ConfigConstraints;

namespace WinPrintLimiter.Configuration.ConfigConstraints
{
    internal class IntegerConstraint : ConstraintBase
    {
        private string _FailMessage = "Integer constraint failed for value: @val it must be a valid number!";
        internal override bool ConstraintCheck(string data)
        {
            return int.TryParse(data, out _);
        }

        internal override string ConstraintFailMessage { get { return _FailMessage; } }

    }
}
