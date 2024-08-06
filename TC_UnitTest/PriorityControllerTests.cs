using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

public class PriorityControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Startup> _factory;

    public PriorityControllerTests(WebApplicationFactory<Startup> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenPriorityExistsAndIsDeleted()
    {
        var client = _factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Delete, "/api/priority/1");

        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<dynamic>(content);
        Assert.Equal("Приоритет с ID 1 успешно удалён.", result.message);
    }
}