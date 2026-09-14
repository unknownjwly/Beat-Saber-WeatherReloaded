using System;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using Zenject;

namespace WeatherReloaded.UI
{
    public class WeatherMenuManager : IInitializable, IDisposable
    {
        private MenuButton? _menuButton;
        private ForecastFlowCoordinator? _flowCoordinator;

        public void Initialize()
        {
            _menuButton = new MenuButton("Forecast", "See your Weather", ShowFlowCoordinator);
            MenuButtons.Instance.RegisterButton(_menuButton);
        }

        public void Dispose()
        {
            if (_menuButton != null && MenuButtons.Instance != null)
            {
                MenuButtons.Instance.UnregisterButton(_menuButton);
            }
        }

        private void ShowFlowCoordinator()
        {
            if (_flowCoordinator == null)
            {
                _flowCoordinator = BeatSaberUI.CreateFlowCoordinator<ForecastFlowCoordinator>();
            }

            BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(_flowCoordinator);
        }
    }
}