using System.Collections.Generic;

namespace Facade.Template.Instance.Gateways
{
    public interface IPlaceholderGateway
    {
        List<Placeholder> Find();
        Placeholder Find(string name);
    }
}