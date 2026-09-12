using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Markel.Application.Companies;

namespace Markel.Api.Tests;

public sealed class CompaniesApiTests(MarkelWebApplicationFactory factory) : IClassFixture<MarkelWebApplicationFactory>
{
    private const string ApiKey = "tech-test-local-key";

    [Fact]
    public async Task Get_company_without_api_key_returns_401()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/api/companies/1");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_company_with_wrong_api_key_returns_401()
    {
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Api-Key", "wrong-key");

        var response = await client.GetAsync("/api/companies/1");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_company_with_valid_api_key_returns_200()
    {
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Api-Key", ApiKey);

        var response = await client.GetAsync("/api/companies/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var company = await response.Content.ReadFromJsonAsync<CompanyDto>();
        company.Should().NotBeNull();
        company!.Id.Should().Be(1);
        company.HasActiveInsurancePolicy.Should().BeTrue();
    }
}
