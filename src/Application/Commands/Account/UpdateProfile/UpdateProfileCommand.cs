using MediatR;
using Shop.Presentation.RequestDTOs;

namespace Shop.Application.Commands.Account.UpdateProfile
{
    public record UpdateProfileCommand(int CustomerId, UpdateProfileRequestDto Dto) : IRequest;
}
