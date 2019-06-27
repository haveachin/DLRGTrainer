using DLRGTrainer.Model;
using DLRGTrainer.Model.Exercise;
using DLRGTrainer.Model.Helper;
using DLRGTrainer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DLRGTrainer.CreationPage
{
    public abstract class ExerciseCreationPage : ContentPage
    {
        private MasterPage _master;
        private Quarry _quarry;

        public ExerciseCreationPage(MasterPage master, Quarry quarry)
        {
            _master = master;
            _quarry = quarry;
        }

        protected void ClampEntry(Entry entry, string oldTextValue, int min, int max)
        {
            int input = 0;

            try
            {
                input = Convert.ToInt32(entry.Text);
            }
            catch (Exception)
            {
                if (entry.Text != "")
                    entry.Text = oldTextValue;

                entry.Text = $"{min}";
                return;
            }

            if (input < min)
                entry.Text = $"{min}";
            else if (input > max)
                entry.Text = $"{max}";
        }

        /// <summary>
        /// Adds a given exercise to a member
        /// </summary>
        /// <typeparam name="T">Any class that inherits from IExercise</typeparam>
        /// <param name="exercise">A exercise</param>
        /// <returns>The success of the operation</returns>
        protected async Task<bool> RegisterExercise<T>(T exercise) where T : IExercise, new()
        {
            int memberID = await SelectMember(await _master.GetMembers());

            if (memberID < 0)
                return false;

            await _quarry.Add(exercise);
            await AddExerciseToMember(_quarry, memberID, exercise.ID, exercise.Identifier);

            return true;
        }

        /// <summary>
        /// Selects a member from all members registered in the DB
        /// </summary>
        /// <param name="members">An instance of a member class</param>
        /// <returns>The id of the selected member. -1 if no member was selected</returns>
        private async Task<int> SelectMember(IEnumerable<Member> members)
        {
            var memberNames = (from m in members
                               select m.FullName).ToArray();

            if (memberNames.Length < 1)
                return -1;

            var memberName = await DisplayActionSheet(
                "Mitglied wählen",
                "",
                "",
                memberNames);

            if (string.IsNullOrWhiteSpace(memberName))
                return -1;

            var memberID = (from m in members
                            where m.FullName == memberName
                            select m.ID).ToList();

            return memberID[0];
        }

        /// <summary>
        /// It couples an exercise with a member
        /// </summary>
        /// <param name="quarry">Quarry to the DB</param>
        /// <param name="exerciseID">Id of an exercise</param>
        /// <param name="memberID">Id of a member</param>
        /// <returns></returns>
        private async Task AddExerciseToMember(Quarry quarry, int memberID, int exerciseID, int exerciseIdentifier)
            => await quarry.Add(new MemberExercise() { MemberID = memberID, ExerciseID = exerciseID, ExerciseIdentifier = exerciseIdentifier });
    }
}
