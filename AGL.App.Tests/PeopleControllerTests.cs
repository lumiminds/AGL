using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AGL.App.Controllers;
using AGL.Models;
using AGL.Services.Contracts;
using AGL.Services.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace AGL.App.Tests
{
    public class PeopleControllerTests
    {
        readonly string errorMessage = "Unable to retrieve data.";

        #region snippet_Get_ReturnsAOkObjectResult_WithAListOfPets

        [Theory(DisplayName = "Get Pets In Alphabet Order Group By With Gender")]
        [InlineData("cat", 4, 3)]
        [InlineData("dog", 2, 0)]
        [InlineData("fish", 0, 1)]
        public async Task Get_ReturnsAOkObjectResult_WithAListOfPets(string petType, int noOfPetswithMale, int noOfPetswithFemale)
        {
            // Arrange
            var mockService = new Mock<IPeopleService>();
            mockService.Setup(service => service.RetrievePeopleListAsync(petType))
                .ReturnsAsync(GetPetsInAlphabetOrderGroupByWithGender(petType));
            var controller = new PeopleController(null, mockService.Object);

            // Act
            var result = await controller.Get(petType);

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<PeopleDTO>>(viewResult.Value);
            
            // Result must have 2 items each one for Male/Female
            Assert.Equal(2, model.Count());

            // Result should match the pet count for Male
            var petCountWithMale = model.Where(c => c.Gender.Equals("male", StringComparison.OrdinalIgnoreCase)).FirstOrDefault()?.Pets.Count;
            Assert.Equal(noOfPetswithMale, petCountWithMale);

            // Result should match the pet count for Female
            var petCountWithFemale = model.Where(c => c.Gender.Equals("female", StringComparison.OrdinalIgnoreCase)).FirstOrDefault()?.Pets.Count;
            Assert.Equal(noOfPetswithFemale, petCountWithFemale);
        }

        private List<PeopleDTO> GetPetsInAlphabetOrderGroupByWithGender(string petType)
        {
            List<PeopleDTO> peopleData = new List<PeopleDTO>();
            switch (petType)
            {
                case "cat":
                    peopleData.Add(new PeopleDTO()
                    {
                        Gender = "Male",
                        Pets = new List<PetDTO>() {
                            new PetDTO() { Name = "Garfield" },
                            new PetDTO() { Name = "Jim" },
                            new PetDTO() { Name = "Max" },
                            new PetDTO() { Name = "Tom" }
                        }
                    });
                    peopleData.Add(new PeopleDTO()
                    {
                        Gender = "Female",
                        Pets = new List<PetDTO>() {
                            new PetDTO() { Name = "Garfield" },
                            new PetDTO() { Name = "Simba" },
                            new PetDTO() { Name = "Tabby" }
                        }
                    });
                    break;
                case "dog":
                    peopleData.Add(new PeopleDTO()
                    {
                        Gender = "Male",
                        Pets = new List<PetDTO>() {
                            new PetDTO() { Name = "Fido" },
                            new PetDTO() { Name = "Sam" }
                        }
                    });
                    peopleData.Add(new PeopleDTO() { Gender = "Female", Pets = new List<PetDTO>() { } });
                    break;
                case "fish":
                    peopleData.Add(new PeopleDTO() { Gender = "Male", Pets = new List<PetDTO>() { } });
                    peopleData.Add(new PeopleDTO()
                    {
                        Gender = "Female",
                        Pets = new List<PetDTO>() {
                            new PetDTO() { Name = "Nemo" }
                        }
                    });
                    break;
                case "monkey":
                    throw new Exception(errorMessage);
                default:
                    break;
            }

            return peopleData;
        }
        #endregion


        #region BadRequest Error Test Scenarios

        [Theory(DisplayName = "Return Bad Request error when petType is empty or null")]
        [InlineData("")]
        [InlineData(null)]
        public async Task Get_ReturnsBadRequestResult_WhenPetTypeIsEmptyOrNull(string petType)
        {
            // Arrange
            var mockService = new Mock<IPeopleService>();
            mockService.Setup(service => service.RetrievePeopleListAsync(petType))
                .ReturnsAsync(GetPetsInAlphabetOrderGroupByWithGender(petType));
            var controller = new PeopleController(null, mockService.Object);
            
            // Act
            var result = await controller.Get(petType);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(badRequestResult.StatusCode, StatusCodes.Status400BadRequest);
        }

        #endregion

        #region InternalServerError Test Scenarios

        [Theory(DisplayName = "Unexpected Internal Server error")]
        [InlineData("monkey")]
        public async Task Get_InternalServerError(string petType)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PeopleController>>();
            var mockService = new Mock<IPeopleService>();
            mockService.Setup(service => service.RetrievePeopleListAsync(petType)).Throws(new Exception(errorMessage));
                
            var controller = new PeopleController(mockLogger.Object, mockService.Object);

            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => controller.Get(petType));

            // Assert
            Assert.Equal(errorMessage, ex.Message);
        }

        #endregion
    }
}
