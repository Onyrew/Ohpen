using OhpenExt.info;
using OhpenExt.processor;

namespace Ohpen.item
{
    internal class MockProcessor : IProcessor
    {
        public async Task<Result> Process(object item)
        {
            await Task.Delay(new Random().Next(100, 1000));
            return new Result {
                Success = true,
                Message = "N/A"
            };
        }
    }
}
