using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebFoodConsole
{
    class Program
    {
        private const string URL = "https://localhost:44335/api/food";

        public class Food
        {
            public int ID;
            public string Name;
            public int Calories;
            public string Ingridients;
            public int Grade;
        }

        static async Task<Uri> CreateFoodAsync(Food msg, HttpClient client)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/food", msg);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        static void Main(string[] args)
        {
            // POST REQUEST
            HttpClient client_post = new HttpClient();

            client_post.BaseAddress = new Uri(URL);
            client_post.DefaultRequestHeaders.Accept.Clear();
            client_post.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            Food food = new Food
            {
                Name = "Borw",
                Calories = 1500,
                Ingridients = "All thinks in the world",
                Grade = 10
            };

            var response_post = client_post.PostAsJsonAsync(
                 "", food).Result;

            Console.WriteLine(response_post);


            // GET REQUEST
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync("").Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var dataObjects = response.Content.ReadAsAsync<IEnumerable<Food>>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
                foreach (var food_db in dataObjects)
                {
                    Console.Write("{0} ", food_db.ID);
                    Console.Write("{0} ", food_db.Name);
                    Console.Write("{0} ", food_db.Calories);
                    Console.Write("{0} ", food_db.Grade);
                    Console.Write("{0} ", food_db.Ingridients);
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

        }
        
    }
}
