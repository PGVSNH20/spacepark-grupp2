﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Protection.PlayReady;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using RestSharp;
using SpacePark.App.Classes;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SpacePark.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public string Example = "Hello";
        public List<User> Users;
        public ObservableCollection<SwStarship> StarShips = new ObservableCollection<SwStarship>();

        public MainPage()
        {
            this.InitializeComponent();
            
            FetchStarships();

            
            Users = new List<User>
            {
                new User("Nisse Jonsson"),
                new User("Luke Skywalker")
            };

            CreatePerson("R2D2");

        }

        private async void FetchStarships()
        {
            var client = new RestClient("https://swapi.dev/api/");
            var request = new RestRequest("starships/", DataFormat.Json);
            var peopleResponse = await client.GetAsync<SwShip.Root>(request);

            foreach (var result in peopleResponse.results)
            {
                StarShips.Add(new SwStarship(result.model, double.Parse(result.length)));
            }
        }

        public async Task<bool> FindPerson(string fullName)
        {
            var client = new RestClient("https://swapi.dev/api/");
            var request = new RestRequest($"people/?search={fullName}", DataFormat.Json);
            var peopleResponse = await client.GetAsync<SwPeople.Root>(request);

            if(peopleResponse.count > 0)
            {
                return true;
            }          
            
            return false;           
        }

        public void CreatePerson(string fullName)
        {
            if(FindPerson(fullName).Result)
            {
                Users.Add(new User(fullName));
            }
        }
    }
}
