using SQLite;
using System;

namespace DLRGTrainer.Model.Exercise
{
    public class PushUp : IExercise
    {
        public static string Name = "Liegestütze";
        public static int Identifier => 0;

        [Ignore]
        string IExercise.Name => Name;
        [Ignore]
        public string Info => $"Anzahl: {Count}";

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        int IExercise.Identifier => Identifier;

        public DateTime RecordTaken { get; set; }

        [MaxLength(1024)]
        public int Count { get; set; }
    }
}
