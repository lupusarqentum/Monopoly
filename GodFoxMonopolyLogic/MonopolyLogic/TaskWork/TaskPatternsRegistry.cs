using System.Reflection;
using System.Collections.Generic;

namespace MonopolyLogic.TaskWork
{
    public interface ITaskPatternsRegistry
    {
        bool AddTaskPattern(MethodInfo pattern);
        GameTask GetTaskByPattern(string patternName, object[] args);
    }

    public sealed class TaskPatternsRegistry : ITaskPatternsRegistry
    {
        private readonly Dictionary<string, MethodInfo> taskPatterns;

        public TaskPatternsRegistry()
        {
            taskPatterns = new Dictionary<string, MethodInfo>();
        }

        public bool AddTaskPattern(MethodInfo pattern)
        {
            if (!pattern.IsStatic) return false;
            if (pattern.ReturnType != typeof(GameTask)) return false;

            taskPatterns[pattern.Name] = pattern;
            return true;
        }

        public GameTask GetTaskByPattern(string patternName, object[] args)
        {
            return taskPatterns[patternName].Invoke(null, args) as GameTask;
        }
    }
}
