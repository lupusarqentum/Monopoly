using System.Collections.Generic;

namespace MonopolyLogic.TaskWork
{
    public interface ITaskCollector
    {
        void AddGameTask(string key, GameTask task);

        GameTask GetRandomTask(string key);
    }

    public sealed class TaskCollector : ITaskCollector
    {
        private readonly Dictionary<string, ITaskCollection> taskCollections;

        internal TaskCollector(params string[] collections)
        {
            taskCollections = new Dictionary<string, ITaskCollection>();

            foreach (var item in collections)
                taskCollections.Add(item, new TaskCollection());
        }

        public TaskCollector() : this(TaskCollectionsNames.AllNames) {}

        public void AddGameTask(string key, GameTask task) => taskCollections[key].AddTask(task);

        public GameTask GetRandomTask(string key) => taskCollections[key].GetRandomTask();

    }
}
