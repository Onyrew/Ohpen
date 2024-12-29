using Ohpen.job.interfaces;
using OhpenExt.processor;

namespace Ohpen.job
{
    internal class BulkJob : Job
    {
        public BulkJob(IProcessor _processor, IList<object> _items, ILogger<IJob> _logger) : base(_processor, _items, _logger) {}

        public override async Task Run()
        {
            foreach (var item in items)
            {
                _ = Process(item);
            }
        }
    }
}
