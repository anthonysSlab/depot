namespace Depot.TaskScheduler
{
    using System;
    using System.Threading.Tasks;

    public class ManualTrigger : ITrigger
    {
        public event Func<Task>? Triggered;

        public void Trigger()
        {
            Triggered?.Invoke().Wait();
        }

        public async Task TriggerAsync()
        {
            if (Triggered != null)
                await Triggered();
        }

        public void OnStart()
        {
        }

        public void OnStop()
        {
        }
    }
}