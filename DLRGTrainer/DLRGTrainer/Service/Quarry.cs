using DLRGTrainer.Model;
using DLRGTrainer.Model.Exercise;
using DLRGTrainer.Model.Helper;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DLRGTrainer.Service
{
    public class Quarry
    {
        private SQLiteAsyncConnection _connection;
        public SQLiteAsyncConnection Connection => _connection;

        public Quarry(SQLiteAsyncConnection connection) => _connection = connection;

        public async Task<T> Get<T>(T model) where T : new()
            => await _connection.GetAsync<T>(model);

        public async Task<IList<T>> GetAll<T>() where T : new()
            => await _connection.Table<T>().ToListAsync();

        public async Task<IList<IExercise>> GetExercise<T>(Member member) where T : IExercise, new() => await GetExercise<T>(member.ID);
        public async Task<IList<IExercise>> GetExercise<T>(int memberID) where T : IExercise, new()
        {
            var memberExercises = await GetAll<MemberExercise>();
            var exercises = await GetAll<T>();

            return ((IEnumerable<IExercise>)from h in memberExercises
                                            join e in exercises
                                            on new { eID = h.ExerciseID, eIdef = h.ExerciseIdentifier}
                                            equals new {eID = e.ID, eIdef = e.Identifier}
                                            where h.MemberID == memberID
                                            select e).ToList();
        }

        public async Task Delete<T>(T model) where T : new()
        {
            if (model is Member)
            {
                foreach (var pushUp in await GetExercise<PushUp>(model as Member))
                    await _connection.DeleteAsync(pushUp);

                foreach (var swimming in await GetExercise<Swimming>(model as Member))
                    await _connection.DeleteAsync(swimming);
            }

            await _connection.DeleteAsync(model);
        }

        public async Task Add<T>(T model) where T : new()
            => await _connection.InsertAsync(model);
    }
}
