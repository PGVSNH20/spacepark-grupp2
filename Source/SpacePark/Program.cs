using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using RestSharp;
using SpacePark.App.Classes;
using SpacePark.Classes;
[assembly: InternalsVisibleTo("SpaceParks.Test")]

namespace SpacePark
{

    public class Program
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

            //Uncomment to add a parking house
            //NewParkingToTheDatabase(12, 238000);
            
            bool isRunning = true;

            do
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine();
                Console.WriteLine("Welcome to SpacePark, please enter your full name or your parking ID to check out!\n");

                string userInput = Console.ReadLine().ToUpper();

                //If int, checkout from parking
                if (int.TryParse(userInput, out int checkoutID))
                {
                    CheckoutParking(checkoutID);
                }
                //Discrimina...check that user/person is credible
                else if (CreatePerson(userInput))
                {
                    isRunning = false;
                }
                //Dev tool, if no input, show existing data
                else if (userInput == string.Empty)
                {
                    ReadParkingSpots();
                    ReadFromUsers();
                }
                //User is not credible
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Did you spell your name wrong? Try again.");
                    Console.ReadKey();
                    Console.Clear();
                }
            } while (isRunning);

            StarShips = await FetchStarships();

            isRunning = true;

            do
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("\nPlease enter your starship id: ");
                if (int.TryParse(Console.ReadLine(), out CurrentStarshipID))
                {
                    //Check if valid starship was chosen
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

        /// <summary>
        /// Checkout from parking, writes invoice and removes parkingspot.
        /// </summary>
        /// <param name="checkoutID">ID of the parkingspot where the vehicle is parked</param>
        private static void CheckoutParking(int checkoutID)
        {
            //Connect to database
            var context = new DBModel();
            //Get all parkingspots
            var parkingSpots = context.ParkingSpots.Where(x => x.ParkingSpotID == checkoutID).ToList();
            if (parkingSpots.Count == 0)
            {
                Console.WriteLine("No ship found there, who are you paying for really?");
                return;
            }
            //Get the correct parkingspot (could be done better, yes)
            var parkingSpot = parkingSpots[0];
            //Check for how long the vehicle was parked, rounded up to closest hour
            var hoursParked = CheckHoursParked(parkingSpot.ParkingStarted, DateTime.Now);
            //Find the parkingspot
            var parking = context.Parkings.Where(x => x.ParkingID == parkingSpot.ParkingID).ToList()[0];
            //Calculate cost
            var cost = parking.HourlyRatePerMeter * hoursParked * parkingSpot.VehicleLength;
            //Get related user for invoice
            var user = context.Users.Where(x => x.UserID == parkingSpot.UserID).ToList()[0];

            //Randomize money, since we aren't evil enough to know their economy
            Random rnd = new Random();
            var money = rnd.Next(3000000);
            Console.WriteLine($"You have {money}gc, I wonder if it will be enough?");
            if (money < cost)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Oh no, you're poor. Don't worry, we'll just sell your ship, no biggy.");
                return;
            }

            //Remove parkingspot from database and give user an invoice. No rollbacking in case of error now.
            context.ParkingSpots.Remove(parkingSpot);
            context.SaveChanges();
            CreateInvoice(user.Name, hoursParked, cost);
        }

        /// <summary>
        /// Print invoice for user.
        /// </summary>
        /// <param name="userName">Name of the user</param>
        /// <param name="hoursParked">How long the user parked</param>
        /// <param name="cost">How much it cost to park in galactic credits</param>
        private static void CreateInvoice(string userName, double hoursParked, double cost)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Name: {0}, Parked for: {1}h, Cost: {2}gc", userName, hoursParked, cost);
        }
        /// <summary>
        /// Fetch starships from SWAPI and returns list.
        /// </summary>
        internal static async Task<List<SwStarship>> FetchStarships()
        {
            List<SwStarship> starships = new List<SwStarship>();
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
                    starships.Add(new SwStarship(result.model, double.Parse(fixedLength)));
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"{StarShips.Count - 1}: {result.model}");
                }
            } while (next != null);
            return starships;
        }

        /// <summary>
        /// Adds new user if valid.
        /// </summary>
        /// <param name="fullName">Name of new user</param>
        /// <returns>True if user is valid, false if not</returns>
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

        /// <summary>
        /// Check with SWAPI if person exists in the Star Wars-universe
        /// </summary>
        /// <param name="fullName">Name of checked user</param>
        /// <returns>True/false if they exist or not</returns>
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

        /// <summary>
        /// Add new user to database and update active user.
        /// </summary>
        /// <param name="name">Name of new user</param>
        private static void AddNameToTheDatabase(string name)
        {
            var context = new DBModel();

            context.Users.Add(new User(name));
            var users = context.Users.Select(x => x).ToList();
            context.SaveChanges();
            CurrentUserID = users.Count();
        }

        /// <summary>
        /// Adds new parking spot to the database
        /// </summary>
        private static void AddParkingSpotToTheDatabase()
        {
            //Check that there's enough space
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

        /// <summary>
        /// Print list of existing parkingspots.
        /// </summary>
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
        
        /// <summary>
        /// Print list of existing users.
        /// </summary>
        private static void ReadFromUsers()
        {
            var context = new DBModel();
            var users = context.Users.Select(x => x).ToList();

            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.UserID}, Name: {user.Name}");
            }
        }

        /// <summary>
        /// Add new parking-place/-house/-lot
        /// </summary>
        /// <param name="hourlyRate">Cost per hour in galactic credits</param>
        /// <param name="length">How long the parkinglot is</param>
        private static void NewParkingToTheDatabase(int hourlyRate, double length)
        {
            var parking = new Parking();
            parking.HourlyRatePerMeter = hourlyRate;
            parking.Length = length;

            var context = new DBModel();
            context.Parkings.Add(parking);
            context.SaveChanges();
        }

        /// <summary>
        /// Check that something fits into the available space.
        /// </summary>
        /// <param name="length">Length of the incoming vehicle</param>
        /// <returns>True if fits, false if not</returns>
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

        /// <summary>
        /// Check how long a vehicle has been parked, round up to closest hour.
        /// </summary>
        /// <param name="startDateTime">Start date and time of parking</param>
        /// <param name="endDateTime">End date and time of parking</param>
        /// <returns>Total hours parked rounded up</returns>
        private static double CheckHoursParked(DateTime startDateTime, DateTime endDateTime)
        {
            TimeSpan ts = endDateTime.Subtract(startDateTime);
            return Math.Ceiling(ts.TotalHours);
        }
    }
}
