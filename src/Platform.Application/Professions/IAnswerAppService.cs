using Platform.Professions.Dtos.Answer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Professions
{
    public interface IAnswerAppService
    {
        //Task CreateAnswer(CreateAnswerDto input);
        Task<AnswerReplyOkDto> UpdateAnswer(UpdateAnswerDto input);
        Task<AnswerReplyOkDto> DeleteAnswer(long id);
        Task AddTranslation(CreateAnswerTranslationDto input, long id);
        Task UpdateTranslation(UpdateAnswerTranslationDto input);
        Task DeleteTranslation(DeleteAnswerTranslationDto input);
       // Task<GetAnswerDto> GetAnswer(long id);
        Task<GetAnswerAllDto> GetAnswerAll(long id);
    }
}
