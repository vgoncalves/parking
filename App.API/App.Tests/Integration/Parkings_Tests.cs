using App.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace App.Tests.Integration
{
    public class Parkings_Tests
    {
        private readonly HttpClient _client;
        private readonly CreateParking.Request _validRequest;
        private readonly AppDbContext _db;

        public Parkings_Tests()
        {
            var app = new App();

            _db = app.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
            _client = app.CreateClient();

            _validRequest = new CreateParking.Request("Main Parking", new CreateParking.Request.AddressType(
                    "14021-644",
                    "SP",
                    "Ribeirão Preto",
                    "Magid Antônio Calil",
                    "66",
                    "apt.22"));
        }

        [Fact]
        public async Task Create_Parking_Returns_Created201_When_Valid_Request()
        {
            var response = await _client.PostAsJsonAsync("/parkings", _validRequest);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Created);
        }

        [Fact]
        public async Task Create_Parking_Returns_Created_Id_When_Valid_Request()
        {
            var response = await _client.PostAsJsonAsync("/parkings", _validRequest);

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
        public async Task Create_Parking_Saves_Parking_When_Valid_Request()
        {
            await _client.PostAsJsonAsync("/parkings", _validRequest);

            var recordCount = await _db.Parkings.CountAsync();

            Assert.Equal(1, recordCount);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task Create_Parking_Validates_Required_Fields(string emptyValue)
        {
            var request = new CreateParking.Request(emptyValue,
                new CreateParking.Request.AddressType(
                    emptyValue,
                    emptyValue,
                    emptyValue,
                    emptyValue,
                    emptyValue,
                    emptyValue));

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
    }
}