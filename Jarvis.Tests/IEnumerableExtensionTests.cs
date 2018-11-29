using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Jarvis.Extensions;
using Jarvis.Tests.Models;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Jarvis.Tests
{
    public class IEnumerableExtensionTests
    {
        private static List<User> _users = new List<User>()
        {
            new User() { Id = 1, FirstName = "Ömer", LastName = "Cinbat", UserName = "ocinbat" },
            new User() { Id = 2, FirstName = "Eren", LastName = "Yener", UserName = "eyener" },
            new User() { Id = 3, FirstName = "Barış", LastName = "Gülmez", UserName = "bgulmez" }
        };

        [Test]
        public void PartitionTest()
        {
            IEnumerable<int> sourceList = new Fixture().CreateMany<int>(56);

            IEnumerable<IEnumerable<int>> lists = sourceList.Partition(5);

            sourceList.Count().ShouldBeEquivalentTo(lists.Sum(l => l.Count()));
        }
    }
}
