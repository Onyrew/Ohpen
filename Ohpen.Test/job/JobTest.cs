using Microsoft.Extensions.Logging;
using Moq;
using Ohpen.job;
using Ohpen.job.interfaces;
using OhpenExt.info;
using OhpenExt.processor;

namespace Ohpen.Test.job
{
    public class JobTest
    {
        private class Job_ : Job
        {
            public Job_(IProcessor _processor, IList<object> _items, ILogger<IJob> _logger) : base(_processor, _items, _logger) { }
            
            public async Task<Result> Process_(object item) => await Process(item);
        }

        private Mock<IProcessor> processor;
        private IList<object> items;
        private Mock<ILogger<IJob>> logger;

        private Job_ job;

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
            job = new Job_(processor.Object, items, logger.Object);
        }

        [Test]
        public void Constructor()
        {
            // IProcessor is null
            Assert.That(
                () => new Job_(null, Mock.Of<IList<object>>(), Mock.Of<ILogger<IJob>>()),
                Throws.Nothing
            );

            // IList<object> is null
            Assert.That(
                () => new Job_(Mock.Of<IProcessor>(), null, Mock.Of<ILogger<IJob>>()),
                Throws.Nothing
            );

            // ILogger is null
            Assert.That(
                () => new Job_(Mock.Of<IProcessor>(), Mock.Of<IList<object>>(), null),
                Throws.Nothing
            );

            Assert.That(
                () => new Job_(Mock.Of<IProcessor>(), Mock.Of<IList<object>>(), Mock.Of<ILogger<IJob>>()),
                Throws.Nothing
            );
        }

        [Test]
        public void ProcessCalledForItem_ReturnsSuccess()
        {
            // Assert
            Assert.That(job.Status.ItemCount, Is.EqualTo(3));
            Assert.That(job.Status.ProcessedCount, Is.EqualTo(0));
            Assert.That(job.Status.FailedCount, Is.EqualTo(0));

            // Act
            var result = job.Process_(items[0]);

            // Assert
            Assert.That(result.Result.Success, Is.True);
            Assert.That(job.Status.ProcessedCount, Is.EqualTo(1));
            Assert.That(job.Status.FailedCount, Is.EqualTo(0));
        }

        [Test]
        public void ProcessCalledForItem_ReturnsFailiure()
        {
            // Assert
            Assert.That(job.Status.ItemCount, Is.EqualTo(3));
            Assert.That(job.Status.ProcessedCount, Is.EqualTo(0));
            Assert.That(job.Status.FailedCount, Is.EqualTo(0));

            // Setup
            processor.Setup(v => v.Process(It.IsAny<object>()))
                .ReturnsAsync(new Result() { Success = false });

            // Act
            var result = job.Process_(items[0]);

            // Assert
            Assert.That(result.Result.Success, Is.False);
            Assert.That(job.Status.ProcessedCount, Is.EqualTo(0));
            Assert.That(job.Status.FailedCount, Is.EqualTo(1));
        }
    }
}
