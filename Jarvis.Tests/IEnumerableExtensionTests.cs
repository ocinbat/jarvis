using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Jarvis.Tests
{
    public class IEnumerableExtensionTests
    {
        [Test]
        public void PartitionTest()
        {
            IEnumerable<int> sourceList = new Fixture().CreateMany<int>(56);

            IEnumerable<IEnumerable<int>> lists = sourceList.Partition(5);

            sourceList.Count().ShouldBeEquivalentTo(lists.Sum(l => l.Count()));
        }
    }
}
