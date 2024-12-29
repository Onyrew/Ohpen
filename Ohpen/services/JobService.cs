using Ohpen.db.interfaces;
using Ohpen.job;
using Ohpen.job.info;
using Ohpen.job.interfaces;
using Ohpen.services.interfaces;
using OhpenExt.info;
using OhpenExt.processor;

namespace Ohpen.services
{
    internal class JobService : IJobService
    {
        private static readonly IDictionary<string, Func<IProcessor, IList<object>, ILogger<IJob>, IJob>> jobs = new Dictionary<string, Func<IProcessor, IList<object>, ILogger<IJob>, IJob>>() {
            { nameof(BatchJob), (IProcessor _processor, IList<object> _items, ILogger<IJob> _logger) => new BatchJob(_processor, _items, _logger) },
            { nameof(BulkJob), (IProcessor _processor, IList<object> _items, ILogger<IJob> _logger) => new BulkJob(_processor, _items, _logger) }
        };

        private readonly IJobRepository jobRepository;
        private readonly IProcessor processor;
        private readonly ILogger<IJob> logger;

        public JobService(IJobRepository _jobRepository, IProcessor _processor, ILogger<IJob> _logger)
        {
            jobRepository = _jobRepository;
            processor = _processor;
            logger = _logger;
        }

        public Guid StartJob(string _type, IList<object> _items)
        {
            if (!jobs.TryGetValue(_type, out var jobCtor))
            {
                logger.LogWarning($"Job type: {_type} not found.");
                return Guid.Empty;
            }

            var job = jobCtor(processor, _items, logger);
            jobRepository.AddJob(job);

            logger.LogInformation($"Starting job of type: {_type}, guid: {job.Id}");
            _ = job.Run();
            return job.Id;
        }

        public Status? GetJobStatus(Guid _guid)
        {
            return jobRepository.GetJob(_guid)?.Status;
        }

        public IList<Result>? GetJobLogs(Guid _guid)
        {
            return jobRepository.GetJob(_guid)?.Logs;
        }
    }
}
