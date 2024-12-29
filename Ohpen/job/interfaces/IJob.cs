using Ohpen.job.info;
using OhpenExt.info;

namespace Ohpen.job.interfaces
{
    internal interface IJob
    {
        Guid Id { get; }

        Status Status { get; }

        IList<Result> Logs { get; }

        Task Run();
    }
}
