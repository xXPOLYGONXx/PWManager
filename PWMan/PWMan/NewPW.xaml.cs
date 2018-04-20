using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

        private async void Pwd_Save_Clicked(object sender, EventArgs e)
        {
            if (anwendung.Text != null && password.Text != null)
            {
                DataTable PCount = Connection.DBtoDT("Get_PID", "");//list_pid
                int counter = 0;
                foreach (System.Data.DataRow row in PCount.Rows)
                {
                    if (row.ItemArray[0].ToString() != "")
                    {
                        if (Int32.Parse(row.ItemArray[0].ToString()) > counter) counter = Int32.Parse(row.ItemArray[0].ToString());
                    }

                }
                counter++;
                //textBox4.Text = counter.ToString();
                //String sqlstring = "INSERT INTO [dbo].[PWList] VALUES ('" + textBox4.Text + "' ,'" + EncryptRSA.EncryptString(textBox1.Text) + "' ,'" + EncryptRSA.EncryptString(textBox2.Text) + "', '" + EncryptRSA.EncryptString(textBox3.Text) + "', '" + EncryptRSA.EncryptString(textBox5.Text) + "')";//insert_pwd
                Connection.DBRequest("Insert_New_Password", "'" + counter.ToString() + "' ,'" + anwendung.Text + "' ,'" + username.Text + "', '" + password.Text + "', '" + information.Text + "'");//insert_pwd
                DataTable GIDcount = Connection.DBtoDT("Get_GID", "");
                int GID = 0;
                foreach (System.Data.DataRow row in GIDcount.Rows)
                {
                    if (Int32.Parse(row.ItemArray[0].ToString()) > GID)
                        GID = Int32.Parse(row.ItemArray[0].ToString());
                }
                GID = GID + 1;
                Connection.DBRequest("Insert_New_Password_Mapping", "'" + UserID + "', '" + counter.ToString() + "', '" + GID + "'");

                await Navigation.PushAsync(new PWMan.MainPage(MainPageUsername));
                }
            else
            {
                await DisplayAlert("Unzureichende Eingaben", "Bitte alle nötigen Werte aufüllen!", "Versuchs nochmal");
            }
        }
    }
}