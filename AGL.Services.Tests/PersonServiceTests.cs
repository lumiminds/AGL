using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AGL.Models;
using AGL.Services.Contracts;
using AGL.Services.DTOs;
using AGL.Services.HelperClasses;
using AGL.Services.Implementation;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;
using Xunit;

namespace AGL.Services.Tests
{
    public class PeopleServiceTests
    {
        readonly string clientName = "PeopleList";

        private string GetPeopleJsonData() {
            string jsonData = "[{\"name1\":\"Bob\",\"gender\":\"Male\",\"age\":23,\"pets\":[{\"name\":\"Garfield\",\"type\":\"Cat\"},{\"name\":\"Fido\",\"type\":\"Dog\"}]},{\"name\":\"Jennifer\",\"gender\":\"Female\",\"age\":18,\"pets\":[{\"name\":\"Garfield\",\"type\":\"Cat\"}]},{\"name\":\"Steve\",\"gender\":\"Male\",\"age\":45,\"pets\":null},{\"name\":\"Fred\",\"gender\":\"Male\",\"age\":40,\"pets\":[{\"name\":\"Tom\",\"type\":\"Cat\"},{\"name\":\"Max\",\"type\":\"Cat\"},{\"name\":\"Sam\",\"type\":\"Dog\"},{\"name\":\"Jim\",\"type\":\"Cat\"}]},{\"name\":\"Samantha\",\"gender\":\"Female\",\"age\":40,\"pets\":[{\"name\":\"Tabby\",\"type\":\"Cat\"}]},{\"name\":\"Alice\",\"gender\":\"Female\",\"age\":64,\"pets\":[{\"name\":\"Simba\",\"type\":\"Cat\"},{\"name\":\"Nemo\",\"type\":\"Fish\"}]}]";
            return jsonData;
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
                    throw new Exception(string.Empty);
                default:
                    break;
            }

            return peopleData;
        }

        private List<People> RetrieveDeserializePeopleListAsync(string petType)
        {
            List<People> peopleData = new List<People>();
            peopleData.Add(new People()
            {
                Name = "Bob",
                Gender = "Male",
                Age = 23,
                Pets = new List<Pet>() {
                    new Pet() { Name = "Garfield", Type = "Cat" },
                    new Pet() { Name = "Fido", Type = "Dog" }
                }
            });
            peopleData.Add(new People()
            {
                Name = "Jennifer",
                Gender = "Female",
                Age = 18,
                Pets = new List<Pet>() {
                    new Pet() { Name = "Garfield", Type = "Cat" }
                }
            });

            peopleData.Add(new People()
            {
                Name = "Steve",
                Gender = "Male",
                Age = 45,
                Pets = null
            });

            peopleData.Add(new People()
            {
                Name = "Fred",
                Gender = "Male",
                Age = 40,
                Pets = new List<Pet>() {
                    new Pet() { Name = "Tom", Type = "Cat" },
                    new Pet() { Name = "Max", Type = "Cat" },
                    new Pet() { Name = "Sam", Type = "Dog" },
                    new Pet() { Name = "Jim", Type = "Cat" }
                }
            });

            peopleData.Add(new People()
            {
                Name = "Samantha",
                Gender = "Female",
                Age = 40,
                Pets = new List<Pet>() {
                    new Pet() { Name = "Tabby", Type = "Cat" }
                }
            });

            peopleData.Add(new People()
            {
                Name = "Alice",
                Gender = "Female",
                Age = 64,
                Pets = new List<Pet>() {
                    new Pet() { Name = "Simba", Type = "Cat" },
                    new Pet() { Name = "Nemo", Type = "Fish" }
                }
            });

            return peopleData;
        }

        [Theory(DisplayName = "Retrieve JsonData And Filter With PetType")]
        [InlineData("cat")]
        [InlineData("dog")]
        [InlineData("fish")]
        public async Task RetrieveJsonDataAndFilterWithPetType(string petType)
        {
            // Arrange
            var myProfile = new AutoMapperConfigHelper();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var mockLogger = new Mock<ILogger<PeopleService>>(); 
            var mockHttpService = new Mock<IHttpService>();
            mockHttpService.Setup(service => service.RetrieveJsonDataAsync<List<People>>(clientName))
                .ReturnsAsync(RetrieveDeserializePeopleListAsync(petType));

            var peopleService = new PeopleService(mapper, mockLogger.Object, mockHttpService.Object);

            // Act
            var expectedResult = GetPetsInAlphabetOrderGroupByWithGender(petType);
            var actualResult = (await peopleService.RetrievePeopleListAsync(petType));

            // Assert
            Assert.Equal(expectedResult.Count(), actualResult.Count());

            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.Equal(expectedResult[i].Gender, actualResult[i].Gender);

                for (int j = 0; j < expectedResult[i].Pets.Count; j++)
                {
                    Assert.Equal(expectedResult[i].Pets[j].Name, actualResult[i].Pets[j].Name);
                }
            }
        }
    }
}
