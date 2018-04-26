using System;
using Xamarin.Forms;


namespace PWMan
{
    public partial class LoginPage : ContentPage
	{
        WebConnect Connection = new WebConnect();                                                                           //Connection to Webserver
		public LoginPage()
		{
			InitializeComponent();
		}
        private async void Login_Clicked(object sender, EventArgs e)                                                        //User wants to login
        {
            loginbutton.IsEnabled = false;                                                                                  //Block Userinput
            if (loginname.Text != null && loginpasswd.Text != null)
            {
                if (Connection.CheckLogin(loginname.Text.ToString(), loginpasswd.Text.ToString()))
                {
                    Navigation.InsertPageBefore(new PWMan.MainPage(loginname.Text.ToString()), this);                       //Create new mainpage
                    await Navigation.PopAsync();
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