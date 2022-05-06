namespace Depot.TaskScheduler
{
    using System;

    public interface ITrigger
    {
        public event Func<Task>? Triggered;

        public void OnStart();

        public void OnStop();
    }
}