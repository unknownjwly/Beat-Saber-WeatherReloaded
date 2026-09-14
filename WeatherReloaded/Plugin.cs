using System.Reflection;
using HarmonyLib;
using IPA;
using IPA.Config.Stores;
using JetBrains.Annotations;
using SiraUtil.Zenject;
using UnityEngine;
using UnityEngine.SceneManagement;
using WeatherReloaded.Components;
using WeatherReloaded.Configuration;
using WeatherReloaded.Core;
using WeatherReloaded.Installers;
using Config = IPA.Config.Config;
using IPALogger = IPA.Logging.Logger;

namespace WeatherReloaded
{
    [Plugin(RuntimeOptions.DynamicInit)]
    internal class Plugin
    {
        private static bool _hasEmptyTransitioned;
        internal static IPALogger? Log { get; private set; }
        internal static Harmony? Harmony { get; private set; }
        internal static Config? Config { get; private set; }
        
        internal const string Menu = "MainMenu";
        internal const string Game = "GameCore";

        [Init]
        public Plugin(IPALogger logger, Config config, SiraUtil.Zenject.Zenjector zenjector)
        {
            Log = logger;
            Config = config;
            Log.Info("Initializing WeatherReloaded");
    
            zenjector.Install<WeatherMenuInstaller>(Location.Menu);
            Harmony = new Harmony("com.FutureMapper.Weather");
        }

        [UsedImplicitly]
        [OnEnable]
        public void OnEnable()
        {
            SceneManager.activeSceneChanged += SceneChanged;
            PluginConfig.Instance = Config!.Generated<PluginConfig>();

            PluginConfig.Instance.AudioSfxVolume = Mathf.Clamp(PluginConfig.Instance.AudioSfxVolume, 0f, 1f);
            MiscConfig.Read();
            Harmony?.PatchAll(Assembly.GetExecutingAssembly());
        }

        [UsedImplicitly]
        [OnDisable]
        public void OnDisable()
        {
            SceneManager.activeSceneChanged -= SceneChanged;
            Harmony?.UnpatchSelf();
        }

        private void SceneChanged(Scene scene, Scene arg2)
        {
            Log?.Info(scene.name + " " + arg2.name);
            if (arg2.name == "HealthWarning" && !_hasEmptyTransitioned)
            {
                _hasEmptyTransitioned = true;
                BundleLoader.Load();
            }

            if (!_hasEmptyTransitioned && arg2.name == "EmptyTransition")
            {
                _hasEmptyTransitioned = true;
                BundleLoader.Load();
            }

            switch (arg2.name)
            {
                case Menu:
                    Log?.Debug(Menu);
                    MenuSceneActive();
                    break;
                case Game:
                    GameSceneActive();
                    break;
            }
        }

        private void MenuSceneActive()
        {
            WeatherSceneInfo.CurrentScene = SceneManager.GetSceneByName(Menu);
            if (BundleLoader.WeatherPrefab != null)
            {
                var sceneInfo = BundleLoader.WeatherPrefab.GetComponent<WeatherSceneInfo>();
                sceneInfo?.SetActiveRefs();
            }
        }

        private static void GameSceneActive()
        {
            WeatherSceneInfo.CurrentScene = SceneManager.GetSceneByName(Game);
            if (BundleLoader.WeatherPrefab != null)
            {
                var sceneInfo = BundleLoader.WeatherPrefab.GetComponent<WeatherSceneInfo>();
                sceneInfo?.SetActiveRefs();
            }
        }
    }
}