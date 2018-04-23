using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PWMan
{
	public partial class ChangePW : ContentPage
	{
		public ChangePW(List<string> olddata)
		{
			InitializeComponent();
            anwendung.Text = olddata[1];
            username.Text = olddata[2];
            password.Text = olddata[3];
            information.Text = olddata[4];
        }

        private async void SaveChanges(object sender, EventArgs e)
        {
            await DisplayAlert("Unzureichende Eingaben", "Bitte alle nötigen Werte aufüllen!", "Versuchs nochmal");
        }

        private async void CancelChanges(object sender, EventArgs e)
        {
            await DisplayAlert("Unzureichende Eingaben", "Bitte alle nötigen Werte aufüllen!", "Versuchs nochmal");
        }
    }
}