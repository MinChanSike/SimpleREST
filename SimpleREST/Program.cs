using System;
using System.Configuration;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using Owin;

namespace SimpleREST {
    class Program {
        static void Main(string[] args) {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var apiHostUrl = config.AppSettings.Settings["apiHostUrl"].Value.ToString();

            using (WebApp.Start<Startup>(url: apiHostUrl)) {
                Console.WriteLine($"API is running on {apiHostUrl}\n");

                Console.WriteLine("======================================================");
                Console.WriteLine("\tCall me by curl with below command.");
                Console.WriteLine($"\tcurl {apiHostUrl}/api/test");
                Console.WriteLine("======================================================\n");
                Console.ReadLine();
            }
        }
    }

    public class Startup {
        public void Configuration(IAppBuilder appBuilder) {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            appBuilder.UseWebApi(config);
        }
    }

    public class TestController : ApiController {

        [HttpGet]
        public IHttpActionResult Test() {
            var msg = $"Received test request at {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff TT")}";
            Console.WriteLine(msg);
            return Ok($"SERVER: {msg}");
        }
    }
}
