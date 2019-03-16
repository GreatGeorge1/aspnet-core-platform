using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Platform.Roles.Dto;
using Platform.Users.Dto;

namespace Platform.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);
    }
}
