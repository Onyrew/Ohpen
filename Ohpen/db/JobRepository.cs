using System.Collections.Concurrent;
using Ohpen.db.interfaces;
using Ohpen.job.interfaces;

namespace Ohpen.db
{
    internal class JobRepository : IJobRepository
    {
        private readonly ConcurrentDictionary<Guid, IJob> _jobs = new();

        public IJob? GetJob(Guid id)
        {
            _jobs.TryGetValue(id, out IJob? value);
            return value;
        }

        public void AddJob(IJob job)
        {
            _jobs.TryAdd(job.Id, job);
        }
    }
}
