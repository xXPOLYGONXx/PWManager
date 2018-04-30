using PCLStorage;
using System;
using System.Security.Cryptography.X509Certificates;
using Xamarin.Forms;

namespace PWMan
{
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }
        private async void Submit_Registration_Clicked(object sender, EventArgs e)
        {
            WebConnect Connection = new WebConnect();
            bool pwdIntegrity = false;
            bool uniqueUsername = false;
            //Sperren des Buttons, damit Mehrfachbenutzung wärent Ladezeiten verhindert werden
            Registration_Button.IsEnabled = false;

            #region CheckPwdIntegrity
            if (registration_password.Text != null && registration_repeat_password.Text != null)
            {
                if (registration_password.Text == registration_repeat_password.Text)
                {
                    pwdIntegrity = true;
                }
                else
                {
                    await DisplayAlert("Passwörter stimmen nicht überein", "Bitte überprüfen Sie die Eingabe der Passwörter", "OK");
                    //Entsperren des Buttons
                    Registration_Button.IsEnabled = true;
                }
            }
            else
            {
                await DisplayAlert("Passwörter dürfen nicht leer sein", "Bitte überprüfen Sie die Eingabe der Passwörter", "OK");
                //Entsperren des Buttons
                Registration_Button.IsEnabled = true;
            }
            #endregion

            #region CheckUniqueUsername
            string UsernameUniqueCheck = System.Text.Encoding.Default.GetString(Connection.DBRequest("Get_Own_Uid", registration_username.Text));
      
            if (UsernameUniqueCheck == "")
            {
                uniqueUsername = true;
            }
            else
            {
               
                await DisplayAlert("Username existiert schon", registration_username.Text, "OK");
                //Entsperren des Buttons
                Registration_Button.IsEnabled = true;
            }
            #endregion

            #region getCertificate
            if (pwdIntegrity && uniqueUsername)
            {
                //Insert User mit Dummy PWD, um eine UID zu erzeugen
                Connection.DBRequest("Insert_New_User_Dummy", registration_username.Text);
                //Hole die neue UID
                string NewUserID = Connection.DBtoDT("Get_Own_Uid", registration_username.Text).Rows[0].ItemArray[0].ToString();
                //Erstelle das Zertifikat auf dem Webserver
                string CertFilename = System.Text.Encoding.Default.GetString(Connection.CreateCert(NewUserID, registration_username.Text));
                //Erstelle Zertifikats-Datei
                String filename = CertFilename;
                IFolder folder = FileSystem.Current.LocalStorage;
                IFile file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                //Schreibe die gedownloadete .p12 in das die lokale .p12
                Connection.GetCertFile(CertFilename, file.Path);//Erstelle aus dem Zertifikat ein Zertifikat v2
                X509Certificate2 userCert2 = new X509Certificate2();
                userCert2.Import(file.Path);
                //Trage die Userdaten verschlüsselt in die DB ein      
                string cryptedpw = X509Certificate.EncryptString(registration_password.Text, userCert2);
                Connection.DBRequest("Update_New_User_Dummy", "Username ='" + registration_username.Text + "',Password = '" + cryptedpw + "', PublicKey='" + userCert2.PublicKey.Key.ToXmlString(false) + "' WHERE UID = " + NewUserID);
                //Zurück zur LoginPage
                Navigation.InsertPageBefore(new PWMan.LoginPage(), this);
                await Navigation.PopAsync();
            }
            //Entsperren des Buttons
            Registration_Button.IsEnabled = true;
            #endregion
        }
    }
}