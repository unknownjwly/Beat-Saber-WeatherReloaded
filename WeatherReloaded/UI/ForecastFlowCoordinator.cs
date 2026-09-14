using System;
using BeatSaberMarkupLanguage;
using HMUI;

namespace WeatherReloaded.UI
{
    internal class ForecastFlowCoordinator : FlowCoordinator
    {
        private Forecast forecastViewController = null!;
        internal EffectSettings EffectViewController = null!;

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            forecastViewController = BeatSaberUI.CreateViewController<Forecast>();
            EffectViewController = BeatSaberUI.CreateViewController<EffectSettings>();

            try
            {
                if (!firstActivation) return;

                SetTitle("Forecast");
                showBackButton = true;
                forecastViewController.flow = this;
                ProvideInitialViewControllers(forecastViewController);
            }
            catch (Exception ex)
            {
                Plugin.Log?.Error(ex);
            }
        }

        protected override void BackButtonWasPressed(ViewController _)
        {
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
        }

        public void ShowEffectSettings()
        {
            SetRightScreenViewController(EffectViewController, ViewController.AnimationType.In);
        }
    }
}