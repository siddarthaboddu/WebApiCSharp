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
    class WebApiService
    {
        HttpClient _client;
        public void query(string url)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(url);
            Console.WriteLine("Type exit to quit ");
            Console.WriteLine("query syntax : (get or post) (student or group) (studentId or groupId)[optional] \n type query : ");
            while (true)
            {
                String[] inputQuery = Console.ReadLine().Split(' ');

                if (inputQuery[0].Equals("exit"))
                {
                    Environment.Exit(0);
                }
                if (inputQuery[0].ToLower().Equals("get"))
                {
                    if (inputQuery[1].StartsWith("student"))
                    {
                        string id = "";
                        if (inputQuery.Length == 3)
                        {
                            id = inputQuery[2];
                        }
                        Type t = Type.GetType("Model.Student,Model");
                        IEnumerable< Student > result = Get< Student > (id).GetAwaiter().GetResult();
                        print(result);
                    }
                    if (inputQuery[1].StartsWith("group"))
                    {
                        string id = "";
                        if (inputQuery.Length == 3)
                        {
                            id = inputQuery[2];
                        }
                        IEnumerable<Group> result = Get<Group>(id).GetAwaiter().GetResult();
                        print(result);
                    }
                }
                if (inputQuery[0].ToLower().Equals("post"))
                {
                    string result="";
                    if (inputQuery[1].StartsWith("student"))
                    {
                        result =  Post<Student>(new Student { StudentId = int.Parse(inputQuery[2]), StudentName = inputQuery[3] });
                        
                    }
                    if (inputQuery[1].StartsWith("group"))
                    {
                        result = Post<Group>(new Group { GroupId = int.Parse(inputQuery[2]), GroupName = inputQuery[3] });
                        
                    }
                    Console.WriteLine("result : " + result);
                }
            }
        }

             public  async Task<IEnumerable<T>> Get<T>(string id = "")
        {
            string query = "";
            if (!id.Equals(""))
            {
                query = "get"+typeof(T).Name+"byid" + "/" + id;
            }
            else
            {
                query = "get" + typeof(T).Name + "s";
            }
            HttpResponseMessage responseTask = await _client.GetAsync(query);
            if (responseTask.IsSuccessStatusCode)
            {
                T[] readTask;
                if (id.Equals(""))
                    readTask = await responseTask.Content.ReadAsAsync<T[]>();
                else
                {
                    T obj = await responseTask.Content.ReadAsAsync<T>();
                    readTask = new T[1];
                    readTask[0] = obj;
                }
                return readTask;
            }
            return null;

        }
        public  string Post<T>(T value)
        {
            var postTask = _client.PostAsJsonAsync<T>("add"+typeof(T).Name, value);
            postTask.Wait();
            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                return "failed";
            }
        }
         void print<T>(IEnumerable<T> results)
        {
            PropertyInfo[] attributes = typeof(T).GetProperties();
            Console.WriteLine(typeof(T).Name+"s  found : ");
            foreach (T result in results)
            {
                foreach (var x in attributes)
                {
                    Console.Write(x.Name + ": " + x.GetValue(result) + "  ");
                }
                Console.WriteLine();
            }


        }
    }
}

