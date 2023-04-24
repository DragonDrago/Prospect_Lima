using Lima.AuthenticationServer.Api.Services;
using Lima.Dictionaries.Api;
using Lima.Dictionaries.Api.Requests;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Lima.Dictionary.Api.Test
{
    [TestFixture]
    public class DoctorsControllerTests : TestBase
    {
        public DoctorsControllerTests() : base() { }

        [Test]
        public async Task Add_Doctor_Should_Be_Ok_And_Returns_Doctor_Id()
        {
            //Arrange

            //Act
            DoctorRequest doctorRequest = new DoctorRequest()
            {
                FullName = "Petr Ivanov Test",
                //OrganizationId = 1,
                Phone = "998913394353",
                Comment = "test"
            };

            HttpContent httpContent = JsonContent.Create(doctorRequest);

            HttpResponseMessage responseMessage = await HttpClient.PostAsync("doctors/add", httpContent);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
            Assert.Greater(await responseMessage.Content.ReadFromJsonAsync<int>(), 0);
        }

        [Test]
        public async Task Get_All_Doctors_Should_Be_Ok()
        {
            //Arrange

            //Act
            HttpResponseMessage httpResponseMessage = await HttpClient.GetAsync("doctors/all");

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, httpResponseMessage.StatusCode);
        }
    }
}
