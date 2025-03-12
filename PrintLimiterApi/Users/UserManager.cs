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


        public void CreateUserContext()
        {
            Clients.Add(new RemoteUserContext());
        }


    }
}
