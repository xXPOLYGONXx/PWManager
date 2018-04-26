using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace PWMan
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        WebConnect Connection = new WebConnect();
		public LoginPage()
		{
			InitializeComponent();
		}

        private async void Login_Clicked(object sender, EventArgs e)
        {
            loginbutton.IsEnabled = false;
            //await DisplayAlert("Ausgabe", loginname.Text.ToString(), loginpasswd.Text.ToString());
            if (loginname.Text != null && loginpasswd.Text != null)
            {
                if (Connection.CheckLogin(loginname.Text.ToString(), loginpasswd.Text.ToString()))
                {
                    Navigation.InsertPageBefore(new PWMan.MainPage(loginname.Text.ToString()), this);
                    //ARBEITE
                    await Navigation.PopAsync();
                    //await Navigation.PushModalAsync(new NavigationPage(new PWMan.MainPage(loginname.Text.ToString())));
                }
                else
                {
                    await DisplayAlert("Login Fehler", "Du hast falsche Logindaten eingegeben!", "Versuchs nochmal");
                    loginbutton.IsEnabled = true;

                }
            }
            else
            {
                await DisplayAlert("Login Fehler", "Du hast nicht alle Logindaten eingegeben!", "Versuchs nochmal");
                loginbutton.IsEnabled = true;

            }
        }
    }
}