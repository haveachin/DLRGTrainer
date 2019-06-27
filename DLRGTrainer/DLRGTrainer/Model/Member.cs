using SQLite;

namespace DLRGTrainer.Model
{
    public class Member
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [MaxLength(255)]
        public string Forename { get; set; }

        [MaxLength(255)]
        public string Surname { get; set; }

        public string FullName => $"{Surname}, {Forename}";
    }
}
