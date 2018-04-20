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
            //await DisplayAlert("Ausgabe", loginname.Text.ToString(), loginpasswd.Text.ToString());
            if (loginname.Text != null && loginpasswd.Text != null)
            {
                if (Connection.CheckLogin(loginname.Text.ToString(), loginpasswd.Text.ToString()))
                {
                    await Navigation.PushModalAsync(new NavigationPage(new PWMan.MainPage(loginname.Text.ToString())));
                    //Navigation.RemovePage(Application.Current.MainPage);
                }
                else
                {
                    await DisplayAlert("Login Fehler", "Du hast falsche Logindaten eingegeben!", "Versuchs nochmal");
                }
            }
            else
            {
                await DisplayAlert("Login Fehler", "Du hast nicht alle Logindaten eingegeben!", "Versuchs nochmal");
            }
        }
    }
}