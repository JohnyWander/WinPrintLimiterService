using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace WinPrintLimiter.PrintControl
{
    [Serializable]
    internal class LocalUserContext : UserContextBase, ISerializable
    {
        internal SharedInt GlobalJobsCounter = new SharedInt(0);
        internal SharedInt MaxGlobal;




        BinaryFormatter formatter = new BinaryFormatter();
        public LocalUserContext() { }

        protected LocalUserContext(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public LocalUserContext(string userename, int maxPages) : base(userename)
        {
            base.ContextDate = DateTime.Now;
            MaxGlobal = new SharedInt(maxPages);
        }

        internal override string GetCurrentUsername()
        {
            return Environment.UserName;
        }




    }
}
