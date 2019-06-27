using DLRGTrainer.CreationPage;
using DLRGTrainer.Model;
using DLRGTrainer.Model.Exercise;
using DLRGTrainer.Model.Helper;
using DLRGTrainer.Service;
using DLRGTrainer.Utility;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DLRGTrainer
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterPage : MasterDetailPage
	{
		private string _searchText;
		private SQLiteAsyncConnection _connection;
		private Quarry _quarry;

		public MasterPage()
		{
			InitializeComponent();

			_connection = DependencyService.Get<ISQLiteDB>().GetConnection();
			_quarry = new Quarry(_connection);
		}
		
		// --- Events

		protected override async void OnAppearing()
		{
			await _connection.CreateTablesAsync<Member, PushUp, Swimming, MemberExercise>();

			await RefreshList();

			base.OnAppearing();

            if ((await GetMembers()).Count() < 1)
                Detail = new MemberCreationPage(this, _quarry);
        }

		protected override bool OnBackButtonPressed()
		{
			IsPresented = !IsPresented;
			return true;
		}

		private async void OnDeleteMember(object sender, EventArgs e)
			=> await DeleteMember((Member)(sender as MenuItem).CommandParameter);

		private async void OnRefreshing(object sender, EventArgs e)
		{
			await RefreshList();

			listView.EndRefresh();
		}

		private async void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
			=> await RefreshList(e.NewTextValue);

		private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
			=> await ShowMemberDetail(e.SelectedItem as Member);

		private void OnItemTapped(object sender, ItemTappedEventArgs e)
			=> IsPresented = false;

		private async void OnAdd(object sender, EventArgs e)
		{
			IsPresented = false;

            if ((await GetMembers()).Count() < 1)
            {
                Detail = new MemberCreationPage(this, _quarry);
                return;
            }

			var response = await DisplayActionSheet("Anlegen", "Abbrechen", "", "Mitglied", "Liegestütze", "Schwimmen");

			switch (response)
			{
				case "Mitglied":
					Detail = new MemberCreationPage(this, _quarry);
					break;

				case "Liegestütze":
					Detail = new PushUpCreationPage(this, _quarry);
					break;

				case "Schwimmen":
					Detail = new SwimmingCreationPage(this, _quarry);
					break;

				default:
					// Abbrechen
					break;
			}
		}

		private async void OnDrop(object sender, EventArgs e)
		{
			var response = await DisplayAlert("Master RESET", "DROP all tables?", "yes", "no");

			if (!response)
				return;

			await _connection.DropTableAsync<Member>();
			await _connection.DropTableAsync<PushUp>();
			await _connection.DropTableAsync<Swimming>();
			await _connection.DropTableAsync<MemberExercise>();

			await DisplayAlert("Master RESET", "all tables droped!", "ok");

			OnAppearing();
		}

		// --- normal Methods

		private async Task DeleteMember(Member member)
		{
			var response = await DisplayAlert(
				"Warnung",
				$"Mitglied '{member.FullName}' löschen?",
				"Löschen",
				"Abbrechen");

			if (!response)
				return;

            if ((Detail as MemberDetailPage)?.Member == member)
                Detail = new MemberCreationPage(this, _quarry);
            
            await _quarry.Delete(member);
			await RefreshList();
		}

		public async Task ShowMemberDetail(Member member)
		{
			Detail = new MemberDetailPage(member, await GetExercises(member));
			IsPresented = false;
		}

		public async Task RefreshList(string searchText = null)
		{
			if (searchText != null)
				_searchText = searchText;

			listView.ItemsSource = await GetMembers(searchText);
		}

		public async Task<IEnumerable<Member>> GetMembers(string searchText = null)
		{
			var allMembers = await _quarry.GetAll<Member>();

			if (string.IsNullOrWhiteSpace(searchText))
				return allMembers;

			return allMembers.Where(m => m.FullName.Contains(searchText));
		}

		private async Task<IDictionary<IList<IExercise>, string>> GetExercises(Member member)
		{
			return new Dictionary<IList<IExercise>, string>
			{
				{
					await _quarry.GetExercise<PushUp>(member),
					PushUp.Name
				},
				{
					await _quarry.GetExercise<Swimming>(member),
					Swimming.Name
				}
			};
		}
	}
}