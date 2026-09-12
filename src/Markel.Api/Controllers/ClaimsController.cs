using Markel.Api.Contracts;
using Markel.Application.Claims.GetClaimByUcr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Markel.Api.Controllers;

[ApiController]
[Route("api/claims")]
public sealed class ClaimsController(IMediator mediator) : ControllerBase
{
    [HttpGet("{ucr}")]
    public async Task<IActionResult> GetByUcr(string ucr, CancellationToken cancellationToken)
    {
        var claim = await mediator.Send(new GetClaimByUcrQuery(ucr), cancellationToken);
        return Ok(claim);
    }

    [HttpPut("{ucr}")]
    public async Task<IActionResult> Update(string ucr, [FromBody] UpdateClaimRequest request, CancellationToken cancellationToken)
    {
        var claim = await mediator.Send(request.ToCommand(ucr), cancellationToken);
        return Ok(claim);
    }
}
