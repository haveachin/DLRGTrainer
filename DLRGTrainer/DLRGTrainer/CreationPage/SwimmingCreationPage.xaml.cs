using DLRGTrainer.Model.Exercise;
using DLRGTrainer.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DLRGTrainer.CreationPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SwimmingCreationPage : ExerciseCreationPage
    {
        private Stopwatch stopwatch;
        private ObservableCollection<TimeSpan> laps;

        private struct Technique
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }

        public SwimmingCreationPage(MasterPage master, Quarry quarry) : base(master, quarry)
        {
            InitializeComponent();

            stopwatch = new Stopwatch();
            laps = new ObservableCollection<TimeSpan>();

            lv_laps.ItemsSource = laps;

            //lv_laps.ItemTapped += async (s, e) =>
            //{
            //};
        }

		// --- Events

        protected override void OnAppearing()
        {
            GeneratePickerItems(pkr_swimmingTechniques, LoadTechniques());

            base.OnAppearing();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
            => ClampEntry(sender as Entry, e.OldTextValue, 0, 1024);

        private bool OnTimerTick()
        {
            UpdateTimerLabel(stopwatch.Elapsed);

            return stopwatch.IsRunning;
        }

        private void OnToggle(object sender, EventArgs e)
        {
            if (stopwatch.IsRunning)
                stopwatch.Stop();
            else
            {
                stopwatch.Start();
                Device.StartTimer(new TimeSpan(0, 0, 0, 0, 1), OnTimerTick);
            }

            btn_toggle.Text = stopwatch.IsRunning ? "Pause" : "Start";
        }

        private void OnLap(object sender, EventArgs e)
            => laps.Add(stopwatch.Elapsed);

        private void OnReset(object sender, EventArgs e)
        {
            stopwatch.Reset();
            UpdateTimerLabel(stopwatch.Elapsed);
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
            => lv_laps.SelectedItem = null;

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var laps = Convert.ToInt32(ety_laps.Text);

            if (laps < 1)
            {
                await DisplayAlert("Eingabefehler", "Bitte überprüfen Sie die Anzahl der Runden!", "OK");
                return;
            }

            var technique = (Swimming.Techniques)Enum.Parse(typeof(Swimming.Techniques), pkr_swimmingTechniques.SelectedIndex.ToString());

            if (pkr_swimmingTechniques.SelectedIndex == -1)
            {
                await DisplayAlert("Eingabefehler", "Bitte wählen Sie eine Technik aus!", "OK");
                return;
            }

            var time = (TimeSpan) e.Item;

            var swimming = new Swimming()
            {
                Laps = laps,
                Technique = technique,
                Time = time,
                RecordTaken = DateTime.Now
            };

            await RegisterExercise(swimming);
        }

        private void OnDeleteLap(object sender, EventArgs e)
            => laps.Remove((TimeSpan) (sender as MenuItem).CommandParameter);

        // --- normal Methods

        private void UpdateTimerLabel(TimeSpan deltaTime)
        {
            lbl_timer.Text = $"{deltaTime:mm\\:ss\\:ff}";
            return;
        }

        private List<Technique> LoadTechniques()
        {
            var techniques = new List<Technique>();

            var names = Enum.GetNames(typeof(Swimming.Techniques));

            for (int i = 0; i < names.Length; i++)
                techniques.Add(new Technique() { ID = i, Name = names[i] });

            return techniques;
        }

        private void GeneratePickerItems(Picker picker, List<Technique> items)
        {
            foreach (var item in items)
                picker.Items.Add(item.Name);
        }
    }
}