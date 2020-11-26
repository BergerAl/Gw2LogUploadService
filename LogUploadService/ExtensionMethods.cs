using System;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExtensionMethods
{
    public static class Extensions
    {
        public static string Hyperlink(this IRestResponse response)
        {
            try
            {
                if (response is null)
                {
                    throw new ArgumentNullException(nameof(response));
                }
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                string content = JsonConvert.DeserializeObject(response.Content)?.ToString();
                if (content is null)
                {
                    Console.WriteLine("here we are");
                }
                JObject jsonObj = JObject.Parse(content);
                return jsonObj.GetValue("permalink").ToString();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8604 // Possible null reference argument.
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return "https://google.com";
            }
        }
    }
}
