using SQLite;

namespace DLRGTrainer.Service
{
    interface ISQLiteDB
    {
        SQLiteAsyncConnection GetConnection();
    }
}
