using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;

namespace staUtilities
{
    public class Main
    {
        public async void ConexionApi()
        {
            var config = ConfigurationBuilder();
            string apiUrl = config["ApiConnectionParameters:url"];
            string token = config["ApiConnectionParameters:Authorization"];
            string directory = config["SaveFileDirectory"];
            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Authorization", token);
            response = await client.GetAsync(apiUrl);            
            GuardaDatos(response,directory);
        }
        private async void GuardaDatos(HttpResponseMessage response, string directory)
        {
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Object[] resp = JsonConvert.DeserializeObject<Object[]>(json);
                Console.WriteLine(resp[resp.Length - 1].ToString());
                StreamWriter sw = new StreamWriter(directory,append:true);
                sw.WriteLine(resp[resp.Length - 1].ToString());
                sw.Close();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var getResponse = await response.Content.ReadAsStringAsync();
                StreamWriter sw = new StreamWriter(directory);
                sw.WriteLine(getResponse.ToString());
                sw.Close();
            }
        }

        private IConfigurationRoot ConfigurationBuilder()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appSettings.json", false, false);
            var config = builder.Build();
            return config;
        }

        public string About()
        {
            var config = ConfigurationBuilder();
            string version = config["Version"];
            return "Version actual: " + version;
        }
    }
}
