namespace Service.Template.Core
{
    public class Placeholder
    {
        public Placeholder(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}