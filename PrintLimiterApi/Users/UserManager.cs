namespace PrintLimiterApi.Users
{
    public class UserManager
    {
        internal List<RemoteUserContext> Clients = new List<RemoteUserContext>();


        public bool CheckForContextExistance(string Username)
        {

            RemoteUserContext x = Clients.Find(x => x.Equals(Username));

            if (x != null && x.UserName == Username)
            {
                return true;
            }
            else
            {
                return false;
            }



        }

        public RemoteUserContext GetContext(string Username)
        {
            return Clients.Where(x => x.UserName == Username).FirstOrDefault();
        }



        public RemoteUserContext CreateUserContext(string username)
        {
            var context = new RemoteUserContext(username);
            Clients.Add(context);
            return context;
        }


    }
}
