using MediatR;
using Shop.Application.DTOs;
using Shop.Presentation.RequestDTOs;

namespace Shop.Application.Commands.Account.Login
{
    public record LoginCommand(CustomerLoginRequestDto Dto) : IRequest<AuthResponseDto>;
}
