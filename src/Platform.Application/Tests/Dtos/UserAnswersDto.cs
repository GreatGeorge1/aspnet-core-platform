using System;
using System.Collections.Generic;
using System.Text;

namespace Platform.Tests.Dtos
{
    public class UserAnswersDto
    {
        public long ProfessionId { get; set; }
        public ICollection<BlockAnswers> BlockAnswers {get;set;}
    }

    public class BlockAnswers {
        public long BlockId { get; set; }
        public ICollection<UserTestResponseDto> UserTests{ get;set;}
    }
}
