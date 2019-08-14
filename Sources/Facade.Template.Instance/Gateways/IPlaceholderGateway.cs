using System.Collections.Generic;

namespace Facade.Scaffold.Instance.Gateways
{
    public interface IPlaceholderGateway
    {
        List<Placeholder> Find();
        Placeholder Find(string name);
    }
}