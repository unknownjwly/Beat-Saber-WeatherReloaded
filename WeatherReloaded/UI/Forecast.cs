using System.Collections.Generic;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using WeatherReloaded.Core;
using WeatherReloaded.Models;

namespace WeatherReloaded.UI
{
    internal class Forecast : BSMLResourceViewController
    {
        public override string ResourceName => "WeatherReloaded.Forecast.bsml";
        public ForecastFlowCoordinator? flow;
        
        [UIComponent("effectList")]
        private readonly CustomListTableData? _customListTableData = null;

        private readonly List<Effect> _effsInTable = new();
        [UIAction("effectSelect")]
        public void Select(TableView table, int row)
        {
            flow?.ShowEffectSettings();
            flow?.EffectViewController.SetData(_effsInTable[row]);
        }

        private readonly List<string> _multiTypeAdded = new();

        [UIAction("#post-parse")]
        public void SetupList()
        {
            if (_customListTableData == null) return;

            _customListTableData.TableView.ClearSelection();
            _customListTableData.Data.Clear();
            foreach (var effect in BundleLoader.Effects)
            {
                if (effect.IsEffectSeparateType())
                {
                    if (_multiTypeAdded.Contains(EffectModel.GetNameWithoutSceneName(effect.Desc.effectName)))
                    {
                        continue;
                    }

                    _multiTypeAdded.Add(EffectModel.GetNameWithoutSceneName(effect.Desc.effectName));
                }
                _effsInTable.Add(effect);
                var customCellInfo = new CustomListTableData.CustomCellInfo(EffectModel.GetNameWithoutSceneName(effect.Desc.effectName), effect.Desc.author, effect.Desc.coverImage);
                _customListTableData.Data.Add(customCellInfo);
            }

            _customListTableData.TableView.ReloadData();
            _customListTableData.TableView.selectionType = TableViewSelectionType.Single;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
            if (_customListTableData?.TableView != null)
            {
                _customListTableData.TableView.ClearSelection();
                _customListTableData.TableView.selectionType = TableViewSelectionType.Single;
            }
        }
    }
}