using Ohpen.job.info;
using OhpenExt.info;

namespace Ohpen.services.interfaces
{
    public interface IJobService
    {
        Guid StartJob(string type, IList<object> data);

        Status? GetJobStatus(Guid guid);

        IList<Result>? GetJobLogs(Guid guid);
    }
}
