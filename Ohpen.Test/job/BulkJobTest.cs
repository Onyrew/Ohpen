using Microsoft.Extensions.Logging;
using Moq;
using Ohpen.job;
using Ohpen.job.interfaces;
using OhpenExt.info;
using OhpenExt.processor;

namespace Ohpen.Test.job
{
    internal class BulkJobTest
    {
        private Mock<IProcessor> processor;
        private IList<object> items;
        private Mock<ILogger<IJob>> logger;

        private BulkJob job;

        [SetUp]
        public void Setup()
        {
            // IProcessor
            processor = new Mock<IProcessor>();
            processor.Setup(v => v.Process(It.IsAny<object>()))
                .ReturnsAsync(new Result() { Success = true });

            // IList<object>
            items = new List<object>() { new object(), new object(), new object() };

            // ILogger
            logger = new Mock<ILogger<IJob>>();

            // Job
            job = new BulkJob(processor.Object, items, logger.Object);
        }

        [Test]
        public async Task RunRunsForAllItems()
        {
            // Act
            await job.Run();

            // Assert
            Assert.That(job.Status.ItemCount, Is.EqualTo(3));
            Assert.That(job.Status.ProcessedCount, Is.EqualTo(3));
            Assert.That(job.Status.FailedCount, Is.EqualTo(0));
        }

        [Test]
        public async Task RunRunsForAllItemsEvenIfOneFails()
        {
            // Arrange
            items.Clear();
            for (int i = 0; i < 100; i++)
            {
                items.Add(new object());
            }

            // Arrange
            processor.Setup(v => v.Process(It.IsAny<object>()))
                .ReturnsAsync((object obj) => { Task.Delay(100); return new Result() { Success = true }; });
            processor.Setup(v => v.Process(items[1]))
                .ReturnsAsync(new Result() { Success = false });

            // Act
            await job.Run();

            // Assert
            Assert.That(job.Status.ProcessedCount, Is.EqualTo(99));
            Assert.That(job.Status.FailedCount, Is.EqualTo(1));
        }
    }
}
