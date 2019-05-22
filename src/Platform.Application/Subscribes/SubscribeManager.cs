using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Platform.Authorization.Users;
using Platform.Professions;
using Platform.Professions.User;

namespace Platform.Subscribes
{
    public class SubscribeManager:DomainService,ISubscribeManager
    {
        [NotNull] private readonly IRepository<User, long> _userRepository;
        [NotNull] private readonly IRepository<Profession, long> _professionRepository;
        [NotNull] private readonly IRepository<UserProfessions, long> _userProfessionsRepository;

        public SubscribeManager([NotNull] IRepository<User, long> userRepository,
            [NotNull] IRepository<Profession, long> professionRepository,
            [NotNull] IRepository<UserProfessions, long> userProfessionsRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _professionRepository =
                professionRepository ?? throw new ArgumentNullException(nameof(professionRepository));
            _userProfessionsRepository = userProfessionsRepository ??
                                         throw new ArgumentNullException(nameof(userProfessionsRepository));
        }

        [UnitOfWork]
        public async Task<bool> SubscribeToProfession(long userid, long professionid)
        {
            var user = await _userRepository.GetAll()
                .Include(u => u.UserProfessions)
                    .ThenInclude(up => up.Profession)
                .FirstOrDefaultAsync(u=>u.Id==userid)??throw new EntityNotFoundException(typeof(User), userid);
            var profession = await _professionRepository.GetAll().Include(p => p.UserProfessions)
                                 .FirstOrDefaultAsync(p => p.Id == professionid) ??
                             throw new EntityNotFoundException(typeof(Profession), professionid);
            if (user.UserProfessions == null || !user.UserProfessions.Any())
            {
                using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    var userprofession = await _userProfessionsRepository
                        .GetAll().Include(up => up.User)
                        .Include(up => up.Profession)
                        .Where(up=> up.ProfessionId==profession.Id)
                        .Where(up=>up.UserId==user.Id)
                        .FirstOrDefaultAsync();
                    if (userprofession != null)
                    {
                        userprofession.UnDelete();
                        return true;
                    }
                    
                }
                user.UserProfessions=new List<UserProfessions>();
                var up1=new UserProfessions()
                {
                    User=user,
                    Profession = profession,
                    UserTests = new List<UserTests>(),
                    UserSeenSteps = new List<UserSeenSteps>(),
                    IsCompleted = false
                };
                user.UserProfessions.Add(up1);
            }
            else
            {
                var userprofession = await _userProfessionsRepository
                    .GetAll().Include(up => up.User)
                    .Include(up => up.Profession)
                    .Where(up=> up.ProfessionId==profession.Id)
                    .Where(up=>up.UserId==user.Id)
                    .FirstOrDefaultAsync();
                if (userprofession == null)
                {
                    var up=new UserProfessions()
                    {
                        User=user,
                        Profession = profession,
                        UserTests = new List<UserTests>(),
                        UserSeenSteps = new List<UserSeenSteps>(),
                        IsCompleted = false
                    };
                    user.UserProfessions.Add(up);
                }
            }
            return true;
        }
        [UnitOfWork]
        public async Task<bool> UnsubscribeToProfession(long userid, long professionid)
        {
            var user = await _userRepository.GetAll()
                           .Include(u => u.UserProfessions)
                           .ThenInclude(up => up.Profession)
                           .FirstOrDefaultAsync(u=>u.Id==userid)??throw new EntityNotFoundException(typeof(User), userid);
            var profession = await _professionRepository.GetAll().Include(p => p.UserProfessions)
                                 .FirstOrDefaultAsync(p => p.Id == professionid) ??
                             throw new EntityNotFoundException(typeof(Profession), professionid);
            if (user.UserProfessions == null || !user.UserProfessions.Any())
            {
                return true;
            }
            var userprofession = await _userProfessionsRepository
                .GetAllIncluding(up => up.User, up => up.Profession)
                .Where(up=> up.ProfessionId==profession.Id)
                .Where(up=>up.UserId==user.Id)
                .FirstOrDefaultAsync();
            if (userprofession != null)
            {
                await _userProfessionsRepository.DeleteAsync(userprofession);
            }
            return true;
        }
        [UnitOfWork]
        public async Task<ICollection<UserProfessions>> GetSubscriptions(long userid)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u=>u.Id==userid)??throw new EntityNotFoundException(typeof(User), userid);
            var ups = await _userProfessionsRepository.GetAll()
              //  .Include(up => up.User)
                .Include(up => up.Profession)
                    .ThenInclude(p=>p.Content)
                .Include(p=>p.Profession)
                    .ThenInclude(p=>p.Author)
                .Include(up => up.UserTests)
              .Where(up=>up.UserId==user.Id)
                .ToListAsync();
            return ups;
        }
        [UnitOfWork]
        public async Task<bool> UserIsSubscribed(long userid, long professionid)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u=>u.Id==userid)??throw new EntityNotFoundException(typeof(User), userid);
            var profession = await _professionRepository.FirstOrDefaultAsync(p => p.Id == professionid) ??
                             throw new EntityNotFoundException(typeof(Profession), professionid);
            var userprofession = await _userProfessionsRepository
                .GetAllIncluding(up => up.User, up => up.Profession)
                .Where(up=> up.ProfessionId==profession.Id)
                .Where(up=>up.UserId==user.Id)
                .FirstOrDefaultAsync();
            if (userprofession != null)
            {
                return true;
            }

            return false;
        }
    }
}