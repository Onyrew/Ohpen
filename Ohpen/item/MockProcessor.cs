using OhpenExt.info;
using OhpenExt.processor;

namespace Ohpen.item
{
    internal class MockProcessor : IProcessor
    {
        public async Task<Result> Process(object item)
        {
            await Task.Delay(500);
            return new Result {
                Success = true,
                Message = "N/A"
            };
        }
    }
}
