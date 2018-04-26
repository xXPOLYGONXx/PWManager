using Plugin.Clipboard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xamarin.Forms;

namespace PWMan
{
    public partial class MainPage : ContentPage
	{
        public string username, UserID;                                   //Save until Closing of App
        WebConnect Connection = new WebConnect();                         //Connection Webserver   

        public MainPage(string username_t)
        {
			InitializeComponent();
            username = username_t;
            UserID = Connection.DBtoDT("Get_Own_Uid", username).Rows[0].ItemArray[0].ToString();
            tableview.IsVisible = false;                                  //Hide Tableview until password is selected
            GetPWList(username);                                          //Fill Listview with data
        }
        private         List<string[]> GetPassasList(string username)     //Get passwordlist from webserver
        {
            //variables
            DataTable Listview = new DataTable();
            DataTable PID = Connection.DBtoDT("Get_Own_Passwords", UserID); //Gets all passwords from one user
            List<string> passwordnamelist = new List<string>();
            //
            Listview.Columns.Add("PID");
            Listview.Columns.Add("Anwendung");
            Listview.Columns.Add("Username");
            Listview.Columns.Add("Passwort");
            Listview.Columns.Add("Informationen");
            if (PID.Rows.Count > 0) //convert data to List
            {
                foreach (System.Data.DataRow row in PID.Rows)
                {
                    passwordnamelist.Add(row.ItemArray[0].ToString());
                }
                Listview.Rows.Clear();
                foreach (string pid in passwordnamelist)
                {
                    Listview.Rows.Add(Connection.FetchToDT(System.Text.Encoding.Default.GetString(Connection.DBRequest("Get_Password_From_ID", pid))).Rows[0].ItemArray);
                }
                
            }
            //Parse Datatable to List<string[]>
            List<string[]> passwordlist =
                    Listview.Select()
                        .Select(dr =>
                            dr.ItemArray
                                .Select(x => x.ToString())
                                .ToArray())
                            .ToList();
            return passwordlist;
        }
        public          void GetPWList(string username)                   //Fill Listview with data
        {
            PasswordListView.ItemsSource = new string[] { };    //Clear Listview
            List<string[]> allpasswords = GetPassasList(username); //Get all passwords
            PasswordListView.ItemsSource = allpasswords; //Refill Listview
            PasswordListView.SelectedItem = 0;
            tableview.IsVisible = false;    //Hide Tableview until new password is selected
        } 
        private async   void NewPW(object sender, EventArgs e)            //create new password entry
        {
            await Navigation.PushAsync(new PWMan.NewPW(UserID,username));
        }
        private async   void ChangePW(object sender, EventArgs e)         //Update passwort values
        {
            //Convert Selected Item to List
            if (PasswordListView.SelectedItem.ToString() != "0")
            {
                object a = PasswordListView.SelectedItem;
                List<string> actualPW = new List<string>();
                if (a is IEnumerable enumerable)
                {
                    foreach (object item in enumerable)
                    {
                        actualPW.Add(item.ToString());
                    }
                }
                await Navigation.PushAsync(new PWMan.ChangePW(actualPW,username,this));
            }
            else await DisplayAlert("Passwort ändern", "Du musst ein Passwort auswählen...", "Okay");
        }
        private async   void ChangePWacl(object sender, EventArgs e)      //Change access for other Users
        {
            int pid = 0;
            object selectedarray = PasswordListView.SelectedItem;       //Get selected password
            //Convert selected password to List
            List<string> actualData = new List<string>();
            if (selectedarray is IEnumerable enumerable)
            {
                foreach (object item in enumerable)
                {
                    actualData.Add(item.ToString());
                }
                pid = Int32.Parse(actualData[0]);
            }
            if (pid != 0) await Navigation.PushModalAsync(new NavigationPage(new PWMan.BerechtigungPW(pid,username)));
            else await DisplayAlert("Berechtigungen ändern", "Du musst ein Passwort auswählen...", "Okay");
        }
        private async   void DelPW(object sender, EventArgs e)            //Delete PW for all users
        {
            //Get PW from Event
            object a = PasswordListView.SelectedItem;
            List<string> actualPW = new List<string>();
            if (a is IEnumerable enumerable)
            {
                foreach (object item in enumerable)
                {
                    actualPW.Add(item.ToString());
                }
            }
            if (PasswordListView.SelectedItem.ToString() != "0")
            {
                bool result = await DisplayAlert("Ausgewähltes passwort löschen", "Sind Sie sicher, dass Sie dieses Passwort unwiederruflich für alle Nutzer löschen wollen?", "Ja", "NEIN");
                // If the yes button was pressed ...
                if (result == true)
                {
                    DataTable GID = Connection.DBtoDT("Get_GID_By_PID", actualPW[0].ToString());                            //find GroupID of the password
                    DataTable Group = Connection.DBtoDT("Get_PID_By_GID_Pwmapping", GID.Rows[0].ItemArray[0].ToString());   //get all passwords with this GroupID

                    foreach (System.Data.DataRow row in Group.Rows)
                    {
                        Connection.DBRequest("Delete_Pw_By_PID", row.ItemArray[0].ToString());                              //Delete all passwords with this GroupID
                    }
                    Connection.DBRequest("Delete_Pw_By_GID", GID.Rows[0].ItemArray[0].ToString());                          //Delete GroupID from Mapping
                    GetPWList(username);                                                                                    //Reload UI
                }

            }
            else
            {
                await DisplayAlert("Fehler", "Du hast kein Passwort zum löschen ausgewählt!", "Zurück");
            }

        }
        protected       void ShowPW(object sender)                        //Fill tableview with values
        {
            List<string> actualPW = new List<string>();
            object itemarray = sender;
            //convert Event to Password
            if (itemarray is IEnumerable enumerable)
            {
                foreach (object item in enumerable)
                {
                    actualPW.Add(item.ToString());
                }
            }
            tableview.IsVisible = true;
            anwlinks.Text = "Anwendung:";
            passlinks.Text = "Passwort:";
            infolinks.Text = "Informationen:";
            namelinks.Text = "Username:";
            anwrechts.Text = actualPW[1];
            passrechts.Text = actualPW[3];
            inforechts.Text = actualPW[4];
            namerechts.Text = actualPW[2];
        }
        private         void OnTAP(object sender, EventArgs e)            //Convert TapEvent to password
        {
            ListView lv = sender as ListView;   
            object password = lv.SelectedItem;
            ShowPW(password);
        }
        private async   void CopyPassword(object sender, EventArgs e)     //Save to Clipboard 
        {
            CrossClipboard.Current.SetText(passrechts.Text); 
            await DisplayAlert("Passwort kopiert", "Das Passwort wurde in deiner Zwischenablage gespeichert.", "super!");
        }
    }
}
