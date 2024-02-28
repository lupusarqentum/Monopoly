namespace MonopolyLogic.TaskWork
{
    public interface ITaskSystem 
    {
        ITaskCollector TaskCollector { get; }
        ITaskPatternsRegistry TaskPatternsRegistry { get; }
    }

    public sealed class TaskSystem : ITaskSystem
    {
        public ITaskCollector TaskCollector { get; }
        public ITaskPatternsRegistry TaskPatternsRegistry { get; }

        public TaskSystem(ITaskCollector taskCollector, ITaskPatternsRegistry taskPatternsRegistry)
        {
            TaskCollector = taskCollector;
            TaskPatternsRegistry = taskPatternsRegistry;
        }

        public TaskSystem(ITaskCollector taskCollector)
        {
            TaskCollector = taskCollector;
            TaskPatternsRegistry = new TaskPatternsRegistry();

            GameTaskPatterns.ApplyTo(TaskPatternsRegistry);
        }
    }
}
