using DLRGTrainer.Model;
using DLRGTrainer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DLRGTrainer.CreationPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MemberCreationPage : ContentPage
	{
		public MemberCreationPage(MasterPage master, Quarry quarry)
		{
			InitializeComponent();

			btn_create.Clicked += async (s, e) =>
			{
				var forename = ety_forename.Text;
				var surname = ety_surname.Text;

				var member = new Member() { Forename = forename, Surname = surname };

				await quarry.Add(member);

				await master.RefreshList();
				await master.ShowMemberDetail(member);
			};
		}

		private void OnTextChanged(object sender, EventArgs e)
		{
			btn_create.IsEnabled = true;

			foreach (var c in stackLayout.Children)
			{
				if (!(c is Entry))
					continue;

				var entry = c as Entry;

				if (!string.IsNullOrWhiteSpace(entry.Text))
					continue;

				btn_create.IsEnabled = false;

				return;
			}
		}
	}
}