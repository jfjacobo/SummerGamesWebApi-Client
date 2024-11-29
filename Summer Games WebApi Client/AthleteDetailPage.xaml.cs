using Summer_Games_WebApi_Client.Data;
using Summer_Games_WebApi_Client.Models;
using Summer_Games_WebApi_Client.Utilities;
using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Summer_Games_WebApi_Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class AthleteDetailPage : Page
    {
        Athlete view;
        IAthleteRepository athleteRepository;
        ISportRepository sportRepository;
        IContingentRepository contingentRepository;
        bool InsertMode;

        public AthleteDetailPage()
        {
            this.InitializeComponent();
            athleteRepository = new AthleteRepository();
            sportRepository = new SportRepository();
            contingentRepository = new ContingentRepository();
            FillDropDown();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            view = (Athlete)e.Parameter;

            if (view.ID == 0) //Inserting
            {
                //Disable the delete button if adding
                btnDelete.IsEnabled = false;
                InsertMode = true;
            }
            else
            {
                InsertMode = false;
            }
        }
        private async void FillDropDown()
        {
            try
            {
                List<Sport> sports = await sportRepository.GetSports();
                List<Contingent> contingents = await contingentRepository.GetContingents();
                //Bind to the ComboBox
                SportCombo.ItemsSource = sports.OrderBy(d => d.Name);
                ContingentCombo.ItemsSource = contingents.OrderBy(d => d.Name);
                //Now you can assign the DataContext for the page
                this.DataContext = view;
            }
            catch (ApiException apiEx)
            {
                string errMsg = "Errors:" + Environment.NewLine;
                foreach (var error in apiEx.Errors)
                {
                    errMsg += Environment.NewLine + "-" + error;
                }
                Jeeves.ShowMessage("Problem filling Dropdowns Selection:", errMsg);
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
        }
        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (view.ContingentID == 0)
                {
                    Jeeves.ShowMessage("Error", "You must select the Contingent the Athlete comes from");
                }
                else if(view.SportID == 0)
                {
                    Jeeves.ShowMessage("Error", "You must select a Sport for the Athlete");
                }
                else
                {
                    if (InsertMode)
                    {
                        await athleteRepository.AddAthlete(view);
                    }
                    else
                    {
                        await athleteRepository.UpdateAthlete(view);
                    }
                    Frame.GoBack();
                }
            }
            catch (AggregateException aex)
            {
                string errMsg = "Errors:" + Environment.NewLine;
                foreach (var exception in aex.InnerExceptions)
                {
                    errMsg += Environment.NewLine + exception.Message;
                }
                Jeeves.ShowMessage("One or more exceptions has occurred:", errMsg);
            }
            catch (ApiException apiEx)
            {
                string errMsg = "Errors:" + Environment.NewLine;
                foreach (var error in apiEx.Errors)
                {
                    errMsg += Environment.NewLine + "-" + error;
                }
                Jeeves.ShowMessage("Problem Saving the Record:", errMsg);
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException().Message.Contains("connection with the server"))
                {
                    Jeeves.ShowMessage("Error", "No connection with the server.");
                }
                else
                {
                    Jeeves.ShowMessage("Error", "Could not complete operation.");
                }
            }
        }
        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string strTitle = "Confirm Delete";
            string strMsg = "Are you certain that you want to delete " + view.Summary + "?";
            ContentDialogResult result = await Jeeves.ConfirmDialog(strTitle, strMsg);
            if (result == ContentDialogResult.Secondary)
            {
                try
                {
                    await athleteRepository.DeleteAthlete(view);
                    Frame.GoBack();
                }
                catch (AggregateException aex)
                {
                    string errMsg = "Errors:" + Environment.NewLine;
                    foreach (var exception in aex.InnerExceptions)
                    {
                        errMsg += Environment.NewLine + exception.Message;
                    }
                    Jeeves.ShowMessage("One or more exceptions has occurred:", errMsg);
                }
                catch (ApiException apiEx)
                {
                    string errMsg = "Errors:" + Environment.NewLine;
                    foreach (var error in apiEx.Errors)
                    {
                        errMsg += Environment.NewLine + "-" + error;
                    }
                    Jeeves.ShowMessage("Problem Deleting the Record:", errMsg);
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
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
