using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using SpacePark.App.Classes;
using SpacePark.Classes;

namespace SpacePark
{
    class Program
    {
        public static List<User> Users;
        public static ObservableCollection<SwStarship> StarShips = new ObservableCollection<SwStarship>();

        static void Main(string[] args)
        {
            bool isRunning = true;

            do
            {
                Console.WriteLine("Welcome to SpacePark, please enter your full name!");

                string userInput = Console.ReadLine();


                if (CreatePerson(userInput))
                {
                    FetchStarships();
                    isRunning = false;
                }
                else if (userInput == string.Empty)
                {
                    ReadFromUsers();
                }
                else
                {
                    Console.WriteLine("Did you spell your name wrong? Try again.");
                    Console.ReadKey();
                    Console.Clear();
                }
            } while (isRunning);

            
        }

        private static void AddNameToTheDatabase(string name)
        {
            var context = new DBModel();

            context.Users.Add(new User(name));

            context.SaveChanges();
        }

        private static void ReadFromUsers()
        {
            var context = new DBModel();
            var users = context.Users.Where(x => x.Name == "Miss Piggy").ToList();

            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.UserID}, Name: {user.Name}");

            }
        }

        private static async void FetchStarships()
        {
            string originalPath = "starships/?page=";
            int page = 1;
            string next;
            do
            {
                var client = new RestClient("https://swapi.dev/api/");
                var request = new RestRequest(originalPath + page, DataFormat.Json);
                var respone = await client.GetAsync<SwShip.Root>(request);

                next = respone.next;
                page++;

                foreach (var result in respone.results)
                {
                    StarShips.Add(new SwStarship(result.model, double.Parse(result.length)));
                    Console.WriteLine($"{result.model}, {StarShips.Count -1}");
                }

            } while (next != null);
        }

        public static async Task<bool> FindPerson(string fullName)
        {

            try
            {
                var client = new RestClient("https://swapi.dev/api/");
                var request = new RestRequest($"people/?search={fullName}", DataFormat.Json);
                var peopleResponse = client.Get<SwPeople.Root>(request).Data;

                if (peopleResponse.count > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;
        }

        public static bool CreatePerson(string fullName)
        {
            if (FindPerson(fullName).Result)
            {
                AddNameToTheDatabase(fullName);
                return true;
            }
            return false;
        }
    }
}
