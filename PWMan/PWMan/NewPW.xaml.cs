using System;
using System.Data;
using Xamarin.Forms;

namespace PWMan
{
    public partial class NewPW : ContentPage
    {
        public string UserID, MainPageUsername;
        WebConnect Connection = new WebConnect();

        public NewPW(string UID, string uname)
        {
            InitializeComponent();
            UserID = UID;
            MainPageUsername = uname;
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
                Connection.DBRequest("Insert_New_Password", "'" + counter.ToString() + "' ,'" + anwendung.Text + "' ,'" + username.Text + "', '" + password.Text + "', '" + information.Text + "'"); //insert password in database
                DataTable GIDcount = Connection.DBtoDT("Get_GID", "");                                                                                      //list all GIDs to calculate the next higher one
                int GID = 0;
                foreach (System.Data.DataRow row in GIDcount.Rows)
                {
                    if (Int32.Parse(row.ItemArray[0].ToString()) > GID)
                        GID = Int32.Parse(row.ItemArray[0].ToString());
                }
                GID = GID + 1;
                Connection.DBRequest("Insert_New_Password_Mapping", "'" + UserID + "', '" + counter.ToString() + "', '" + GID + "'");                       //insert new password in mapping table
                Navigation.InsertPageBefore(new PWMan.MainPage(MainPageUsername), this);                                                                    //Go back to Mainpage
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Unzureichende Eingaben", "Bitte alle nötigen Werte aufüllen!", "Versuchs nochmal");
            }
        }
    }
}