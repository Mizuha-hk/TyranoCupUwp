using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyranoCupUwpApp.Shared.api
{
    public interface IOpenAIFormation
    {
        Task GetOpenAIApiKey();
        Task<string> FormatTextToJson(string text);
    }
}
