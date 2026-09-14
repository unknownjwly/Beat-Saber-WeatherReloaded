using System.ComponentModel;
using Zenject;
using WeatherReloaded.UI;

namespace WeatherReloaded.Installers
{
    internal class WeatherMenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<WeatherMenuManager>().AsSingle();
        }
    }
}