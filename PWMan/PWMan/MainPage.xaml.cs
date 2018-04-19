﻿using System;
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
        public DataTable deletlog;

        public DataTable Listview = new DataTable();
        public string username, UserID;
        public List<string> PIDs = new List<string>();
        public List<System.Data.DataRow> PWs;
        WebConnect Connection = new WebConnect();
        public MainPage(string username)
		{
			InitializeComponent();
            getPWList(username);
        }
        private void getPWList(string username)
        {
            PasswordListView.ItemsSource = new string[] { };

            DataTable UID = Connection.DBtoDT("Get_Own_Uid", username);
            //Testausgabe
            Connection.PrintDTtoDebug(UID);
            deletlog = UID;
            //foreach (System.Data.DataRow row in UID.Rows)
            //{
            //    UserID = row.ItemArray[0].ToString();
            //}

            UserID = UID.Rows[1].ItemArray[0].ToString();


            DataTable PID = Connection.DBtoDT("Get_Own_Passwords", UserID);
            if (PID.Rows.Count > 0)
            {
                foreach (System.Data.DataRow row in PID.Rows)
                {
                    PIDs.Add(row.ItemArray[0].ToString());
                }
                Listview.Columns.Add("PID");
                Listview.Columns.Add("Anwendung");
                Listview.Columns.Add("Username");
                Listview.Columns.Add("Passwort");
                Listview.Columns.Add("Informationen");
                foreach (string pid in PIDs)
                {
                    Listview.Rows.Add(Connection.FetchToDT(System.Text.Encoding.Default.GetString(Connection.DBRequest("Get_Password_From_ID", pid))).Rows[0].ItemArray);
                }
                string[] allpasswords = new string[Listview.Rows.Count];
                int counter = 0;
                foreach (System.Data.DataRow row in Listview.Rows)
                {
                    allpasswords[counter] = row.ItemArray[1].ToString();
                    counter++;
                    //listBox1.Items.Add(EncryptRSA.DecryptString(row.ItemArray[1].ToString()));
                }
                PasswordListView.ItemsSource = allpasswords;
                if (counter > 0) PasswordListView.SelectedItem = 0;
            }
                           
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
            string delete2 = await DisplayActionSheet(deletlog.Rows[0].ItemArray[0].ToString(), "NEIN!", "ja");
            delete2 = await DisplayActionSheet(deletlog.Rows[1].ItemArray[0].ToString(), "NEIN!", "ja");
            delete2 = await DisplayActionSheet(deletlog.Rows[2].ItemArray[0].ToString(), "NEIN!", "ja");
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
