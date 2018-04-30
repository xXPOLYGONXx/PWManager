using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using Xamarin.Forms;

namespace PWMan
{
    public partial class NewPW : ContentPage
    {
        public string UserID, MainPageUsername;
        public X509Certificate2 X509UserCert;
        WebConnect Connection = new WebConnect();

        public NewPW(string UID, string uname, X509Certificate2 X509UserCert_t)
        {
            InitializeComponent();
            UserID = UID;
            MainPageUsername = uname;
            X509UserCert = X509UserCert_t;
        }
        private async void Pwd_Save_Clicked(object sender, EventArgs e)                                                                                     //Save the new password
        {
            int counter = 0;
            if (anwendung.Text != null && password.Text != null)                                                                                            //The least of input
            {
                byte[] tmp = Connection.DBRequest("Get_PID", "");
                if (tmp.Length != 0)
                {
                    savebutton.IsEnabled = false;                                                                                                               //Block Userinput
                    DataTable PCount = Connection.DBtoDT("Get_PID", "");                                                                                        //list all PIDs to calculate the next higher one
                    foreach (System.Data.DataRow row in PCount.Rows)
                    {
                        if (row.ItemArray[0].ToString() != "")
                        {
                            if (Int32.Parse(row.ItemArray[0].ToString()) > counter) counter = Int32.Parse(row.ItemArray[0].ToString());
                        }
                    }
                }
                counter++;
                //Das PW muss verschlüsselt einmal in einem String gespeichert werden, da es sonst Probleme gibt
                string cryptednewpw = X509Certificate.EncryptString(password.Text, X509UserCert);
                string parameter = "'" + counter.ToString() + "' ,'" + anwendung.Text + "' ,'" + username.Text + "', '" + cryptednewpw + "', '" + information.Text + "'";
                Connection.DBRequest("Insert_New_Password", parameter); //insert password in database

                int GID = 0;
                byte[] tmpGID = Connection.DBRequest("Get_GID", "");
                if (tmpGID.Length != 0)
                {
                    DataTable GIDcount = Connection.DBtoDT("Get_GID", "");                                                                                      //list all GIDs to calculate the next higher one

                    foreach (System.Data.DataRow row in GIDcount.Rows)
                    {
                        if (Int32.Parse(row.ItemArray[0].ToString()) > GID)
                            GID = Int32.Parse(row.ItemArray[0].ToString());
                    }
                    GID = GID + 1;
                }
                else
                {
                    GID = 1;
                }
                Connection.DBRequest("Insert_New_Password_Mapping", "'" + UserID + "', '" + counter.ToString() + "', '" + GID + "'");                       //insert new password in mapping table
                Navigation.InsertPageBefore(new PWMan.MainPage(MainPageUsername, X509UserCert), this);                                                                    //Go back to Mainpage
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Unzureichende Eingaben", "Bitte alle nötigen Werte aufüllen!", "Versuchs nochmal");
            }
        }
    }
}