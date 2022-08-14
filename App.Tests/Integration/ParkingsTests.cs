using App.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace App.Tests.Integration
{
    public class ParkingsTests
    {
        private readonly HttpClient _client;
        private readonly CreateParking.Request _validCreateRequest;
        private readonly AppDbContext _db;

        public ParkingsTests()
        {
            var app = new App();

            _db = app.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
            _client = app.CreateClient();

            _validCreateRequest = new CreateParking.Request()
            {
                Name = "Main Parking",
                Address = new CreateParking.Request.AddressType()
                {
                    PostalCode = "14021-644",
                    City = "Ribeirão Preto",
                    Complement = "Apt. 22",
                    Number = "66",
                    State = "SP",
                    Street = "Magid Ant�nio Calil"
                }
            };
        }

        [Fact]
        public async Task Create_Parking_Returns_Created201()
        {
            var response = await _client.PostAsJsonAsync("/parkings", _validCreateRequest);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public async Task Create_Parking_Returns_Parking_Id()
        {
            var response = await _client.PostAsJsonAsync("/parkings", _validCreateRequest);

            string? createdId = null;

            if (response.IsSuccessStatusCode)
            {
                var payload = await response.Content.ReadFromJsonAsync<CreateParking.Response>();
                createdId = payload?.Id;
            }

            var createdParking = await _db.Parkings.FirstOrDefaultAsync();

            Assert.Equal(createdParking?.Id, createdId);
        }

        [Fact]
        public async Task Create_Parking_Saves_Parking()
        {
            await _client.PostAsJsonAsync("/parkings", _validCreateRequest);

            var recordCount = await _db.Parkings.CountAsync();

            Assert.Equal(1, recordCount);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task Create_Parking_Validates_Required_Fields(string emptyValue)
        {
            var request = new CreateParking.Request()
            {
                Name = emptyValue,
                Address = new CreateParking.Request.AddressType()
                {
                    City = emptyValue,
                    Complement = emptyValue,
                    Number = emptyValue,
                    PostalCode = emptyValue,
                    State = emptyValue,
                    Street = emptyValue
                }
            };

            var response = await _client.PostAsJsonAsync("/parkings", request);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);

            var payload = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();

            Assert.Collection(payload,
                x => Assert.Equal("'Name' must not be empty.", x),
                x => Assert.Equal("'Address Number' must not be empty.", x),
                x => Assert.Equal("'Address City' must not be empty.", x),
                x => Assert.Equal("'Address Postal Code' must not be empty.", x),
                x => Assert.Equal("'Address Complement' must not be empty.", x),
                x => Assert.Equal("'Address State' must not be empty.", x),
                x => Assert.Equal("'Address Street' must not be empty.", x)
                );
        }

        //[Fact]
        //public async Task Update_Parking_Returns_NoContent204()
        //{
        //    var parkingId = await CreateParking();

        //    var response = await _client.PutAsJsonAsync($"/parkings/{parkingId}", _validCreateRequest);

        //    Assert.True(response.StatusCode == System.Net.HttpStatusCode.Created);
        //}

        //async Task<string> CreateParking()
        //{
        //    var response = await _client.PostAsJsonAsync("/parkings", _validCreateRequest);
        //    response.EnsureSuccessStatusCode();

        //    var parking = await response.Content.ReadFromJsonAsync<CreateParking.Response>();

        //    return parking!.Id;
        //}
    }
}