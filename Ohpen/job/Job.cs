using System.Collections.Concurrent;
using Ohpen.job.info;
using Ohpen.job.interfaces;
using OhpenExt.info;
using OhpenExt.processor;

namespace Ohpen.job
{
    internal abstract class Job : IJob
    {
        private readonly IProcessor processor;
        protected readonly IList<object> items;
        protected readonly ILogger<IJob> logger;

        private readonly ConcurrentBag<object> processedItems = [];
        private readonly ConcurrentBag<object> failedItems = [];
        private readonly ConcurrentBag<Result> results = [];

        public Guid Id { get; } = Guid.NewGuid();

        public Status Status => new() {
            ItemCount = items.Count, ProcessedCount = processedItems.Count, FailedCount = failedItems.Count
        };

        public IList<Result> Logs => results.ToList();

        public Job(IProcessor _processor, IList<object> _items, ILogger<IJob> _logger)
        {
            processor = _processor;
            items = _items;
            logger = _logger;
        }

        public virtual async Task Run() {}

        protected async Task<Result> Process(object item)
        {
            logger.LogInformation($"{item.GetHashCode()}: started processing.");
            var result = await processor.Process(item);

            logger.LogInformation($"{item.GetHashCode()}: success: {result.Success}, message: {result.Message}");
            Logs.Add(result);

            if (result)
            {
                processedItems.Add(item);
                return result;
            }
            failedItems.Add(item);
            return result;
        }
    }
}
