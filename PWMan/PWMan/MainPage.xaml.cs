using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using System.IO;

namespace PWMan
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            PasswordListView.ItemsSource = new string[]{
            "mono",
            "monodroid",
            "monotouch",
            "monorail",
            "monodevelop",
            "monotone",
            "monopoly",
            "monomodal",
            "mono",
            "monodroid",
            "monotouch",
            "monorail",
            "monodevelop",
            "monotone",
            "monopoly",
            "monomodal",
            "mononucleosis"
            };
        }
        private async void NewPW(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PWMan.NewPW());
        }
        private async void ChangePW(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PWMan.ChangePW());
        }
        private async void ChangePWacl(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PWMan.BerechtigungPW());
        }
        private async void DelPW(object sender, EventArgs e)
        {
            string delete = await DisplayActionSheet("Bist du sicher das du das Passwort bei allen Besitzern löschen möchtest?", "NEIN!", "Ja");

            //await DisplayAlert("Passwort löschen!", "Bist du sicher das du das Passwort bei allen Besitzern löschen möchtest?", "Ja");
        }

        private void EditClicked_Share()
        {

        }

        private void EditClicked_Create()
        {

        }
    }
}
