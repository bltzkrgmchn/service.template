using System.Collections.Generic;

namespace Service.Template.Core
{

    public interface IPlaceholderService
    {
        Placeholder Get(string name);

        List<Placeholder> Get();
    }
}
