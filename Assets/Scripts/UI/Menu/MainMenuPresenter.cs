using LoxQuest3D.Core;
using LoxQuest3D.IO;
using LoxQuest3D.Scenes;
using LoxQuest3D.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace LoxQuest3D.UI.Menu
{
    public sealed class MainMenuPresenter : MonoBehaviour
    {
        [Header("UI")]
        public Dropdown campaignDropdown;
        public Toggle newGameToggle;
        public Button playSoloButton;
        public Button hostLanButton;
        public Button joinLanButton;
        public InputField addressInput;
        public InputField portInput;

        public Slider volumeSlider;
        public Slider sensitivitySlider;
        public Dropdown fpsDropdown;
        public Button applySettingsButton;

        public Text statusText;

        private void Start()
        {
            var settings = SettingsSystem.Load();

            if (campaignDropdown != null)
            {
                campaignDropdown.ClearOptions();
                campaignDropdown.AddOptions(new System.Collections.Generic.List<string> { "7 дней", "14 дней", "30 дней", "42 дня" });
                campaignDropdown.value = 1;
            }

            if (newGameToggle != null) newGameToggle.isOn = true;
            if (volumeSlider != null) volumeSlider.value = settings.masterVolume;
            if (sensitivitySlider != null) sensitivitySlider.value = settings.mouseSensitivity;
            if (fpsDropdown != null)
            {
                fpsDropdown.ClearOptions();
                fpsDropdown.AddOptions(new System.Collections.Generic.List<string> { "60 FPS", "120 FPS", "Без лимита" });
                fpsDropdown.value = settings.targetFps == 120 ? 1 : settings.targetFps <= 0 ? 2 : 0;
            }

            if (playSoloButton != null)
            {
                playSoloButton.onClick.RemoveAllListeners();
                playSoloButton.onClick.AddListener(PlaySolo);
            }

#if LOXQUEST_NETCODE
            if (hostLanButton != null)
            {
                hostLanButton.onClick.RemoveAllListeners();
                hostLanButton.onClick.AddListener(HostLan);
            }
            if (joinLanButton != null)
            {
                joinLanButton.onClick.RemoveAllListeners();
                joinLanButton.onClick.AddListener(JoinLan);
            }
#else
            if (hostLanButton != null) hostLanButton.gameObject.SetActive(false);
            if (joinLanButton != null) joinLanButton.gameObject.SetActive(false);
            if (addressInput != null) addressInput.gameObject.SetActive(false);
            if (portInput != null) portInput.gameObject.SetActive(false);
#endif

            if (applySettingsButton != null)
            {
                applySettingsButton.onClick.RemoveAllListeners();
                applySettingsButton.onClick.AddListener(ApplySettings);
            }

            RefreshStatus();
        }

        private void PlaySolo()
        {
            var length = CampaignLength.Days14;
            if (campaignDropdown != null)
            {
                length = campaignDropdown.value switch
                {
                    0 => CampaignLength.Days7,
                    1 => CampaignLength.Days14,
                    2 => CampaignLength.Days30,
                    3 => CampaignLength.Days42,
                    _ => CampaignLength.Days14
                };
            }

            var newGame = newGameToggle == null || newGameToggle.isOn;
            if (newGame)
                SaveSystem.Delete();

            RunConfig.Set(length, newGame);
            SceneLoader.LoadCity();
        }

#if LOXQUEST_NETCODE
        private void HostLan()
        {
            ApplyRunConfigFromUi(out var length, out var newGame);
            if (newGame) SaveSystem.Delete();
            RunConfig.Set(length, newGame);
            LoxQuest3D.Net.LanBoot.SetHost(ReadPort());
            SceneLoader.LoadCity();
        }

        private void JoinLan()
        {
            ApplyRunConfigFromUi(out var length, out var newGame);
            if (newGame) SaveSystem.Delete();
            RunConfig.Set(length, newGame);
            LoxQuest3D.Net.LanBoot.SetClient(ReadAddress(), ReadPort());
            SceneLoader.LoadCity();
        }
#endif

        private void ApplyRunConfigFromUi(out CampaignLength length, out bool newGame)
        {
            length = campaignDropdown != null ? campaignDropdown.value switch
            {
                0 => CampaignLength.Days7,
                1 => CampaignLength.Days14,
                2 => CampaignLength.Days30,
                3 => CampaignLength.Days42,
                _ => CampaignLength.Days14
            } : CampaignLength.Days14;
            newGame = newGameToggle == null || newGameToggle.isOn;
        }

        private string ReadAddress()
        {
            if (addressInput == null) return "127.0.0.1";
            var v = (addressInput.text ?? "").Trim();
            return string.IsNullOrWhiteSpace(v) ? "127.0.0.1" : v;
        }

        private ushort ReadPort()
        {
            if (portInput == null) return 7777;
            return ushort.TryParse((portInput.text ?? "").Trim(), out var p) ? p : (ushort)7777;
        }

        private void ApplySettings()
        {
            var data = new SettingsData();
            if (volumeSlider != null) data.masterVolume = volumeSlider.value;
            if (sensitivitySlider != null) data.mouseSensitivity = Mathf.Max(0.1f, sensitivitySlider.value);
            if (fpsDropdown != null)
            {
                data.targetFps = fpsDropdown.value switch
                {
                    0 => 60,
                    1 => 120,
                    2 => -1,
                    _ => 60
                };
            }

            SettingsSystem.Save(data);
            RefreshStatus();
        }

        private void RefreshStatus()
        {
            if (statusText == null) return;
            statusText.text = SaveSystem.TryLoad(out _) ? "Сейв найден" : "Сейва нет";
        }
    }
}
