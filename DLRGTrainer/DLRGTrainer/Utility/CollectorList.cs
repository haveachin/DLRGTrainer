using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLRGTrainer.Utility
{
    public class CollectorList<T> // WIP
    {
        private List<T> list;
        //private SQLiteAsyncConnection connection;

        public CollectorList()
        {
            list = new List<T>();
        }
    }
}
