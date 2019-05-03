using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(AnswerContent))]
    public class AnswerContentDto : GenericContentDto<AnswerContent, long>
    {
     
    }
}
