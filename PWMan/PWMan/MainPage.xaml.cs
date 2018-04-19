using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using System.Data;
using System.Diagnostics;

namespace PWMan
{
	public partial class MainPage : ContentPage
	{
        string uid;
        string UserID;
        WebConnect Connection = new WebConnect();
        public MainPage(string username)
		{
			InitializeComponent();
            getPWList(username);
        }
        private void getPWList(string username)
        {
            PasswordListView.ItemsSource = new string[] { };

            byte[] tempbytes = Connection.DBRequest("Get_Own_Uid",username);
            uid = System.Text.Encoding.Default.GetString(tempbytes);
            DataTable UID = Connection.FetchToDT(uid);
            //Testausgabe
            Connection.PrintDTtoDebug(UID);
            foreach (System.Data.DataRow row in UID.Rows)
            {
                UserID = row.ItemArray[0].ToString();
            }
            /*
            DataTable PID = getSQLData("SELECT PID from [dbo].[PWMapping] where UID='" + UserID + "'");
            foreach (System.Data.DataRow row in PID.Rows)
            {
                PIDs.Add(row.ItemArray[0].ToString());
            }
            string pidstring = "";
            foreach (string pid in PIDs)
            {
                if (pidstring == "")
                {
                    pidstring = "SELECT PID,Anwendung,Username,Passwort,Informationen from [dbo].[PWList] where PID='" + pid + "'";
                }
                else
                {
                    pidstring = pidstring + " OR PID='" + pid + "'";
                }
            }
            Listview = getSQLData(pidstring);
            foreach (System.Data.DataRow row in Listview.Rows)
            {
                listBox1.Items.Add(EncryptRSA.DecryptString(row.ItemArray[1].ToString()));
            }
            if (listBox1.Items.Count > 0) listBox1.SelectedIndex = 0;*/
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
            //string delete = await DisplayActionSheet("Bist du sicher das du das Passwort bei allen Besitzern löschen möchtest?", "NEIN!", "ja");
            string delete = await DisplayActionSheet(uid, "NEIN!", "ja");
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
