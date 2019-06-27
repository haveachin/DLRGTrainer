using DLRGTrainer.Model.Exercise;
using DLRGTrainer.Service;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DLRGTrainer
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ExercisePage : ContentPage
	{
		private SQLiteAsyncConnection connection;
		private ObservableCollection<IExercise> exercise;

		public ExercisePage(IList<IExercise> exercise, string exerciseName)
		{
			if (exercise == null || string.IsNullOrWhiteSpace(exerciseName))
				throw new ArgumentNullException();

			InitializeComponent();

			connection = DependencyService.Get<ISQLiteDB>().GetConnection();

			Title = exerciseName;

			this.exercise = new ObservableCollection<IExercise>(exercise as IEnumerable<IExercise>);
			
			listView.ItemsSource = this.exercise;
		}

		// --- Events ---

		private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
			=> listView.SelectedItem = null;

		private async Task OnDeleteExercise(object sender, EventArgs e)
			=> await DeleteExercise((IExercise)(sender as MenuItem).CommandParameter);

		// --- Methoden ---

		private async Task DeleteExercise(IExercise exercise)
		{
			var response = await DisplayAlert("Warnung", $"Datensatz löschen?", "Löschen", "Abbrechen");

			if (!response)
				return;

			await connection.DeleteAsync(exercise);
			this.exercise.Remove(exercise);
		}
	}
}