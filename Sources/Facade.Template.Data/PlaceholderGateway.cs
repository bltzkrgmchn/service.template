using Facade.Template.Instance.Gateways;
using System.Collections.Generic;

namespace Facade.Template.Data.Gateways
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