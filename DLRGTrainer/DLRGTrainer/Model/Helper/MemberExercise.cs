using SQLite;

namespace DLRGTrainer.Model.Helper
{
    public class MemberExercise
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int MemberID { get; set; }
        public int ExerciseID { get; set; }
        public int ExerciseIdentifier { get; set; }
    }
}
