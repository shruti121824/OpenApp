using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Satrabel.OpenApp.Authorization.Users;
using System.ComponentModel;
using NJsonSchema.Annotations;

namespace Satrabel.OpenApp.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserDto : EntityDto<long>
    {
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]        
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        [JsonSchemaExtensionData("x-ui-grid", false)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        [JsonSchemaExtensionData("x-ui-grid", false)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        [ReadOnly(true)]
        public string FullName { get; set; }

        [ReadOnly(true)]
        [JsonSchemaExtensionData("x-ui-grid", false)]
        public DateTime? LastLoginTime { get; set; }

        [ReadOnly(true)]
        [JsonSchemaExtensionData("x-ui-grid", false)]
        public DateTime CreationTime { get; set; }

        [JsonSchemaExtensionData("x-ui-grid", false)]
       
        public string[] RoleNames { get; set; }
    }
}