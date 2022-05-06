namespace Depot.TaskScheduler
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ScheduledTask
    {
        private bool isRunning;
        private readonly List<ITrigger> triggers = new();

        public ScheduledTask(Func<Task> callback)
        {
            Callback = callback;
        }

        public IReadOnlyList<ITrigger> Triggers => triggers;

        public Func<Task> Callback { get; }

        public void AddTrigger(ITrigger trigger)
        {
            triggers.Add(trigger);
            if (isRunning)
            {
                trigger.Triggered += Callback;
                trigger.OnStart();
            }
        }

        public void RemoveTrigger(ITrigger trigger)
        {
            triggers.Remove(trigger);
            if (isRunning)
            {
                trigger.OnStop();
                trigger.Triggered -= Callback;
            }
        }

        public void Start()
        {
            isRunning = true;
            foreach (var trigger in Triggers)
            {
                trigger.Triggered += Callback;
                trigger.OnStart();
            }
        }

        public void Cancel()
        {
            foreach (var trigger in Triggers)
            {
                trigger.OnStop();
                trigger.Triggered -= Callback;
            }
            isRunning = false;
        }
    }
}