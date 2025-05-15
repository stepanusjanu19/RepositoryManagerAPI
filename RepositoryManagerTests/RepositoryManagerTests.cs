using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RepositoryManagerAPI;
using Xunit;

namespace RepositoryManagerTests
{
    public class RepositoryApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public RepositoryApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Register_ValidJson_ReturnsOk_AndCanBeRetrieved()
        {
            var data = new
            {
                ItemName = "enhancedTestItem",
                ItemContent = "{\"key\":\"value\"}",
                ItemType = 1
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/RepositoryManager/register", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var retrieveResponse = await _client.GetAsync($"/api/RepositoryManager/retrieve?itemName={data.ItemName}");
            var responseContent = await retrieveResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, retrieveResponse.StatusCode);
            Assert.Contains("key", responseContent);
        }

        [Fact]
        public async Task Register_InvalidJson_ReturnsBadRequest()
        {
            var invalidJson = "{ ItemName: 'bad', ItemContent: { key: no_quotes }, ItemType: 1 }";
            var content = new StringContent(invalidJson, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/RepositoryManager/register", content);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Register_MissingFields_ReturnsBadRequest()
        {
            var data = new
            {
                ItemContent = "{\"key\":\"value\"}",
                ItemType = 1
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/RepositoryManager/register", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Register_InvalidItemType_ReturnsBadRequest()
        {
            var data = new
            {
                ItemName = "invalidItemTypeTest",
                ItemContent = "{\"key\":\"value\"}",
                ItemType = 999
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/RepositoryManager/register", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Retrieve_ExistingItem_ReturnsContent()
        {
            var data = new
            {
                ItemName = "retrieveTestItem",
                ItemContent = "{\"hello\":\"world\"}",
                ItemType = 1
            };

            var registerContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var registerResponse = await _client.PostAsync("/api/RepositoryManager/register", registerContent);
            Assert.Equal(HttpStatusCode.OK, registerResponse.StatusCode);

            var retrieveResponse = await _client.GetAsync($"/api/RepositoryManager/retrieve?itemName={data.ItemName}");
            var responseString = await retrieveResponse.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, retrieveResponse.StatusCode);
            Assert.Contains("hello", responseString);
        }

        [Fact]
        public async Task Retrieve_NonExistingItem_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/RepositoryManager/retrieve?itemName=nonexistent");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Deregister_Item_RemovesItem()
        {
            var itemName = "deleteTestItem";
            var data = new
            {
                ItemName = itemName,
                ItemContent = "{\"delete\":\"me\"}",
                ItemType = 1
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var registerResponse = await _client.PostAsync("/api/RepositoryManager/register", content);
            Assert.Equal(HttpStatusCode.OK, registerResponse.StatusCode);

            var deleteResponse = await _client.DeleteAsync($"/api/RepositoryManager/deregister?itemName={itemName}");
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

            var retrieveResponse = await _client.GetAsync($"/api/RepositoryManager/retrieve?itemName={itemName}");
            Assert.Equal(HttpStatusCode.NotFound, retrieveResponse.StatusCode);
        }

        [Fact]
        public async Task Deregister_NonExistingItem_ReturnsNotFound()
        {
            var response = await _client.DeleteAsync("/api/RepositoryManager/deregister?itemName=ghostItem");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
