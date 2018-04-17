using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net;
using System.Collections.Specialized;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace PWMan
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
		}

        private async void Login_Clicked(object sender, EventArgs e)
        {
            PHPDBConnection conn1 = new PHPDBConnection();
            //await DisplayAlert("Ausgabe", loginname.Text.ToString(), loginpasswd.Text.ToString());
            if (loginname.Text != null && loginpasswd.Text != null)
            {
                if (conn1.CheckLogin(loginname.Text.ToString(), loginpasswd.Text.ToString()))
                {
                    await Navigation.PushAsync(new PWMan.MainPage());
                    //Navigation.RemovePage(Application.Current.MainPage);
                }
                else
                {
                    await DisplayAlert("Login Fehler", "Du hast falsche Logindaten eingegeben!", "Versuchs nochmal");
                }
            }
            else
            {
                await DisplayAlert("Login Fehler", "Du hast nicht alle Logindaten eingegeben!", "Versuchs nochmal");
            }
        }
        class PHPDBConnection
        {
            private string url = "http://mrbeats.zapto.org:2622/dbrequest.php";
            private string urlLogin = "http://mrbeats.zapto.org:2622/checklogin.php";

            public void DBRequest(string keyword, string parameter)
            {
                using (WebClient client = new WebClient())
                {
                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "statement", keyword },
                        { "parameter", parameter }       //order: {"parameter name", "parameter value"}
                                                           //DB Statement, dass an den Webserver geschickt wird
                    };

                    // client.UploadValues returns page's source as byte array (byte[])
                    // so it must be transformed into a string
                    string pagesource = Encoding.UTF8.GetString(client.UploadValues(url, postData));
                    Console.Write(pagesource);
                    Console.Read();
                }
            }
            public bool CheckLogin(string username, string pwd)
            {
                using (WebClient client = new WebClient())
                {
                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "username", username },
                        { "password", pwd }       //order: {"parameter name", "parameter value"}
                                                           //DB Statement, dass an den Webserver geschickt wird
                    };

                    // client.UploadValues returns page's source as byte array (byte[])
                    // so it must be transformed into a string
                    string pagesource = Encoding.UTF8.GetString(client.UploadValues(urlLogin, postData));
                    Debug.WriteLine(pagesource);
                    //Console.Read();
                    if (pagesource == "TRUE")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        class DBConnection
        {
            private string server = "192.168.178.53";
            private string database = "pwmanager";
            private string user = "pwmanagerapp";
            private string pwd = "9575286e4DHu5U7w%cpJ6";
            private MySqlConnection connection;
            private string query = "INSERT INTO test(passwort,anwendung,beschreibung) VALUES('papapapapa','InsertTest','Insert erfolgreich')";

            public void DBConnect()
            {
                InitializeDBConnection();
                DBInsert(query);
            }

            private void InitializeDBConnection()
            {
                try
                {
                    string connStr = "server=" + server + ";database=" + database + ";uid=" + user + ";password=" + pwd + ";";
                    connection = new MySqlConnection(connStr);
                }
                catch (Exception exIniDBConn)
                {
                }
            }
            private bool OpenDBConnection()
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (MySqlException exDBOpen)
                {
                    switch (exDBOpen.Number)
                    {
                        case 0:
                            Console.WriteLine("DB-Verbindung fehlgeschlagen");
                            Console.ReadLine();
                            break;

                        case 1045:
                            Console.WriteLine("Login in DB fehlgeschlagen. Username oder Passwort falsch!");
                            Console.ReadLine();
                            break;
                        default:
                            Console.WriteLine(exDBOpen.Message);
                            Console.ReadLine();
                            break;
                    }
                    return false;
                }
            }
            private bool CloseDBConnection()
            {
                try
                {
                    connection.Close();
                    return true;
                }
                catch (MySqlException exDBClose)
                {
                    Console.WriteLine(exDBClose.Message);
                    Console.ReadLine();
                    return false;
                }

            }
            public bool DBInsert(string InsertQuery)
            {
                Console.WriteLine(InsertQuery);
                Console.ReadLine();
                //fragt ab, ob DB-Verbindung vorhanden ist
                if (OpenDBConnection() == true)
                {
                    // Erstellt einen MySql Command für die Verbindung mit dem Query
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    //Command ausführen
                    cmd.ExecuteNonQuery();

                    //DB-Verbindung schließen
                    CloseDBConnection();

                    return true;
                }
                else
                {
                    Console.WriteLine("False");
                    Console.ReadLine();
                    return false;
                }
            }
        }

    }
}