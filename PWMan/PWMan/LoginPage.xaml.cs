using PCLStorage;
using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;
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
            registrationbutton.IsEnabled = false;
            if (loginname.Text != null && loginpasswd.Text != null)
            {
                DataTable User = Connection.DBtoDT("Get_All_From_UserBy_Username", loginname.Text);
                if (User == null)
                {
                    await DisplayAlert("Fehler", "Du hast einen falschen Username angegeben!", "Username überprüfen");
                    //Entsperren des Buttons
                    loginbutton.IsEnabled = true;
                    registrationbutton.IsEnabled = true;
                }
                else
                {
                    string tmpUID = User.Rows[0].ItemArray[0].ToString();
                    string X509UserCertFileName = loginname.Text + "_" + tmpUID + ".p12";
                    IFolder folder = FileSystem.Current.LocalStorage;
                    string existFile = folder.CheckExistsAsync(X509UserCertFileName).Result.ToString();
                    if (existFile == "FileExists")
                    {
                        IFile X509UserCertFile = await folder.GetFileAsync(X509UserCertFileName);
                        X509Certificate2 X509UserCert = new X509Certificate2();
                        X509UserCert.Import(X509UserCertFile.Path);
                        if (X509Certificate.DecryptString(User.Rows[0].ItemArray[2].ToString(), X509UserCert) == loginpasswd.Text)
                        {
                            Navigation.InsertPageBefore(new PWMan.MainPage(loginname.Text.ToString()), this);                       //Create new mainpage
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await DisplayAlert("Login Fehler", "Du hast falsche Logindaten eingegeben!", "Versuchs nochmal");
                            loginpasswd.Text = "";
                            //Entsperren des Buttons
                            loginbutton.IsEnabled = true;
                            registrationbutton.IsEnabled = true;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Fehler", "Zertifikat nicht vorhanden.", "Support anschreiben");
                        //Entsperren des Buttons
                        loginbutton.IsEnabled = true;
                        registrationbutton.IsEnabled = true;
                    }
                }
            }
            else
            {
                await DisplayAlert("Login Fehler", "Du hast nicht alle Logindaten eingegeben!", "Versuchs nochmal");
                loginbutton.IsEnabled = true;
                registrationbutton.IsEnabled = true;
            }
        }
        private async void Registration_Clicked(object sender, EventArgs e)
        {
            //Aufrufen der Registrieren-Seite
            Navigation.InsertPageBefore(new PWMan.RegistrationPage(), this);
            await Navigation.PopAsync();
        }
    }
}