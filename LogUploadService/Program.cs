using Microsoft.Extensions.Configuration;
using System.IO;


namespace LogUploadService
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = CreateConfiguration();
            new FileWatcher(config);
        }

        static IConfigurationRoot CreateConfiguration()
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.user.json"))) {
                config.AddJsonFile("appsettings.user.json");
            }
            return config.Build();
        }
    }
}
