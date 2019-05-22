using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.BackgroundJobs;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platform.Authorization;
using Platform.Authorization.Accounts;
using Platform.Authorization.Roles;
using Platform.Authorization.Users;
using Platform.Background;
using Platform.Roles.Dto;
using Platform.Users.Dto;

namespace Platform.Users
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;
        private readonly IBackgroundJobManager _backgroundJobManager;

        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager,
            IBackgroundJobManager backgroundJobManager)
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _backgroundJobManager = backgroundJobManager;
        }

        [AbpAllowAnonymous]
        public override async Task<UserDto> Create(CreateUserDto input)
        {
            var user = ObjectMapper.Map<User>(input);
            var tenantId = 1;
            //user.TenantId = AbpSession.TenantId;
            user.TenantId = tenantId;
            user.IsEmailConfirmed = true;
            await _userManager.InitializeOptionsAsync(tenantId);

            CheckErrors(await _userManager.CreateAsync(user, input.Password));
            user.Roles=new List<UserRole>();
            if (PermissionChecker.IsGranted(PermissionNames.Pages_Users))
            {
                if (input.RoleNames != null)
                {
                    CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
                }
            }
            
            CurrentUnitOfWork.SaveChanges();

            _ = await _backgroundJobManager.EnqueueAsync<SendEMailJob, SendEmailArgs>(
                new SendEmailArgs
                {
                    Email = user.EmailAddress,
                    Subject = $"Реєстрація Choizy.org",
                    isHtml = true,
                    Message = $@"Вітаємо з реєстрацією на профорієнтаційній платформі ChoiZY!<br><br>
                                Обирати професію з нами легко та швидко!<br><br>
                                Виникли питання? Звертайтесь до нас: <a href = 'mailto: info@choizy.org'>info@choizy.org</a><br>"
                });
            
            return MapToEntityDto(user);
        }

        public override async Task<UserDto> Update(UserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);

            MapToEntity(input, user);

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
            }

            return await Get(input);
        }

        public override async Task Delete(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roles = _roleManager.Roles.Where(r => user.Roles.Any(ur => ur.RoleId == r.Id)).Select(r => r.NormalizedName);
            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();
            return userDto;
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attemping to change password.");
            }
            long userId = _abpSession.UserId.Value;
            var user = await _userManager.GetUserByIdAsync(userId);
            var loginAsync = await _logInManager.LoginAsync(user.UserName, input.CurrentPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Existing Password' did not match the one on record.  Please try again or contact an administrator for assistance in resetting your password.");
            }
            if (!new Regex(AccountAppService.PasswordRegex).IsMatch(input.NewPassword))
            {
                throw new UserFriendlyException("Passwords must be at least 8 characters, contain a lowercase, uppercase, and number.");
            }
            user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
            CurrentUnitOfWork.SaveChanges();
            return true;
        }

        [AbpAllowAnonymous]
        public async Task<bool> RecoverPassword(RecoverPasswordDto input)
        {
            var token = input.ResetCode;
            var user = await _userManager.GetUserByIdAsync(input.UserId)??throw new UserFriendlyException($"Такий користувач не існує");
            (await _userManager.ResetPasswordAsync(user, token.Trim(), input.NewPassword.Trim())).CheckErrors();
            
            _ = await _backgroundJobManager.EnqueueAsync<SendEMailJob, SendEmailArgs>(
                new SendEmailArgs
                {
                    Email = user.EmailAddress,
                    Subject = "Відновлення паролю - Choizy.Org",
                    isHtml = true,
                    Message = $@"Ім'я: <b>{user.Name}</b><br><br>
                               Пароль змінено<br><br>
                              "
                });
            return true;
        }

        [AbpAllowAnonymous]
        public async Task<bool> SendRecoveryCodeOnEmail(SendResetCodeDto input)
        {
            var user = await _userManager.FindByNameOrEmailAsync(input.UserNameOrEmail)??throw new UserFriendlyException($"Такий користувач не існує: {input.UserNameOrEmail}");
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = input.ResetFormUrl.Trim();
            var urlToken = WebUtility.HtmlEncode(resetToken);
            _ = await _backgroundJobManager.EnqueueAsync<SendEMailJob, SendEmailArgs>(
                new SendEmailArgs
                {
                    Email = user.EmailAddress,
                    Subject = "Відновлення паролю - Choizy.Org",
                    isHtml = true,
                    Message = $@"Ім'я: <b>{user.Name}</b><br><br>
                                <a href = '{url}/?userid={user.Id}&token={urlToken}'>Натисніть сюди, щоб перейти до відновлення паролю</a><br><br>
                                Або перейдіть за посиланням:  <a href = '{url}/?userid={user.Id}&token={urlToken}'>{url}/?userid={user.Id}&token={urlToken}</a><br><br>
                                Це посилання буде дійсне 24 години<br><br>
                              "
                });
            return true;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [AbpAllowAnonymous]
        public async Task<bool> SendRecoveryCodeOnEmail2(SendResetCodeDto input)
        {
            var user = await _userManager.FindByNameOrEmailAsync(input.UserNameOrEmail)??throw new UserFriendlyException($"Такий користувач не існує: {input.UserNameOrEmail}");
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = input.ResetFormUrl.Trim();
            var urlToken = WebUtility.HtmlEncode(resetToken);
            _ = await _backgroundJobManager.EnqueueAsync<SendEMailJob, SendEmailArgs>(
                new SendEmailArgs
                {
                    Email = user.EmailAddress,
                    Subject = "Встановлення паролю - Choizy.Org",
                    isHtml = true,
                    Message = $@"Ім'я: <b>{user.Name}</b><br><br>
                                <a href = '{url}/?userid={user.Id}&token={urlToken}'>Натисніть сюди, для встановлення паролю</a><br><br>
                                Або перейдіть по адресі:  <a href = '{url}/?userid={user.Id}&token={urlToken}'>{url}/?userid={user.Id}&token={urlToken}</a><br><br>
                                Ця адреса буде дійсна 24 години.<br><br>
                                Token: {resetToken}
                              "
                });
            return true;
        }

        
        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attemping to reset password.");
            }
            long currentUserId = _abpSession.UserId.Value;
            var currentUser = await _userManager.GetUserByIdAsync(currentUserId);
            var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
            }
            if (currentUser.IsDeleted || !currentUser.IsActive)
            {
                return false;
            }
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains(StaticRoleNames.Tenants.Admin))
            {
                throw new UserFriendlyException("Only administrators may reset passwords.");
            }

            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                CurrentUnitOfWork.SaveChanges();
            }

            return true;
        }

    }
}

