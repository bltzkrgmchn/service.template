#pragma warning disable 1591

using FakeItEasy;
using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Service.Template.Consumers.Healthchecks;
using System.Net;
using System.Threading;

namespace Service.Template.WebApi.Tests
{
    public class HealthControllerTests
    {
        [Test]
        [Description("Запрос статуса службы должен завершаться успешно.")]
        public void CanCheckHealth()
        {
            Response<HealthcheckResponse> response = A.Fake<Response<HealthcheckResponse>>();
            A.CallTo(() => response.Message).Returns(new HealthcheckResponse { Result = "success" });
            IRequestClient<HealthcheckCommand> client = A.Fake<IRequestClient<HealthcheckCommand>>();
            A.CallTo(() => client.GetResponse<HealthcheckResponse>(
                    A<HealthcheckCommand>._,
                    A<CancellationToken>._,
                    A<RequestTimeout>._))
                .Returns(response);

            HealthController sut = new HealthController(client);
            StatusCodeResult result = sut.Get().GetAwaiter().GetResult() as StatusCodeResult;

            result.Should().NotBe(null);
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}

#pragma warning restore 1591