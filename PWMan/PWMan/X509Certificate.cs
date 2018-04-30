using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace PWMan
{
    class X509Certificate
    {
        public static string EncryptString(string clearText, X509Certificate2 usercert)
        {
            if (usercert == null)
            {
                Debug.WriteLine("Keine Sicherheitszertifikat vorhanden!");
                return null;
            }
            else
            {
                byte[] clearBytes = System.Text.Encoding.UTF8.GetBytes(clearText);
                byte[] temp = EncryptData(usercert, clearBytes);
                return Convert.ToBase64String(temp);
            }

        }
        public static byte[] EncryptData(X509Certificate2 cert, byte[] data)
        {
            RSACryptoServiceProvider publicrsaprovider =
                    (RSACryptoServiceProvider)cert.PublicKey.Key;
            return publicrsaprovider.Encrypt(data, true);
        }
        public static byte[] EncryptData(RSACryptoServiceProvider rsa, byte[] data)
        {
            return rsa.Encrypt(data, true);
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string DecryptString(string cipherText, X509Certificate2 usercert)
        {
            if (usercert == null)
            {
                Debug.WriteLine("Keine Sicherheitszertifikat vorhanden!");
                return "";
            }
            else
            {
                byte[] cipherbytes = Convert.FromBase64String(cipherText);
                byte[] temp = DecryptData(usercert, cipherbytes);
                return System.Text.Encoding.UTF8.GetString(temp);
            }

        }
        public static byte[] DecryptData(X509Certificate2 cert, byte[] data)
        {
            RSACryptoServiceProvider rsaprovider =
                    (RSACryptoServiceProvider)cert.PrivateKey;
            return rsaprovider.Decrypt(data, true);
        }
        public static string EncryptStringforother(string clearText, string pkey)
        {

            RSACryptoServiceProvider publicrsaprovider = new RSACryptoServiceProvider();
            publicrsaprovider.FromXmlString(pkey);
            byte[] clearBytes = System.Text.Encoding.UTF8.GetBytes(clearText);
            byte[] temp = EncryptData(publicrsaprovider, clearBytes);
            return Convert.ToBase64String(temp);

        }
        public static string GetPublicKey()
        {
            RSACryptoServiceProvider publicrsaprovider;
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2 usercert = null;
            foreach (X509Certificate2 mCert in store.Certificates)
            {
                if (mCert.IssuerName.Name.Contains("INTCA02"))
                {
                    usercert = mCert;
                }
            }
            if (usercert == null)
            {
                Debug.WriteLine("Keine Sicherheitszertifikat vorhanden!");
                return null;
            }
            else
            {
                publicrsaprovider = (RSACryptoServiceProvider)usercert.PublicKey.Key;
                return publicrsaprovider.ToXmlString(false);
            }
        }
    }
}
