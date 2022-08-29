
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
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
        List<Game> games;
        List<Dev> devGames;
        List<Console> consoles;
        public int mode = 0;
        


        public MainWindow()

        {
            InitializeComponent();
            AddVersionNumber();
            LoadConsolesList();
            lbConsole.ItemsSource = consoles;
            tbSetPercentage.Text = "Completion %";
            atGamePage.Text = "Show site of the game in a browser";
            cbMode.Text = (string)cbiToPlay.Content;

            if (mode == 0)
            {
                LoadGamesList();
            }
            else if (mode == 1)
            {
                LoadDevGamesList();
            }
        }

        private void AddVersionNumber()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            this.Title += $" v.{versionInfo.FileVersion}";
        }

        private void LoadConsolesList()
        {
            consoles = SqliteDataAccess.LoadConsoles();
            WireUpConsolesList();
        }

        private void LoadGamesList()
        {
            games = SqliteDataAccess.LoadGames();
            WireUpGamesList();
        }

        private void LoadDevGamesList()
        {
            devGames = SqliteDataAccess.LoadDevGames();
            WireUpDevGamesList();
        }

        private void WireUpGamesList()
        {
            lbListOfGames.ItemsSource = null;
            lbListOfGames.ItemsSource = games;
        }

        private void WireUpDevGamesList()
        {
            lbListOfGames.ItemsSource = null;
            lbListOfGames.ItemsSource = devGames;
        }
        private void WireUpConsolesList()
        {
            lbConsole.ItemsSource = null;
            lbConsole.ItemsSource = consoles;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (mode == 0)
            {
                LoadGamesList();
            }
            else if (mode == 1)
            {
                LoadDevGamesList();
            }
        }

        private void btnSaveGame_Click(object sender, RoutedEventArgs e)
        {
            if (mode == 0)
            {
                Game game = new Game();
                game.Name = tbGame.Text;
                game.Console = ((Console)lbConsole.SelectedItem).Name;
                game.Link = tbLink.Text;

                if (game != null)
                {

                    SqliteDataAccess.SaveGame(game);

                    tbGame.Text = "";
                    tbLink.Text = "";
                    LoadGamesList();
                }

            }
            else if (mode == 1)
            {
                Dev dev = new Dev();
                dev.Name = tbGame.Text;
                dev.Console = ((Console)lbConsole.SelectedItem).Name;
                dev.Link = tbLink.Text;

                if (dev != null)
                {

                    SqliteDataAccess.SaveDevGame(dev);

                    tbGame.Text = "";
                    tbLink.Text = "";
                    LoadDevGamesList();
                }
            }
        }

        private void btnSetPercentage_Click(object sender, RoutedEventArgs e)
        {
            if (mode == 0)
            {
                Game game = (Game)lbListOfGames.SelectedItem;
                int percentage = 0;
                if (game != null)
                {
                    int.TryParse(tbSetPercentage.Text, out percentage);
                    game.Completion = percentage;

                    SqliteDataAccess.UpdateCompletion(game);
                    LoadGamesList();
                    if (percentage >= 100)
                    {
                        MessageBox.Show($"Congratulations for the mastery of {game.Name}! 🎉");
                    }
                }

                else
                {
                    MessageBox.Show("You need to select a game first to set the percentage.");
                }
            }
            else if (mode == 1)
            {
                Dev dev = (Dev)lbListOfGames.SelectedItem;
                int percentage = 0;
                if (dev != null)
                {
                    int.TryParse(tbSetPercentage.Text, out percentage);
                    dev.Completion = percentage;

                    SqliteDataAccess.UpdateDevCompletion(dev);
                    LoadDevGamesList();
                    if (percentage >= 100)
                    {
                        MessageBox.Show($"Congratulations for the completion of your new set for the game {dev.Name}! 🎉");
                    }
                }

                else
                {
                    MessageBox.Show("You need to select a game first to set the percentage.");
                }
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (mode == 0)
            {
                if (lbListOfGames.SelectedItem != null)
                {
                    Game game = (Game)lbListOfGames.SelectedItem;
                    if (MessageBox.Show($"Are you sure you want to remove {game.Name} from the list?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        if (game != null)
                        {
                            games.Remove(game);

                            WireUpGamesList();

                            SqliteDataAccess.RemoveGame(game);
                            LoadGamesList();
                        }
                    }
                }
            }

            else if (mode == 1)
            {
                if (lbListOfGames.SelectedItem != null)
                {
                    Dev dev = (Dev)lbListOfGames.SelectedItem;

                    if (MessageBox.Show($"Are you sure you want to remove {dev.Name} from the list?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        if (dev != null)
                        {
                            games.Remove(dev);

                            WireUpGamesList();

                            SqliteDataAccess.RemoveDevGame(dev);
                            LoadDevGamesList();
                        }
                    }
                }
            }
        }

        private void btnGoToGamePage_Click(object sender, RoutedEventArgs e)
        {
            if (mode == 0)
            {
                Game game = (Game)lbListOfGames.SelectedItem;


                if (game != null)
                {
                    string gamePage = game.Link;
                    System.Diagnostics.Process.Start($"{gamePage}");
                }
            }

            else if (mode == 1)
            {
                Dev dev = (Dev)lbListOfGames.SelectedItem;

                if (dev != null)
                {
                    string gamePage = dev.Link;

                    System.Diagnostics.Process.Start($"{gamePage}");
                }
            }
        }

        private void tbSetPercentage_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            tbSetPercentage.Clear();
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {

            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness();
            }
        }

        private void cbMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            mode = cbMode.SelectedIndex;
            if (mode == 0)
            {
                LoadGamesList();
            }
            if (mode == 1)
            {
                LoadDevGamesList();
            }

        }

        private void lbListOfGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (lbListOfGames.SelectedIndex != -1 && lbListOfGames.SelectedItem != null)
            {
                if (mode == 0)
                {
                    atGamePage.Text = "Show site of the game " + ((Game)lbListOfGames.SelectedItem).Name + " in a browser";

                }
                else if (mode == 1)
                {

                    atGamePage.Text = "Show site of the game " + ((Dev)lbListOfGames.SelectedItem).Name + " in a browser";
                }
            }
        }

        private void tbSetPercentage_GotFocus(object sender, RoutedEventArgs e)
        {
            tbSetPercentage.Text = "";
        }

        private void btnAddConsole_Click(object sender, RoutedEventArgs e)
        {
            Console console = new Console();



            console.Name = tbConsole.Text;

            if (console != null)
            {

                SqliteDataAccess.SaveConsole(console);

                tbConsole.Text = "";
                LoadConsolesList();
            }
        }

        private void btnRemoveConsole_Click(object sender, RoutedEventArgs e)
        {

            Console console = (Console)lbConsole.SelectedItem;
            if (lbConsole.SelectedItem != null)
            {
                if (MessageBox.Show($"Are you sure you want to remove {console.Name} from the list?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    if (console != null)
                    {
                        consoles.Remove(console);

                        WireUpConsolesList();

                        SqliteDataAccess.RemoveConsole(console);
                        LoadConsolesList();
                    }
                }
            }
        }

        //TODO Progressbar colors

        //TODO Save/Load your lists?

        //TODO Check for updates

    }
}
