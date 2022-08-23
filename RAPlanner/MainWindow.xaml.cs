using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RAPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Game> games = new List<Game>();
        List<string> consoles = new List<string>();

        //GlobalConfig.Connection.GetTournament_All();
        public MainWindow()
        {
            InitializeComponent();
            consoles = LoadConsolesList();
            LoadGamesList();
            lbConsole.ItemsSource = consoles;
        }

        private List<string> LoadConsolesList()
        {
            List<string> list = new List<string>();
            list.Add("GBC");
            list.Add("GBA");
            list.Add("NES");
            list.Add("SNES");
            list.Add("N64");
            list.Add("NDS");
            list.Add("Playstation");
            list.Add("PSP");
            list.Add("PC Engine");
            list.Add("PC-8000/8800");
            list.Add("PC-FX");
            list.Add("Master System");
            list.Add("Game Gear");
            list.Add("Genesis");
            list.Add("Sega CD");
            list.Add("Sega Saturn");
            list.Add("Sega Dreamcast");
            list.Add("3DO");
            list.Add("Arduboy");
            list.Add("MSX");
            list.Add("Neo Geo Pocket");
            list.Add("WASM-4");
            list.Add("WonderSwan");
            list.Add("Other");
            return list;
        }

        private void LoadGamesList()
        {
            games = SqliteDataAccess.LoadGames();
            //games.Add(new Game() { Name = "Sakura Wars | Sakura Taisen", Console = "Saturn", Completion = 10 });
            //games.Add(new Game() { Name = "Popful Mail", Console = "PC Engine", Completion = 10 });
            WireUpGamesList();
        }

        private void WireUpGamesList()
        {
            lbListOfGames.ItemsSource = null;
            lbListOfGames.ItemsSource = games;
            //lbListOfGames.DisplayMemberPath = "FullGame";
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadGamesList();
        }

        private void btnSaveGame_Click(object sender, RoutedEventArgs e)
        {
            Game game = new Game();
            game.Name = tbGame.Text;
            game.Console = lbConsole.SelectedValue.ToString();
            game.Link = tbLink.Text;

            //games.Add(game);
            //WireUpGamesList();
            SqliteDataAccess.SaveGame(game);

            tbGame.Text = "";
            tbLink.Text = "";
        }

        //private void btnMarkAsCompleted_Click(object sender, RoutedEventArgs e)
        //{

        //}

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            Game game = (Game)lbListOfGames.SelectedItem;

            if(game != null)
            {
                games.Remove(game);

                WireUpGamesList();

                SqliteDataAccess.RemoveGame(game);
            }
        }

        private void btnGoToGamePage_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
