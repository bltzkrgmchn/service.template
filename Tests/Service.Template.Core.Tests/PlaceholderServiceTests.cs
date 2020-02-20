using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System.Collections.Generic;

namespace Service.Template.Core.Tests
{
    public class PlaceholderServiceTests
    {
        [Test]
        [Description("Когда есть Placeholder возвращается список Placeholder.")]
        public void CanGetAllPlaceholders()
        {
            List<Placeholder> placeholders = new List<Placeholder> { new Placeholder("1"), new Placeholder("2") };
            IPlaceholderRepository repository = A.Fake<IPlaceholderRepository>();
            A.CallTo(() => repository.Find()).Returns(placeholders);
            ILogger<PlaceholderService> logger = A.Fake<ILogger<PlaceholderService>>();

            PlaceholderService sut = new PlaceholderService(repository, logger);

            List<Placeholder> result = sut.Get();
            result.Should().NotContainNulls();
            result.Should().ContainEquivalentOf(new Placeholder("1"));
            result.Should().ContainEquivalentOf(new Placeholder("2"));
        }

        [Test]
        [Description("Когда есть Placeholder возвращается Placeholder.")]
        public void CanPlaceholder()
        {
            Placeholder placeholder = new Placeholder("1");
            IPlaceholderRepository repository = A.Fake<IPlaceholderRepository>();
            A.CallTo(() => repository.Find(A<string>._)).Returns(placeholder);
            ILogger<PlaceholderService> logger = A.Fake<ILogger<PlaceholderService>>();

            PlaceholderService sut = new PlaceholderService(repository, logger);

            Placeholder result = sut.Get("test");
            result.Should().Be(placeholder);
        }

        [Test]
        [Description("Когда нет Placeholder возвращается null.")]
        public void CantGetInexistingPlaceholder()
        {
            IPlaceholderRepository repository = A.Fake<IPlaceholderRepository>();
            A.CallTo(() => repository.Find(A<string>._)).Returns(null);
            ILogger<PlaceholderService> logger = A.Fake<ILogger<PlaceholderService>>();

            PlaceholderService sut = new PlaceholderService(repository, logger);

            Placeholder result = sut.Get("test");
            result.Should().Be(null);
        }
    }
}