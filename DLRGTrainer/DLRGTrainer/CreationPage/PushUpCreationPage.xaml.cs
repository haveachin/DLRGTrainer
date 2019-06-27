using DLRGTrainer.Model.Exercise;
using DLRGTrainer.Service;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DLRGTrainer.CreationPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PushUpCreationPage : ExerciseCreationPage
	{
		public PushUpCreationPage(MasterPage master, Quarry quarry) : base(master, quarry)
		{
			InitializeComponent();
		}

		private async void OnCreate(object sender, EventArgs e)
		{
			var count = Convert.ToInt32(ety_count.Text);

			if (count < 1)
			{
				await DisplayAlert("Eingabefehler", "Bitte überprüfen Sie die Anzahl der Liegestützen!", "OK");
				return;
			}

			var pushUp = new PushUp() { Count = count, RecordTaken = DateTime.Now };

			await RegisterExercise(pushUp);
		}

		private void OnTextChanged(object sender, TextChangedEventArgs e)
			=> ClampEntry(sender as Entry, e.OldTextValue, 0, 1024);
	}
}