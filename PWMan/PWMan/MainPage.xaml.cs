using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using System.IO;

namespace PWMan
{
	public partial class MainPage : ContentPage
	{
        private bool _isValid = false;

        public bool IsValid
        {
            get { return _isValid; }
            set { _isValid = value; }
        }
        Button eins, zwei, drei;
		public MainPage()
		{
			InitializeComponent();
            var button_font = Device.GetNamedSize(NamedSize.Large, typeof(Button));
            var button_layout = new LayoutOptions(LayoutAlignment.Center, false);

            eins = new Button() { Text = "eins", FontSize = button_font, BackgroundColor = Color.Gray };
            zwei = new Button() { Text = "zwei", FontSize = button_font, BackgroundColor = Color.Gray };
            drei = new Button() { Text = "drei", FontSize = button_font, BackgroundColor = Color.Gray };

            eins.Pressed += Eins_Pressed;
            zwei.Pressed += Zwei_Pressed;
            drei.Pressed += Drei_Pressed;

            var grid = new rMultiplatform.AutoGrid();
            grid.DefineGrid(1, 3);
            grid.AutoAdd(eins);
            grid.AutoAdd(zwei);
            grid.AutoAdd(drei);

            Content = grid;
        }

        private void Drei_Pressed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Zwei_Pressed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Eins_Pressed(object sender, EventArgs e)
        {
            IsValid = false;
            Certificate cert = new Certificate(txtCer.Text, txtKey.Text, txtPassword.Text);
            X509Certificate2 xcert = null;
            
            try
            {
                if (string.IsNullOrEmpty(cert.PrivateKey))
                    xcert = cert.GetCertificateFromPEMstring(true);
                else
                    xcert = cert.GetCertificateFromPEMstring(false);
            }
            catch (Exception ex)
            {
                DisplayAlert("error", "An error occure during certificate creation: Error {0}", "OK");
                return;
            }
            IsValid = true;
        }
    }
    #region

    #endregion
}
