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
using Plugin.Clipboard;

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
        
        public MainPage(string temp_username)
        {
			InitializeComponent();
            username = temp_username;
            GetPWList(username);
            tableview.IsVisible = false;
        }
        private void GetPWList(string username)
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
            

            UserID = Connection.DBtoDT("Get_Own_Uid", username).Rows[0].ItemArray[0].ToString();
            //UserID = System.Text.Encoding.Default.GetString(Connection.DBRequest("Get_Own_Uid", username));
            
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
                    //Debug.WriteLine(System.Text.Encoding.Default.GetString(Connection.DBRequest("Get_Password_From_ID", pid)));
                    Listview.Rows.Add(Connection.FetchToDT(System.Text.Encoding.Default.GetString(Connection.DBRequest("Get_Password_From_ID", pid))).Rows[0].ItemArray);
                }
                //string[] allpasswords = new string[Listview.Rows.Count];
                //int counter = 0;
                List<string[]> allpasswords =
                    Listview.Select()
                        .Select(dr =>
                            dr.ItemArray
                                .Select(x => x.ToString())
                                .ToArray())
                            .ToList();

                //allpasswords[counter] = row.ItemArray[1].ToString();
                //counter++;
                //listBox1.Items.Add(EncryptRSA.DecryptString(row.ItemArray[1].ToString()));

                PasswordListView.ItemsSource = allpasswords;

                if (allpasswords.Count > 0) PasswordListView.SelectedItem = 0;

            }
                           
        } 
        private async void NewPW(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PWMan.NewPW(UserID,username));
        }
        private async void ChangePW(object sender, EventArgs e)
        {
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
                await Navigation.PushAsync(new PWMan.ChangePW(actualPW,username));
            }
            else await DisplayAlert("Passwort ändern", "Du musst ein Passwort auswählen...", "Okay");
        }
        private async void ChangePWacl(object sender, EventArgs e)
        {
            int pid = 0;
            object selectedarray = PasswordListView.SelectedItem;
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
        private async void DelPW(object sender, EventArgs e)
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
            if (PasswordListView.SelectedItem.ToString() != "0")
            {
                bool result = await DisplayAlert("Ausgewähltes passwort löschen", "Sind Sie sicher, dass Sie dieses Passwort unwiederruflich für alle Nutzer löschen wollen?", "Ja", "NEIN");

                // If the yes button was pressed ...
                if (result == true)
                {
                    DataTable GID = Connection.DBtoDT("Get_GID_By_PID", actualPW[0].ToString());
                    DataTable Group = Connection.DBtoDT("Get_PID_By_GID_Pwmapping", GID.Rows[0].ItemArray[0].ToString());

                    foreach (System.Data.DataRow row in Group.Rows)
                    {
                        Connection.DBRequest("Delete_Pw_By_PID", row.ItemArray[0].ToString());
                    }
                    Connection.DBRequest("Delete_Pw_By_GID", GID.Rows[0].ItemArray[0].ToString());

                    //await Navigation.PushAsync(new PWMan.MainPage(username));
                    //await Navigation.PushModalAsync(new NavigationPage(new PWMan.MainPage(username)));

                    Navigation.InsertPageBefore(new PWMan.MainPage(username), this);
                    await Navigation.PopAsync();
                }

            }
            else
            {
                await DisplayAlert("Fehler", "Du hast kein Passwort zum löschen ausgewählt!", "Zurück");
            }

        }
        protected void ShowPW(object sender)
        {
            //MenuItem parameters = sender as MenuItem;
            //object itemarray = parameters.CommandParameter;
            object itemarray = sender;
            List<string> actualPW = new List<string>();
            if (itemarray is IEnumerable enumerable)
            {
                foreach (object item in enumerable)
                {
                    actualPW.Add(item.ToString());
                }
            }
            string Passwd = "PID: " + actualPW[0] + "\nAnwendung: " + actualPW[1] + "\nUsername: " + actualPW[2] + "\nPasswort: " + actualPW[3] + "\nzus. Infos: " + actualPW[4];
            tableview.IsVisible = true;
            anwlinks.Text = "Anwendung:";
            passlinks.Text = "Passwort:";
            infolinks.Text = "Informationen:";
            namelinks.Text = "Username:";
            anwrechts.Text = actualPW[1];
            passrechts.Text = actualPW[3];
            inforechts.Text = actualPW[4];
            namerechts.Text = actualPW[2];

            //await DisplayAlert("Passwort:", Passwd, "Okay");
        }
        protected async void LongPress(object sender, EventArgs e)
        {
            await DisplayAlert("error", sender.ToString(), "kk");
        }
        private void OnTAP(object sender, EventArgs e)
        {
            ListView lv = sender as ListView;
            object password = lv.SelectedItem;
            //PasswordListView.SelectedItem = password;
            ShowPW(password);
        }
        private async void CopyPassword(object sender, EventArgs e)
        {
            CrossClipboard.Current.SetText(passrechts.Text);
            await DisplayAlert("Passwort kopiert", "Das Passwort wurde in deiner Zwischenablage gespeichert.", "super!");
        }
    }
}
