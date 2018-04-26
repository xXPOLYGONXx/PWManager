using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace PWMan
{
    public class WebConnect
    {
            private string url = "http://mrbeats.zapto.org:2622/dbrequest.php";                                     //Raspi 3
            private string urlLogin = "http://mrbeats.zapto.org:2622/checklogin.php";

            public byte[] DBRequest(string keyword, string parameter)                                               //simple DB Request
            {
                using (WebClient client = new WebClient())
                {
                    NameValueCollection postData = new NameValueCollection()                                        //send params via POST
                    {
                        { "statement", keyword },
                        { "parameter", parameter }                                                                  //statement = name of db-rquest, parameter changes depending on statement                               
                    };
                return client.UploadValues(url, postData);                
                }
            }
            public bool CheckLogin(string username, string pwd)                                                     //Login via username and password
            {
                using (WebClient client = new WebClient())
                {
                    NameValueCollection postData = new NameValueCollection()
                    {
                        { "username", username },
                        { "password", pwd }                                                           
                    };
                    string pagesource = Encoding.UTF8.GetString(client.UploadValues(urlLogin, postData));           //Byte[] to string conversion
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
            public DataTable FetchToDT(string input)                                                                //Convert DBRequest string to DataTable
            {
            DataTable sqlreturn = new DataTable();
            List<string> resultlist = new List<string>();
            List<string[]> splittedresults = new List<string[]>();
            string pattern = "[|]";
            string newpattern = "[;]";
            string[] result = Regex.Split(input, pattern);
            
            foreach (string item in result)
            {
                resultlist.Add(item);                                                                                //Every result from the request converts to a datatable row
            }
            resultlist.RemoveAt(resultlist.Count-1);
            resultlist.RemoveAt(0);
            
            foreach (string Element in resultlist)
            {
                splittedresults.Add(Regex.Split(Element, newpattern));                                              //Split each column of every row
            }
            for (int ctr = 0; ctr < splittedresults[0].Length; ctr++)
            {
                sqlreturn.Columns.Add(ctr.ToString(), typeof(String));                                              //prepare DataTable
            }
            foreach (string[] row in splittedresults)
            {
                sqlreturn.Rows.Add(row);                                                                            //Get everything back together
            }
            return sqlreturn;
            }
            public DataTable DBtoDT(string keyword, string parameter)                                               //Simplifys handling of the connector
            {
            return FetchToDT(System.Text.Encoding.Default.GetString(DBRequest(keyword, parameter)));
            }
    }
}
