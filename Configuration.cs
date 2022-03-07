namespace Livraria
{
    public static class Configuration
    {
        public static string ConnectionString;

        public static string JwtKey;
        public static SmtpConfiguration Smtp { get; set; } = new();

        public class SmtpConfiguration 
        {
            public string Host { get; set; }
            public int Port { get; set; } = 25;
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
