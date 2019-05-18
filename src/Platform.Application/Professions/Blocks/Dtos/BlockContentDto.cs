using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Platform.Professions.Blocks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Platform.Professions.Dtos
{
    [AutoMap(typeof(BlockContent))]
    public class BlockContentDto : GenericContentDto<BlockContent, long>
    {
    }
    
    [AutoMap(typeof(BlockContent))]
    public class BlockContentUpdateDto : GenericContentUpdateDto<BlockContent, long>
    {
    }
}
