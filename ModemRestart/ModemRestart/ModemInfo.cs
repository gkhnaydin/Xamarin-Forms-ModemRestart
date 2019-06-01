namespace ModemRestart
{
    public class ModemInfo
    {
        public string IpAdress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public ModemInfo(string ip, string user, string pass)
        {
            this.IpAdress = ip;
            this.UserName = user;
            this.Password = pass;
        }

        public bool Check()
        {
            return false;
        }
    }
}
