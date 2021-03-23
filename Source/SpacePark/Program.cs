using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RestSharp;
using SpacePark.Classes;

namespace SpacePark
{
    class Program
    {
        public static List<User> Users;
        public static ObservableCollection<SwStarship> StarShips = new ObservableCollection<SwStarship>();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello SpacePark!");

            FetchStarships();


            Users = new List<User>
            {
                new User("Nisse Jonsson"),
                new User("Luke Skywalker")
            };

            CreatePerson("Leia Organa");
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
                var peopleResponse = await client.GetAsync<SwShip.Root>(request);

                next = peopleResponse.next;
                page++;

                foreach (var result in peopleResponse.results)
                {
                    StarShips.Add(new SwStarship(result.model, double.Parse(result.length)));
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

        public static void CreatePerson(string fullName)
        {
            if (FindPerson(fullName).Result)
            {
                Users.Add(new User(fullName));
            }
        }
    }
}
