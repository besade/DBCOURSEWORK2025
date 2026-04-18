using MediatR;
using Shop.Application.DTOs;
using Shop.Presentation.RequestDTOs;

namespace Shop.Application.Commands.Account.Register
{
    public record RegisterCommand(CustomerRegisterRequestDto Dto) : IRequest<AuthResponseDto>;
}
