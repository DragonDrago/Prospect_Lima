using Lima.AuthenticationServer.Api.Services;
using Lima.Dictionaries.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Lima.Dictionary.Api.Test
{
    [TestFixture]
    public class CommonControllerTests : TestBase
    {
        public CommonControllerTests() : base()
        {

        }

        [Test]
        public async Task Get_Countries_Should_Be_Ok()
        {
            //Arrange

            //Act
            HttpResponseMessage responseMessage = await HttpClient.GetAsync("common/countries");

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
        }

        [Test]
        public async Task Get_Producers_Should_Be_Ok()
        {
            //Arrange

            //Act
            HttpResponseMessage responseMessage = await HttpClient.GetAsync("common/producers");

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
        }
    }
}
