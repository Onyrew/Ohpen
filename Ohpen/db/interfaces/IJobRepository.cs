using Ohpen.job.info;
using Ohpen.job.interfaces;

namespace Ohpen.db.interfaces
{
    internal interface IJobRepository
    {
        IJob? GetJob(Guid id);

        void AddJob(IJob job);
    }
}
