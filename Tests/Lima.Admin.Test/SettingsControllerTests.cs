using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Lima.Admin.Api.Test
{
    [TestFixture]
    public class SettingsControllerTests : TestBase
    {
        public SettingsControllerTests() : base()
        {

        }

        [Test]
        public async Task Get_Company_Should_Be_Ok()
        {
            //Arrange


            //Act
            HttpResponseMessage httpResponseMessage = await HttpClient.GetAsync("/settings/company");

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, httpResponseMessage.StatusCode);
        }
    }
}
