using Summer_Games_WebApi_Client.Data;
using Summer_Games_WebApi_Client.Models;
using Summer_Games_WebApi_Client.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Summer_Games_WebApi_Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly IAthleteRepository athleteRepository;
        private readonly ISportRepository sportRepository;
        private readonly IContingentRepository contingentRepository;

        public MainPage()
        {
            InitializeComponent();
            athleteRepository = new AthleteRepository();
            sportRepository = new SportRepository();
            contingentRepository = new ContingentRepository();
            FillDropdown();
        }

        private async void FillDropdown()
        {
            // Show Progress
            progRing.IsActive = true;
            progRing.Visibility = Visibility.Visible;

            try
            {
                List<Sport> sports = await sportRepository.GetSports();
                List<Contingent> contingents = await contingentRepository.GetContingents();
                //Add the All Option
                sports.Insert(0, new Sport { ID = 0, Name = " - All Sports" });
                contingents.Insert(0, new Contingent { ID = 0, Name = " - All Contingents" });
                //Bind to the ComboBox
                SportCombo.ItemsSource = sports;
                ContingentCombo.ItemsSource = contingents;
                btnAdd.IsEnabled = true;
                ShowAthletes(null, null);
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException().Message.Contains("connection with the server"))
                {
                    Jeeves.ShowMessage("Error", "No connection with the server.");
                }
                else
                {
                    Jeeves.ShowMessage("Error", "Could not complete operation");
                }
            }
            finally
            {
                progRing.IsActive = false;
                progRing.Visibility = Visibility.Collapsed;
            }
        }

        private async void ShowAthletes(int? SportID, int? ContingentID)
        {
            //Show Progress
            progRing.IsActive = true;
            progRing.Visibility = Visibility.Visible;

            try
            {
                List<Athlete> athletes;
                if (SportID.GetValueOrDefault() > 0)
                {
                    athletes = await athleteRepository.GetAthletesBySport(SportID.GetValueOrDefault());
                }
                else if (ContingentID.GetValueOrDefault() > 0)
                {
                    athletes = await athleteRepository.GetAthletesByContingent(ContingentID.GetValueOrDefault());
                }
                else
                {
                    athletes = await athleteRepository.GetAthletes();
                }
                athleteList.ItemsSource = athletes;

            }
            catch (ApiException apiEx)
            {
                string errMsg = "Errors:" + Environment.NewLine;
                foreach (var error in apiEx.Errors)
                {
                    errMsg += Environment.NewLine + "-" + error;
                }
                Jeeves.ShowMessage("Problem accessing Athletes:", errMsg);
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException().Message.Contains("connection with the server"))
                {
                    Jeeves.ShowMessage("Error", "No connection with the server.");
                }
                else
                {
                    Jeeves.ShowMessage("Error", "Could not complete operation");
                }
            }
            finally
            {
                progRing.IsActive = false;
                progRing.Visibility = Visibility.Collapsed;
            }
        }
        private void SportCombo_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Sport selSport = (Sport)SportCombo.SelectedItem;
            ShowAthletes(selSport?.ID, null);
        }
        private void ContingentCombo_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Contingent selContingent = (Contingent)ContingentCombo.SelectedItem;
            ShowAthletes(null, selContingent?.ID);
        }

        private void athleteGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the detail page
            Frame.Navigate(typeof(AthleteDetailPage), (Athlete)e.ClickedItem);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Athlete newAth = new Athlete();

            // Navigate to the detail page
            Frame.Navigate(typeof(AthleteDetailPage), newAth);
        }
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            SportCombo.SelectedIndex = 0;
            ContingentCombo.SelectedIndex = 0;
            //Sport selSport = (Sport)SportCombo.SelectedItem;
            //Contingent selContingent =(Contingent)ContingentCombo.SelectedItem;
            ShowAthletes(null, null);
        }
    }
}
