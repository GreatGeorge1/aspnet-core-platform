using System.Threading.Tasks;
using Platform.Configuration.Dto;

namespace Platform.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
