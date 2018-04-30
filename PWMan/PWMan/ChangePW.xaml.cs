using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLStorage;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PWMan
{
	public partial class ChangePW : ContentPage
	{
        WebConnect Connection = new WebConnect();
        string pid, oldusername;
        PWMan.MainPage lastpage;

		public ChangePW(List<string> olddata,string temp_username,PWMan.MainPage lastSite)
		{
			InitializeComponent();
            //Fill Labels with Text
            anwendung.Text = olddata[1];
            username.Text = olddata[2];
            password.Text = olddata[3];
            information.Text = olddata[4];
            pid = olddata[0];
            //Save values globally
            oldusername = temp_username;
            lastpage = lastSite;
        }
        private async void SaveChanges(object sender, EventArgs e)                                                                          //Save the changes
        {
            savebutton.IsEnabled = false;                                                                                                   //block userinput
            if (anwendung.Text != "" && password.Text!= "")
            {
                string gid = Connection.DBtoDT("Get_GID_By_PID", pid).Rows[0].ItemArray[0].ToString();                                      //obtain GID
                DataTable Group = Connection.DBtoDT("Get_PID_By_GID_Pwmapping", gid);                                                       //obtain all passwords with this GID
                foreach (System.Data.DataRow row in Group.Rows)
                {
                    string updatestr = "PID='" + pid + "', Anwendung='" + anwendung.Text + "', Username='" + username.Text + "', Passwort='" + password.Text + "', Informationen='" + information.Text + "' WHERE PID='" + pid +"'";
                    Connection.DBRequest("Update_Pw_By_PID", updatestr);                                                                    //change all these passwords
                }
                lastpage.GetPWList(oldusername);                                                                                            //refresh main UI
                await Navigation.PopAsync();                                                                                                //Go to Mainpage
            }
            else
            {
                await DisplayAlert("Passwort ändern", "Bitte gib ein vollständigen Datensatz an.", "Okay");
                savebutton.IsEnabled = true;
            }
        }
    }
}