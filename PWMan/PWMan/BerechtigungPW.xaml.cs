using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PWMan
{
	public partial class BerechtigungPW : ContentPage
	{
		public BerechtigungPW()
		{
			InitializeComponent();
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
	}
}