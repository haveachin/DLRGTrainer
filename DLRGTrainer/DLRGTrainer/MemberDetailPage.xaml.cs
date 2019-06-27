using DLRGTrainer.Model;
using DLRGTrainer.Model.Exercise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DLRGTrainer
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MemberDetailPage : ContentPage
	{
        private Member _member;
        public Member Member => _member;

        public MemberDetailPage(Member member, IDictionary<IList<IExercise>, string> exercises)
		{
			if (member == null || exercises == null)
				throw new ArgumentNullException();

            _member = member;

			BindingContext = member;

			InitializeComponent();

            GenerateTextCells(exercises);
        }

        private void GenerateTextCells(IDictionary<IList<IExercise>, string> exercises)
        {
            foreach (var e in exercises)
            {
                var textCell = new TextCell()
                {
                    Text = e.Value,
                    Detail = (GetLatestRecord(e.Key).Year != 1999) ?
                           $"Letzte Messung: {GetLatestRecord(e.Key):dd.MM.yyyy}" :
                           "Keine Messung vorhanden"
                };

                textCell.Tapped += async (s, ea)
                    => await Navigation.PushAsync(new ExercisePage(e.Key, e.Value));

                ts_exercises.Add(textCell);
            }
        }

		private DateTime GetLatestRecord<T>(IList<T> exercise) where T : IExercise
		{
			DateTime date = new DateTime(1999, 7, 13);

			foreach (var e in exercise)
				if (DateTime.Compare(e.RecordTaken, date) > 0)
					date = e.RecordTaken;

			return date;
		}
	}
}