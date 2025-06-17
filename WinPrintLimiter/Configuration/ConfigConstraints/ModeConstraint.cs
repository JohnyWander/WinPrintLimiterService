namespace WinPrintLimiter.Configuration.ConfigConstraints
{
    internal class ModeConstraint : ConstraintBase
    {
        private string _FailMessage = "Invalid value - it should be \"local\" or \"online\"";
        internal override bool ConstraintCheck(string data)
        {
            if (data == null) return false;
            if (data == "") return false;

            if (data == "local" || data == "online")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal override string ConstraintFailMessage { get { return _FailMessage; } }

    }
}
