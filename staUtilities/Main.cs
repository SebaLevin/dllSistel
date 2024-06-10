using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace staUtilities
{
    public class Main
    {
        //Metodos invocables
        public string ObtieneDataLiveConnect()
        {
            //Invoca funcion de conexion con API y guarda en una variable el resultado
            Task<string> apiResult = ConexionApi();
            var responseMessage = apiResult.Result;

            //Parsea respuesta a un tipo JObject, y guarda en variable un valor especifico del json
            JToken jobject = JObject.Parse(responseMessage);            
            var stringParse = Convert.ToString(jobject["data"][0]["reglas"]["profession"].ToString());
            return stringParse;
        }
        public float ObtieneCotizacionDolar()
        {
            //Invoca funcion de conexion con API y guarda en una variable el resultado
            Task<string> respuesta = ConexionApi();
            var jobject = respuesta.Result;
            //Deserializa la respuesta en un tipo dinamico, y obtiene la cotizacion del dia anterior
            dynamic resp = JsonConvert.DeserializeObject<Object[]>(jobject);
            float stringParse = resp[resp.Length - 1].v;
            return stringParse;
        }
        public string About()
        {
            var config = ConfigurationBuilder();
            string version = config["Version"];
            return "Version actual: " + version;
        }

        public void GuardaDatos()
        {
            // Funcion que guarda en una carpeta asignada un txt con la cotizacion del dolar
            var config = ConfigurationBuilder();
            string saveDirectory = config["SaveFileDirectory"];
            StreamWriter writer = new StreamWriter(saveDirectory, append:true);
            writer.WriteLine(ObtieneCotizacionDolar().ToString());
            writer.Close();
            Console.WriteLine("Correcto");
        }

        //Metodos privados de utilidad
        private async Task<string> ConexionApi()
        {            
            // Lee el archivo de config, y se asigna a variables los parametros especificados
            var config = ConfigurationBuilder();
            string apiUrl = config["ApiConnectionParameters:url"];
            string authenticationType = config["ApiConnectionParameters:AuthenticationType"];
            string apiToken = config["ApiConnectionParameters:Token"];
            string saveDirectory = config["SaveFileDirectory"];
            HttpResponseMessage httpResponse;
            // Ejecuta una tarea que realiza la conexion con la api, y devuelve el mensaje de respuesta
            return await Task.Run(async () =>
            {                         
                using (HttpClient client = new HttpClient())
                {
                    // Se especifican la url y los parametros a enviar en el header
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add(authenticationType, apiToken);
                    httpResponse = await client.GetAsync(apiUrl);
                }
                // Si es correcta, devuelve el mensaje de la respuesta. Caso contrario, informa el error por consola
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseMessage = await httpResponse.Content.ReadAsStringAsync();
                    return responseMessage;
                }
                else if (httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine(httpResponse.Content.ReadAsStringAsync());
                }
                return null;
            });           
        }

        private IConfigurationRoot ConfigurationBuilder()
        {
            // Funcion que lee el archivo appSettings.json, y lo devuelve
            var builder = new ConfigurationBuilder().AddJsonFile("appSettings.json", false, false);
            var config = builder.Build();
            return config;
        }
    }
}
