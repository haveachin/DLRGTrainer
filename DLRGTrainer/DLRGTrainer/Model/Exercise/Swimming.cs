using SQLite;
using System;

namespace DLRGTrainer.Model.Exercise
{
    public class Swimming : IExercise
    {
        public static string Name => "Swimmen";
        public static int Identifier => 1;

        public enum Techniques
        {
            Brust,
            Kraul,
            Schmetterling,
            Rücken
        };

        [Ignore]
        string IExercise.Name => Name;
        [Ignore]
        public string Info => $"Zeit: {Time:mm\\:ss\\:ff} | Runden: {Laps} | Technik: {Enum.GetName(typeof(Swimming.Techniques), Technique)}";

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        int IExercise.Identifier => Identifier;

        [MaxLength(1024)]
        public int Laps { get; set; }

        [MaxLength(4)]
        public Techniques Technique { get; set; }

        public DateTime RecordTaken { get; set; }
        public TimeSpan Time { get; set; }
    }
}
