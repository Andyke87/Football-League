using League.Domein.DTO;
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
    /// Interaction logic for UpdateSpelerWindow.xaml
    /// </summary>
    public partial class UpdateSpelerWindow : Window
    {
        private SpelerManager spelerManager;
        public UpdateSpelerWindow()
        {
            InitializeComponent();
            spelerManager = new SpelerManager(new SpelerRepositoryADO(ConfigurationManager.ConnectionStrings["LeagueDBConnection"].ToString()));
        }
        private void ZoekSpelerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int? spelerid = null;
                string naam = null;
                if(!string.IsNullOrWhiteSpace(ZoekNaamTextBox.Text)) naam = ZoekNaamTextBox.Text;
                if (!string.IsNullOrWhiteSpace(ZoekSpelerIDTextBox.Text)) spelerid = int.Parse(ZoekSpelerIDTextBox.Text);
                List<SpelerInfo> spelers = (List<SpelerInfo>)spelerManager.SelecteerSpelers(spelerid,naam);
                if(spelers.Count == 0)
                {
                    NaamTextBox.Text = "";
                    SpelerIDTextBox.Text = "";
                    GewichtTextBox.Text = "";
                    LengteTextBox.Text = "";
                    RugnummerTextBox.Text = "";
                    TeamTextBox.Text = "";
                }
                if(spelers.Count == 1)
                {
                    NaamTextBox.Text = spelers[0].Naam;
                    SpelerIDTextBox.Text = spelers[0].Id.ToString();
                    GewichtTextBox.Text = spelers[0].Gewicht.ToString();
                    LengteTextBox.Text = spelers[0].Lengte.ToString();
                    RugnummerTextBox.Text = spelers[0].Rugnummer.ToString();
                    TeamTextBox.Text = spelers[0].Team.ToString();
                }
                if(spelers.Count > 1)
                {
                   SelecteerSpelerWindow selecteerSpeler = new SelecteerSpelerWindow(spelers);
                    if(selecteerSpeler.ShowDialog() == true)
                    {
                        NaamTextBox.Text = selecteerSpeler.GeselecteerdeSpeler.ToString();
                        SpelerIDTextBox.Text = selecteerSpeler.GeselecteerdeSpeler.ToString();
                        GewichtTextBox.Text = selecteerSpeler.GeselecteerdeSpeler.ToString();
                        LengteTextBox.Text = selecteerSpeler.GeselecteerdeSpeler.ToString();
                        RugnummerTextBox.Text = selecteerSpeler.GeselecteerdeSpeler.ToString();
                        TeamTextBox.Text = selecteerSpeler.GeselecteerdeSpeler.ToString();
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Button_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                int spelerid = int.Parse(SpelerIDTextBox.Text);
                int? gewicht = null;
                if(!string.IsNullOrWhiteSpace(GewichtTextBox.Text)) gewicht = int.Parse(GewichtTextBox.Text);
                int? lengte = null;
                if (!string.IsNullOrWhiteSpace(LengteTextBox.Text)) lengte = int.Parse(LengteTextBox.Text);
                int? rugnummer = null;
                if (!string.IsNullOrWhiteSpace(RugnummerTextBox.Text)) rugnummer = int.Parse(RugnummerTextBox.Text);
                SpelerInfo spelerInfo = new SpelerInfo(spelerid, NaamTextBox.Text,TeamTextBox.Text, rugnummer,lengte,gewicht);
                spelerManager.UpdateSpeler(spelerInfo);
                MessageBox.Show($"Speler : {spelerInfo}", "Speler is up to date");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } 
    }
}
