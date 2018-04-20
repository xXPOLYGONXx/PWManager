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
        List<string> authusers = new List<string>();
        List<string> unauthusers = new List<string>();
        public BerechtigungPW(int pid_temp)
		{
			InitializeComponent();
            pid = pid_temp;
            ohnerechte.ItemsSource = new string[]{
            "mono",
            "monodroid",
            "monotouch",
            "monorail",
            "monodevelop",
            "mono",
            "monodroid",
            "monotouch",
            "monorail",
            "monodevelop"
            };
            mitrechte.ItemsSource = new string[]{
            "mono",
            "monodroid",
            "monotouch",
            "monorail",
            "monodevelop",
            "mono",
            "monodroid",
            "monotouch",
            "monorail",
            "monodevelop"
            };
        }
        private void getuserlist(List<string> authusers, List<string> unauthusers)
        {
            DataTable Users = Connection.DBtoDT("Get_All_Users", "");
            DataTable Group = Connection.DBtoDT("Get_GID_By_PID", pid.ToString());  
            //richtig?
            string gid = Group.Rows[0].ItemArray[0].ToString();
            DataTable authUsers = Connection.DBtoDT("Get_Auth_Users", gid);
            authusers.Clear();
            unauthusers.Clear();

            foreach (System.Data.DataRow row in Users.Rows)
            {
                foreach (System.Data.DataRow row2 in authUsers.Rows)
                {
                    if (row.ItemArray[0].ToString() == row2.ItemArray[0].ToString())
                    {
                        authusers.Add(row.ItemArray[1].ToString());
                    }
                }
            }
            foreach (System.Data.DataRow row in Users.Rows)
            {
                foreach (System.Data.DataRow row2 in authUsers.Rows)
                {
                    if (!(row.ItemArray[0].ToString() == row2.ItemArray[0].ToString()))
                    {
                        if (!(unauthusers.Contains(row.ItemArray[1].ToString()) || unauthusers.Contains(row.ItemArray[1].ToString()))) { unauthusers.Add(row.ItemArray[1].ToString()); }
                    }
                }
            }
            authcount = authusers.Count;
            deauthcount = unauthusers.Count;
            mitrechte.ItemsSource = authusers;
            ohnerechte.ItemsSource = unauthusers;
        }

        private async void removeAccess()
        {
            if (mitrechte.SelectedItem != null)
            {
               if (authcount > 1)
                {
                    string UserID = Connection.DBtoDT("Get_Own_Uid", mitrechte.SelectedItem.ToString()).Rows[0].ItemArray[0].ToString();
                    DataTable PID = Connection.DBtoDT("Get_Own_Passwords", UserID);
                    foreach (DataRow row in PID.Rows)
                    {
                        if (row.ItemArray[1].ToString() == mitrechte.SelectedItem.ToString())
                        {
                            Connection.DBRequest("Delete_Passwd_Pwlist", row.ItemArray[0].ToString());
                            Connection.DBRequest("Delete_Passwd_Pwmapping", row.ItemArray[0].ToString());
                        }
                        else await DisplayAlert("Fehler", "Der Programmierer ist dämlich :D", "Okay");
                    }
                    authcount--;
                    deauthcount++;
                    await Navigation.PushAsync(new PWMan.BerechtigungPW(pid));
                }
                else
                {
                    await DisplayAlert("Rechte entziehen", "Es muss ein User mit Rechten verbleiben!", "Okay");
                }
            }
        }

        private async void addAccess()
        {
            
            if (ohnerechte.SelectedItem != null)
            {

                string username = ohnerechte.SelectedItem.ToString();

                /*
                
                DataTable PCount = Form2.getSQLData("SELECT PID from [dbo].[PWList]");
                int counter = 0;
                foreach (System.Data.DataRow row in PCount.Rows)
                {
                    if (Int32.Parse(row.ItemArray[0].ToString()) > counter) counter = Int32.Parse(row.ItemArray[0].ToString());
                }
                counter++;

                DataTable UID = Form2.getSQLData("SELECT UID,Pkey from [dbo].[UserList] WHERE Uname='" + username + "'");
                DataTable pkey = Form2.getSQLData("SELECT Pkey from [dbo].[UserList] WHERE Uname='" + username + "'");
                String sqlstring = "INSERT INTO [dbo].[PWList] VALUES ('" + counter.ToString() + "','" + EncryptRSA.EncryptStringforother(PW.Rows[0].ItemArray[1].ToString(), pkey.Rows[0].ItemArray[0].ToString()) + "', '" + EncryptRSA.EncryptStringforother(PW.Rows[0].ItemArray[2].ToString(), pkey.Rows[0].ItemArray[0].ToString()) + "', '" + EncryptRSA.EncryptStringforother(PW.Rows[0].ItemArray[3].ToString(), pkey.Rows[0].ItemArray[0].ToString()) + "', '" + EncryptRSA.EncryptStringforother(PW.Rows[0].ItemArray[4].ToString(), pkey.Rows[0].ItemArray[0].ToString()) + "')";
                Form2.updateSQLData(sqlstring);
                DataTable Group = Form2.getSQLData("SELECT GID from [dbo].[PWMapping] WHERE PID='" + pid + "'");
                string gid = Group.Rows[0].ItemArray[0].ToString();
                String sqlstring2 = "INSERT INTO [dbo].[PWMapping] VALUES ('" + UID.Rows[0].ItemArray[0].ToString() + "','" + counter.ToString() + "', '" + gid + "')";
                Form2.updateSQLData(sqlstring2);


    */



                await Navigation.PushAsync(new PWMan.BerechtigungPW(pid));
            }
            else await DisplayAlert("Rechte erteilen", "Wer soll denn Rechte erhalten???", "Okay");

        }
    }
}