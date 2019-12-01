using Service.Template.Core;
using System.Collections.Generic;

namespace Service.Template.Data
{
    public interface IPlaceholderRepository
    {
        List<Placeholder> Find();
        Placeholder Find(string name);
    }
}