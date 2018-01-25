using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using Newtonsoft.Json;
using Model;
using System.Reflection;

namespace WebApiConsole
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
           
           
            

            WebApiService service = new WebApiService();
            service.query("http://localhost:53864/api/values/");


        }
    }
}
