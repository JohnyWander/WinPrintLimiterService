namespace PrintLimiterApi.Users
{
    public class UserManager
    {
        internal List<RemoteUserContext> Clients = new List<RemoteUserContext>();

        public UserManager()
        {
            Task.Run(() =>
            {
                while (true)
                {

                
                // Set the target hour (e.g., 17 for 5:00 PM)
                int targetHour = 5;
                

                // Get current time
                DateTime now = DateTime.Now;

                // Create a DateTime for today at the target hour
                DateTime targetTime = new DateTime(now.Year, now.Month, now.Day, targetHour, 0, 0);

                // If the target time has already passed today, move to the same time tomorrow
                if (now > targetTime)
                {
                    targetTime = targetTime.AddDays(1);
                }

                // Calculate time left
                TimeSpan timeLeft = targetTime - now;

                // Convert to seconds
                double secondsLeft = timeLeft.TotalSeconds;

                Console.WriteLine($"Time left until limits reset at {targetHour}:00 is {secondsLeft} seconds.");

                Task.Delay(timeLeft).Wait();

                    Console.WriteLine("Resetting limits...");

                    Clients.ForEach(client =>
                    {
                        client.CurrentPagesCount.value = 0;
                    });

                }
            });
        }
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
