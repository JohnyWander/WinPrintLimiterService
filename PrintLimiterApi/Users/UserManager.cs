using static PrintLimiterApi.Program;

namespace PrintLimiterApi.Users
{
    public class UserManager
    {
        List<RemoteUserContext> Clients = new List<RemoteUserContext>();


        public bool CheckForContextExistance(string Username)
        {
            if(Clients.Where(x=>x.UserName == Username).ToList().Count() == 1)
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



        public void CreateUserContext(string username)
        {
            Clients.Add(new RemoteUserContext(username));
        }


    }
}
