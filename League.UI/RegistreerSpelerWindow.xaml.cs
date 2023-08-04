using League.Domein.Managers;
using League.Domein.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Shapes;



namespace League.UI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class RegistreerSpelerWindow : Window
    {
        private SpelerManager spelerManager;
        public RegistreerSpelerWindow()
        {
            InitializeComponent();
            spelerManager = new SpelerManager( new SpelerRepositoryADO(ConfigurationManager.ConnectionStrings["LeagueDBConnection"].ToString()));
        }
        private void RegistreerSpelerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NaamTextBox.Text))
                {
                    MessageBox.Show("Naam is verplicht");
                    
                }
                else
                {
                    string naam = NaamTextBox.Text;
                    int? rugnummer = null;
                    if (!string.IsNullOrWhiteSpace(RugnummerTextBox.Text))
                    {
                        rugnummer = int.Parse(RugnummerTextBox.Text);
                    }
                    int? lengte = null;
                    if (!string.IsNullOrWhiteSpace(LengteTextBox.Text))
                    {
                        lengte = int.Parse(LengteTextBox.Text);
                    }
                    int? gewicht = null;
                    if (!string.IsNullOrWhiteSpace(GewichtTextBox.Text))
                    {
                        gewicht = int.Parse(GewichtTextBox.Text);
                    }
                    spelerManager.RegistreerSpeler(naam, lengte, gewicht);
                    MessageBox.Show($"Speler : {naam}","Speler is geregistreerd");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
        }
    }
}
