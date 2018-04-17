using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace PWMan
{
    public class WebConnect
    {
            private string url = "http://mrbeats.zapto.org:2622/dbrequest.php";
            private string urlLogin = "http://mrbeats.zapto.org:2622/checklogin.php";

            public byte[] DBRequest(string keyword, string parameter)
            {
                using (WebClient client = new WebClient())
                {
                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "statement", keyword },
                        { "parameter", parameter }       //order: {"parameter name", "parameter value"}
                                                           //DB Statement, dass an den Webserver geschickt wird
                    };
                return client.UploadValues(url, postData);
                    // client.UploadValues returns page's source as byte array (byte[])
                    // so it must be transformed into a string
                
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
                    string pagesource = Encoding.UTF8.GetString(client. client.UploadValues(urlLogin, postData));
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
}
