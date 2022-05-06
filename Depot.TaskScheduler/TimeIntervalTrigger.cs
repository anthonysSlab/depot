namespace Depot.TaskScheduler
{
    using System;
    using System.Threading.Tasks;

    public class TimeIntervalTrigger : ITrigger
    {
        private Task? task;
        private CancellationTokenSource source;

        public TimeIntervalTrigger(TimeSpan interval)
        {
            Interval = interval;
            source = new();
        }

        public TimeSpan Interval { get; set; }

        public event Func<Task>? Triggered;

        public void OnStart()
        {
            source = new();
            task = TriggerVoid();
        }

        public void OnStop()
        {
            source?.Cancel();
            task?.Wait();
            source?.Dispose();
            task?.Dispose();
        }

        private async Task TriggerVoid()
        {
            while (!source.IsCancellationRequested)
            {
                await Task.Delay(Interval, source.Token);
                if (source.IsCancellationRequested) return;
                if (Triggered != null)
                    await Triggered();
            }
        }
    }
}