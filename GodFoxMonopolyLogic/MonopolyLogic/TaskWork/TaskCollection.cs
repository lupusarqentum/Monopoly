using System.Collections.Generic;

namespace MonopolyLogic.TaskWork
{
    public interface ITaskCollection
    {
        void AddTask(GameTask task);
        
        GameTask GetRandomTask();
    }

    public sealed class TaskCollection : ITaskCollection
    {
        private readonly List<GameTask> gameTasks;

        public TaskCollection() => gameTasks = new List<GameTask>();

        public void AddTask(GameTask task) => gameTasks.Add(task);
        
        public GameTask GetRandomTask() => gameTasks[IDice.Random.Next(0, gameTasks.Count)];
    }
}
