using SQLite;
using System;

namespace DLRGTrainer.Model.Exercise
{
    public interface IExercise
    {
        int Identifier { get; }
        int ID { get; set; }
        string Name { get; }
        string Info { get; }
        DateTime RecordTaken { get; set; }
    }
}
