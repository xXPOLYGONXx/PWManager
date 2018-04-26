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
	public partial class BerechtigungPW : ContentPage
	{
        WebConnect Connection = new WebConnect();
        int pid, authcount, deauthcount;
        string gid,uname;
        List<string> authusers = new List<string>();
        List<string> unauthusers = new List<string>();
        public BerechtigungPW(int pid_temp,string username)
		{
			InitializeComponent();
            pid = pid_temp;
            uname = username;
            GetUserList();
        }
        private void GetUserList()
        {
            DataTable Users = Connection.DBtoDT("Get_All_Users", "");
            DataTable Group = Connection.DBtoDT("Get_GID_By_PID", pid.ToString());  
            //richtig?
            gid = Group.Rows[0].ItemArray[0].ToString();
            DataTable authUsers = Connection.DBtoDT("Get_Auth_Users", gid);
            authusers.Clear();
            unauthusers.Clear();

            foreach (System.Data.DataRow row in Users.Rows)
            {
                unauthusers.Add(row.ItemArray[1].ToString());
                foreach (System.Data.DataRow row2 in authUsers.Rows)
                {
                    if (row.ItemArray[0].ToString() == row2.ItemArray[0].ToString())
                    {
                        authusers.Add(row.ItemArray[1].ToString());
                    }
                }
            }

            foreach (System.Data.DataRow row in authUsers.Rows)
            {
                if (unauthusers.Contains(Connection.DBtoDT("Get_Uname_By_UID", row.ItemArray[0].ToString()).Rows[0].ItemArray[0].ToString()))
                {
                    unauthusers.Remove(Connection.DBtoDT("Get_Uname_By_UID", row.ItemArray[0].ToString()).Rows[0].ItemArray[0].ToString());
                }
            }
            
            authcount = authusers.Count;
            deauthcount = unauthusers.Count;
            mitrechte.ItemsSource = authusers;
            ohnerechte.ItemsSource = unauthusers;
        }

        private async void RemoveAccess(object sender, EventArgs e)
        {

            if (mitrechte.SelectedItem != null && mitrechte.SelectedItem.ToString() != "0")
            {
                addaccess.IsEnabled = false;
                removeaccess.IsEnabled = false;
                if (authcount > 1)
                {
                    string UserID = Connection.DBtoDT("Get_Own_Uid", mitrechte.SelectedItem.ToString()).Rows[0].ItemArray[0].ToString();
                    DataTable PID = Connection.DBtoDT("Get_Own_Passwords", UserID);

                    foreach (DataRow row in PID.Rows)
                    {
                        if (Connection.DBtoDT("Get_GID_By_PID",row.ItemArray[0].ToString()).Rows[0].ItemArray[0].ToString() == gid)
                        {
                            Connection.DBRequest("Delete_Pw_By_PID", row.ItemArray[0].ToString());
                            Connection.DBRequest("Delete_Pw_By_PID_Pwmapping", row.ItemArray[0].ToString());
                        }
                        else await DisplayAlert("Fehler", Connection.DBtoDT("Get_GID_By_PID", row.ItemArray[0].ToString()).Rows[0].ItemArray[0].ToString(), "Okay");
                    }
                    authcount--;
                    deauthcount++;
                    Navigation.InsertPageBefore(new PWMan.BerechtigungPW(pid, uname), this);
                    await Navigation.PopAsync();
                    //await Navigation.PushModalAsync(new NavigationPage(new PWMan.BerechtigungPW(pid,uname)));
                }
                else
                {
                    await DisplayAlert("Rechte entziehen", "Es muss ein User mit Rechten verbleiben!", "Okay");
                }
                addaccess.IsEnabled = true;
                removeaccess.IsEnabled = true;
            }
            else await DisplayAlert("Rechte entziehen", "Wem sollen die Rechte entzogen werden???", "Ich wähle jemanden aus...");
        }

        private async void AddAccess(object sender, EventArgs e)
        {
            
            if (ohnerechte.SelectedItem != null && ohnerechte.SelectedItem.ToString() != "0")
            {
                addaccess.IsEnabled = false;
                removeaccess.IsEnabled = false;
                string username = ohnerechte.SelectedItem.ToString();
                DataTable PCount = Connection.DBtoDT("Get_PID", "");//list_pid
                int counter = 0;
                foreach (System.Data.DataRow row in PCount.Rows)
                {
                    if (Int32.Parse(row.ItemArray[0].ToString()) > counter) counter = Int32.Parse(row.ItemArray[0].ToString());
                }
                counter++;
                string UID = Connection.DBtoDT("Get_Own_Uid", username).Rows[0].ItemArray[0].ToString();
                //DataTable pkey = Form2.getSQLData("SELECT Pkey from [dbo].[UserList] WHERE Uname='" + username + "'");
                DataTable PassInfo = Connection.DBtoDT("Get_Password_From_ID", pid.ToString());
                Connection.DBRequest("Insert_New_Password", "'" + counter.ToString() + "' ,'" + PassInfo.Rows[0].ItemArray[1].ToString() + "' ,'" + PassInfo.Rows[0].ItemArray[2].ToString() + "', '" + PassInfo.Rows[0].ItemArray[3].ToString() + "', '" + PassInfo.Rows[0].ItemArray[4].ToString() + "'");//insert_pwd
                Connection.DBRequest("Insert_New_Password_Mapping", "'" + UID + "', '" + counter.ToString() + "', '" + gid + "'");

                Navigation.InsertPageBefore(new PWMan.BerechtigungPW(pid, uname), this);
                addaccess.IsEnabled = true;
                removeaccess.IsEnabled = true;
                await Navigation.PopAsync();
                //await Navigation.PushModalAsync(new NavigationPage(new PWMan.BerechtigungPW(pid, uname)));
                
            }
            else await DisplayAlert("Rechte erteilen", "Wer soll denn Rechte erhalten???", "Ich wähle jemanden aus...");

        }

        private async void GoBack(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new PWMan.MainPage(uname), this);
            await Navigation.PopAsync();
            //await Navigation.PushModalAsync(new NavigationPage(new PWMan.MainPage(uname)));
        }

        private void FirstSelected(object sender, EventArgs e)
        {
            if (ohnerechte.SelectedItem.ToString() != "0")
            {
                mitrechte.SelectedItem = 0;
            }
        }

        private void SecondSelected(object sender, EventArgs e)
        {
            if (mitrechte.SelectedItem.ToString() != "0")
            {
                ohnerechte.SelectedItem = 0;
            }
        }
    }
}