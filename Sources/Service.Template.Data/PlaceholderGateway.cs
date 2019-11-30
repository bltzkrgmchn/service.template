using Service.Template.Core;
using System.Collections.Generic;

namespace Service.Template.Data
{
    public class PlaceholderGateway : IPlaceholderGateway
    {
        public List<Placeholder> Find()
        {
            return new List<Placeholder> { new Placeholder("pen"), new Placeholder("apple"), new Placeholder("pineapple") };
        }

        public Placeholder Find(string name)
        {
            return new Placeholder($"{name}-placeholder");
        }
    }
}