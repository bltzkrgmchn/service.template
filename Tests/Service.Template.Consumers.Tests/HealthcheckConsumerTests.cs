#pragma warning disable CS1591

using FakeItEasy;
using MassTransit;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Service.Template.Consumers.Healthchecks;

namespace Service.Template.Consumers.Tests
{
    public class HealthcheckConsumerTests
    {
        [Test]
        [Description("Проверка состояния сервиса проходит успешно")]
        public void CanCheckHealth()
        {
            ILogger<HealthcheckConsumer> logger = A.Fake<ILogger<HealthcheckConsumer>>();
            ConsumeContext<HealthcheckCommand> context = A.Fake<ConsumeContext<HealthcheckCommand>>();
            A.CallTo(() => context.Message).Returns(new HealthcheckCommand());

            HealthcheckConsumer sut = new HealthcheckConsumer(logger);
            sut.Consume(context).GetAwaiter().GetResult();

            A.CallTo(
                    () => context.RespondAsync(A<HealthcheckResponse>.That.Matches(o => o.Result == "success")))
                .MustHaveHappenedOnceExactly();
        }
    }
}

#pragma warning disable CS1591