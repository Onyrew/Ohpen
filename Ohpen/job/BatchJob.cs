using Ohpen.job.interfaces;
using OhpenExt.processor;

namespace Ohpen.job
{
    internal class BatchJob : Job
    {
        public BatchJob(IProcessor _processor, IList<object> _items, ILogger<IJob> _logger) : base(_processor, _items, _logger) {}

        public override async Task Run()
        {
            using var tokenSource = new CancellationTokenSource();
            try
            {
                await Parallel.ForEachAsync(
                    items,
                    tokenSource.Token,
                    async (item, token) => {
                        var result = await Process(item);
                        if (!result)
                        {
                            logger.LogError("Item failed. Cancelling the job.");
                            tokenSource.Cancel();
                            return;
                        }
                    }
                );
            }
            catch (Exception _e)
            {
                logger.LogCritical($"Exception: {_e.Message}.");
            }
        }
    }
}
