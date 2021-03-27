using System;
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
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

[assembly: InternalsVisibleTo("SpacePark")]
namespace SpacePark.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public string Example = "Hello";
        public static ObservableCollection<object> Users = new ObservableCollection<object>();
        public static ObservableCollection<object> StarShips = new ObservableCollection<object>();
        public ObservableCollection<object> users;
        public ObservableCollection<object> starShips;

        public MainPage()
        {
            this.InitializeComponent();
        }

        public void UpdateLists()
        {
            users = Users;
            starShips = StarShips;
        }
    }
}
