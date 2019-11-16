using Nancy;
using Nancy.TinyIoc;
using Service.Template.Instance.Gateways;

namespace Service.Template.Instance
{
    public class Bootstraper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register<IPlaceholderGateway, PlaceholderGateway>().AsSingleton();

            base.ConfigureApplicationContainer(container);
        }
    }
}