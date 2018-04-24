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
		public ChangePW(List<string> olddata,string temp_username)
		{
			InitializeComponent();
            anwendung.Text = olddata[1];
            username.Text = olddata[2];
            password.Text = olddata[3];
            information.Text = olddata[4];
            pid = olddata[0];
            oldusername = temp_username;
        }

        private async void SaveChanges(object sender, EventArgs e)
        {
            if (anwendung.Text != "" && password.Text!= "")
            {

            string gid = Connection.DBtoDT("Get_GID_By_PID", pid).Rows[0].ItemArray[0].ToString();
            DataTable Group = Connection.DBtoDT("Get_PID_By_GID_Pwmapping", gid);
            foreach (System.Data.DataRow row in Group.Rows)
            {
                string updatestr = "PID='" + pid + "', Anwendung='" + anwendung.Text + "', Username='" + username.Text + "', Passwort='" + password.Text + "', Informationen='" + information.Text + "' WHERE PID='" + pid +"'";
                Connection.DBRequest("Update_Pw_By_PID", updatestr);
            }
            await Navigation.PushModalAsync(new NavigationPage(new PWMan.MainPage(oldusername)));
            }
            else await DisplayAlert("Passwort ändern", "Bitte gib ein vollständigen Datensatz an.", "Okay");
        }

        public async Task CreateRealFileAsync()
        {
            // get hold of the file system
            IFolder rootFolder = FileSystem.Current.LocalStorage;

            // create a folder, if one does not exist already
            IFolder folder = await rootFolder.CreateFolderAsync("MySubFolder", CreationCollisionOption.OpenIfExists);

            // create a file, overwriting any existing file
            IFile file = await folder.CreateFileAsync("MyFile.txt", CreationCollisionOption.ReplaceExisting);

            // populate the file with some text
            await file.WriteAllTextAsync("Sample Text...");
        }

    }
}