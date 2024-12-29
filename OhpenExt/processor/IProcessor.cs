using OhpenExt.info;

namespace OhpenExt.processor
{
    public interface IProcessor
    {
        Task<Result> Process(object item);
    }
}
