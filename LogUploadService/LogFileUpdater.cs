using System;
using RestSharp;
using System.IO;
using ExtensionMethods;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LogUploadService
{
    public class LogFileUpdater
    {
        private static IConfigurationRoot _config;

        public LogFileUpdater(IConfigurationRoot config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }
        public async Task<string> UploadFile(string filePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    throw new ArgumentException(nameof(filePath));
                }
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException(nameof(filePath));
                }

                var clientAdress = _config.GetSection("LogFilesUpdate:restClient").Value ?? throw new FieldAccessException("LogFilesUpdate:restClient");

                var client = new RestClient(clientAdress);

                var response = await client.ExecuteAsync(CreateRequest(filePath));

                if ((int)response.StatusCode > 299 || (int)response.StatusCode < 200)
                {
                    Console.Write($"Error from Server. Response Code: {response.StatusCode}, content {response.Content}");
                    return $"Error from Server. Response Code: {response.StatusCode}, content {response.Content}";
                }
                return response.Hyperlink();
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                return "https://google.com";
            }
        }

        private IRestRequest CreateRequest(string filePath)
        {
            var request = new RestRequest("/uploadContent", Method.POST)
                .AddParameter("json", 1)
                .AddParameter("generator", "ei")
                .AddHeader("Accept", "application/json")
                .AddHeader("Content-Type", "multipart/form-data");

            request.AddFile("file", filePath);

            return request;
        }
    }
}
