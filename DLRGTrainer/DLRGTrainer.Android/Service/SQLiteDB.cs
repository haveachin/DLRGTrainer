using System;
using System.IO;
using DLRGTrainer.Service;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(DLRGTrainer.Droid.Service.SQLiteDB))]

namespace DLRGTrainer.Droid.Service
{
    class SQLiteDB : ISQLiteDB
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "DLRGTrainer.db3");

            return new SQLiteAsyncConnection(path);
        }
    }
}
