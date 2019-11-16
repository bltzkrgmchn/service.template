using System.Collections.Generic;

namespace Service.Template.Instance.Gateways
{
    public interface IPlaceholderGateway
    {
        List<Placeholder> Find();
        Placeholder Find(string name);
    }
}