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
        public static List<SwStarship> StarShips = new List<SwStarship>();
        public static int CurrentUserID;
        public static int CurrentStarshipID;

        static async Task Main(string[] args)
        {
            // NewParkingToTheDatabase(12, 238000);

            bool isRunning = true;

            do
            {
                Console.WriteLine("Welcome to SpacePark, please enter your full name!");

                string userInput = Console.ReadLine().ToUpper();


                if (CreatePerson(userInput))
                {
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

            await FetchStarships();

            isRunning = true;
            

            do
            {
                Console.Write("Please enter your starship id: ");
                if (int.TryParse(Console.ReadLine(), out CurrentStarshipID))
                {
                    if (CurrentStarshipID > 0 && CurrentStarshipID <= StarShips.Count)
                    {
                        isRunning = false;
                    }
                    else
                    {
                        Console.WriteLine("You entered an invalid starship id!");
                    }
                }
                else
                {
                    Console.WriteLine("Couldn't parse starship id!");
                }
            } while (isRunning);

            AddParkingSpotToTheDatabase();
            ReadParkingSpots();
            ReadFromUsers();
            Console.WriteLine("Thanks for selling your soul to SpacePark©");
        }

        private static async Task FetchStarships()
        {
            string originalPath = "starships/?page=";
            int page = 1;
            string next;
            do
            {
                var client = new RestClient("https://swapi.dev/api/");
                var request = new RestRequest(originalPath + page, DataFormat.Json);
                var response = await client.GetAsync<SwShip.Root>(request);

                next = response.next;
                page++;

                foreach (var result in response.results)
                {
                    string fixedLength = result.length.Replace(".", ",");
                    StarShips.Add(new SwStarship(result.model, double.Parse(fixedLength)));
                    Console.WriteLine($"{StarShips.Count - 1}: {result.model}");
                }
            } while (next != null);
        }

        public static bool CreatePerson(string fullName)
        {
            var context = new DBModel();
            int index = context.Users.ToList().FindIndex(x => x.Name == fullName);
            if (index == -1)
            {
                if (FindPerson(fullName).Result)
                {
                    AddNameToTheDatabase(fullName.ToUpper());
                    return true;
                }
                return false;
            }
            CurrentUserID = index;
            return true;
        }

        public static async Task<bool> FindPerson(string fullName)
        {
            try
            {
                var client = new RestClient("https://swapi.dev/api/");
                var request = new RestRequest($"people/?search={fullName}", DataFormat.Json);
                var peopleResponse = await client.GetAsync<SwPeople.Root>(request);

                foreach (var result in peopleResponse.results)
                {
                    if (result.name.ToLower() == fullName.ToLower())
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;
        }

        private static void AddNameToTheDatabase(string name)
        {
            var context = new DBModel();

            context.Users.Add(new User(name));
            var users = context.Users.Select(x => x).ToList();
            CurrentUserID = users.Count();
            context.SaveChanges();
        }

        private static void AddParkingSpotToTheDatabase()
        {
            var context = new DBModel();

            context.ParkingSpots.Add(new ParkingSpot(0, CurrentUserID, StarShips[CurrentStarshipID].Model, StarShips[CurrentStarshipID].LengthInM));

            context.SaveChanges();
        }

        private static void ReadParkingSpots()
        {
            var context = new DBModel();
            var parkingSpots = context.ParkingSpots.Select(x => x).ToList();
            var users = context.Users.Select(x => x).ToList();

            foreach (var parkingSpot in parkingSpots)
            {
                Console.WriteLine($"{Environment.NewLine}Parkingspot: {parkingSpot.ParkingSpotID}" +
                    $"{Environment.NewLine}Parking started: {parkingSpot.ParkingStarted}" +
                    $"{Environment.NewLine}Model: {parkingSpot.Vehicle}" +
                    $"{Environment.NewLine}Length: {parkingSpot.VehicleLength}m" +
                    $"{Environment.NewLine}User: {users[parkingSpot.UserID.GetValueOrDefault()].Name} {Environment.NewLine}");
            }
        }
                
        private static void ReadFromUsers()
        {
            var context = new DBModel();
            var users = context.Users.Select(x => x).ToList();

            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.UserID}, Name: {user.Name}");
            }
        }

        private static void NewParkingToTheDatabase(int hourlyRate, int length)
        {
            var parking = new Parking();
            parking.HourlyRatePerMeter = hourlyRate;
            parking.Length = length;

            var context = new DBModel();
            context.Parkings.Add(parking);
            context.SaveChanges();
        }
    }
}
