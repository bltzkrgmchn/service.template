using System.Collections.Generic;

namespace Service.Template.Core
{
    public interface IPlaceholderGateway
    {
        List<Placeholder> Find();
        Placeholder Find(string name);
    }
}