using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Platform.Authorization.Users;
using Platform.Events.Dtos;
using Platform.Packages.Dtos;

namespace Platform.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserDto : EntityDto<long>
    {
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

        public ICollection<OrderDto> Orders{get;set;}
        //public ICollection<UserTestDto> UserProfession { get; set; }
        //public ICollection<UserEventsDto> UserEvents { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public string FullName { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public DateTime CreationTime { get; set; }

        public string[] RoleNames { get; set; }
    }
}
