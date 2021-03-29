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
        public static List<SwStarship> StarShips = new List<SwStarship>();
        public static int CurrentUserID;
        public static int CurrentStarshipID;
        public static int CurrentParkingID = 1;
        
        static async Task Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();

            //NewParkingToTheDatabase(12, 238000);

            bool isRunning = true;

            do
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine();
                Console.WriteLine("Welcome to SpacePark, please enter your full name or your parking ID to check out!\n");

                string userInput = Console.ReadLine().ToUpper();

                if (int.TryParse(userInput, out int checkoutID))
                {
                    CheckoutParking(checkoutID);
                }
                else if (CreatePerson(userInput))
                {
                    isRunning = false;
                }
                else if (userInput == string.Empty)
                {
                    ReadParkingSpots();
                    ReadFromUsers();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Did you spell your name wrong? Try again.");
                    Console.ReadKey();
                    Console.Clear();
                }
            } while (isRunning);

            await FetchStarships();

            isRunning = true;

            do
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("\nPlease enter your starship id: ");
                if (int.TryParse(Console.ReadLine(), out CurrentStarshipID))
                {
                    if (CurrentStarshipID >= 0 && CurrentStarshipID <= StarShips.Count)
                    {
                        isRunning = false;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You entered an invalid starship id!");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Couldn't parse starship id!");
                }
            } while (isRunning);

            AddParkingSpotToTheDatabase();
            ReadParkingSpots();
            ReadFromUsers();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine();
            Console.WriteLine("Thanks for selling your soul to SpacePark©");
        }

        private static void CheckoutParking(int checkoutID)
        {
            var context = new DBModel();
            var parkingSpots = context.ParkingSpots.Where(x => x.ParkingSpotID == checkoutID).ToList();
            if (parkingSpots.Count == 0)
            {
                Console.WriteLine("No ship found there, who are you paying for really?");
                return;
            }
            var parkingSpot = parkingSpots[0];
            var hoursParked = CheckHoursParked(parkingSpot.ParkingStarted, DateTime.Now);
            var parkings = context.Parkings.Where(x => x.ParkingID == parkingSpot.ParkingID).ToList();
            var parking = parkings[0];
            var cost = parking.HourlyRatePerMeter * hoursParked * parkingSpot.VehicleLength;
            var user = context.Users.Where(x => x.UserID == parkingSpot.UserID).ToList()[0];

            Random rnd = new Random();
            var money = rnd.Next(3000000);
            Console.WriteLine($"You have {money}gc, I wonder if it will be enough?");
            if (money < cost)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Oh no, you're poor. Don't worry, we'll just sell your ship, no biggy.");
                return;
            }

            context.ParkingSpots.Remove(parkingSpot);
            context.SaveChanges();
            CreateInvoice(user.Name, hoursParked, cost);
        }

        private static void CreateInvoice(string userName, double hoursParked, double cost)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Name: {0}, Parked for: {1}h, Cost: {2}gc", userName, hoursParked, cost);
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
                    Console.ForegroundColor = ConsoleColor.Magenta;
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
            context.SaveChanges();
            CurrentUserID = users.Count();
        }

        private static void AddParkingSpotToTheDatabase()
        {
            if (CheckAvailability(StarShips[CurrentStarshipID].LengthInM))
            {
                var context = new DBModel();
                context.ParkingSpots.Add(new ParkingSpot(CurrentParkingID, CurrentUserID + 1, StarShips[CurrentStarshipID].Model, StarShips[CurrentStarshipID].LengthInM));
                context.SaveChanges();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("fuck off cuz you're too big");
            } 
        }

        private static void ReadParkingSpots()
        {
            var context = new DBModel();
            var parkingSpots = context.ParkingSpots.Select(x => x).ToList();
            var users = context.Users.Select(x => x).ToList();

            foreach (var parkingSpot in parkingSpots)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"{Environment.NewLine}Parkingspot: {parkingSpot.ParkingSpotID}" +
                    $"{Environment.NewLine}Parking started: {parkingSpot.ParkingStarted}" +
                    $"{Environment.NewLine}Model: {parkingSpot.Vehicle}" +
                    $"{Environment.NewLine}Length: {parkingSpot.VehicleLength}m" +
                    $"{Environment.NewLine}User: {users[parkingSpot.UserID.GetValueOrDefault() - 1].Name} {Environment.NewLine}");
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

        private static void NewParkingToTheDatabase(int hourlyRate, double length)
        {
            var parking = new Parking();
            parking.HourlyRatePerMeter = hourlyRate;
            parking.Length = length;

            var context = new DBModel();
            context.Parkings.Add(parking);
            context.SaveChanges();
        }

        private static bool CheckAvailability(double length)
        {
            var context = new DBModel();
            if (context.Parkings.Count() > 0)
            {
                double maxLength = context.Parkings.ToList()[CurrentParkingID - 1].Length;
                var occupiedLength = context.ParkingSpots.Sum(x => x.VehicleLength);

                if (length + occupiedLength > maxLength)
                {
                    return false;
                }
                return true;
            }

            Console.WriteLine("There isn't even a parkingplace here, leave.");
            return false;
        }

        private static double CheckHoursParked(DateTime startDateTime, DateTime endDateTime)
        {
            TimeSpan ts = endDateTime.Subtract(startDateTime);
            return Math.Ceiling(ts.TotalHours);
        }
    }
}
