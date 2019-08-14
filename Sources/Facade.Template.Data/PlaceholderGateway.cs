using Facade.Scaffold.Instance.Gateways;
using System.Collections.Generic;

namespace Facade.Scaffold.Data.Gateways
{
    public class PlaceholderGateway : IPlaceholderGateway
    {
        public List<Placeholder> Find()
        {
            throw new System.NotImplementedException();
        }

        public Placeholder Find(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}