using Markel.Application.Claims.GetClaimsByCompanyId;
using Markel.Application.Companies.GetCompanies;
using Markel.Application.Companies.GetCompanyById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Markel.Api.Controllers;

[ApiController]
[Route("api/companies")]
public sealed class CompaniesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCompanies(CancellationToken cancellationToken)
    {
        var companies = await mediator.Send(new GetCompaniesQuery(), cancellationToken);
        return Ok(companies);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCompanyById(int id, CancellationToken cancellationToken)
    {
        var company = await mediator.Send(new GetCompanyByIdQuery(id), cancellationToken);
        return Ok(company);
    }

    [HttpGet("{id:int}/claims")]
    public async Task<IActionResult> GetClaimsByCompanyId(int id, CancellationToken cancellationToken)
    {
        var claims = await mediator.Send(new GetClaimsByCompanyIdQuery(id), cancellationToken);
        return Ok(claims);
    }
}
