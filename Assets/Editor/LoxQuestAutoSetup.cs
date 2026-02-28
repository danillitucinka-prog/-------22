using System.IO;
using LoxQuest3D.Audio;
using LoxQuest3D.Core;
using LoxQuest3D.Scenes;
using LoxQuest3D.UI;
using LoxQuest3D.UI.Menu;
using LoxQuest3D.World;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LoxQuest3D.Editor
{
    [InitializeOnLoad]
    public static class LoxQuestAutoSetup
    {
        private const string MarkerPath = "ProjectSettings/LoxQuest3D.setup.done";
        private const string MenuScenePath = "Assets/Scenes/LoxQuest3D_Menu.unity";
        private const string CityScenePath = "Assets/Scenes/LoxQuest3D_City.unity";

        static LoxQuestAutoSetup()
        {
            EditorApplication.delayCall += TryRunOnce;
        }

        [MenuItem("LoxQuest3D/Setup Project (Generate Scene + Assets)")]
        public static void MenuRun()
        {
            RunSetup(force: true);
        }

        private static void TryRunOnce()
        {
            if (File.Exists(MarkerPath)) return;
            RunSetup(force: false);
        }

        private static void RunSetup(bool force)
        {
            try
            {
                Directory.CreateDirectory("Assets/Scenes");
                Directory.CreateDirectory("Assets/Config");
                Directory.CreateDirectory("Assets/Audio");

                var config = EnsureAsset<GameConfig>("Assets/Config/GameConfig.asset", () =>
                {
                    var a = ScriptableObject.CreateInstance<GameConfig>();
                    a.startingMoney = 3000;
                    a.startingStress = 10;
                    a.maxStress = 100;
                    a.encountersPerSlotMin = 1;
                    a.encountersPerSlotMax = 2;
                    return a;
                });

                var theme = EnsureAsset<CityTheme>("Assets/Config/CityTheme.asset", () =>
                {
                    var a = ScriptableObject.CreateInstance<CityTheme>();
                    a.cityName = "Уныльск";
                    a.subtitle = "панельки, промзона, переезд и вечный перфоратор";
                    return a;
                });

                var ambientProfile = EnsureAsset<AmbientProfile>("Assets/Config/AmbientProfile.asset",
                    () => ScriptableObject.CreateInstance<AmbientProfile>());

                CreateMenuScene();
                CreateCityScene(config, theme, ambientProfile);
                AddScenesToBuildSettings();

                File.WriteAllText(MarkerPath, "ok");
                AssetDatabase.Refresh();
                Debug.Log("LoxQuest3D: setup complete. Open scene Assets/Scenes/LoxQuest3D_Menu.unity and press Play.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"LoxQuest3D setup failed: {ex}");
                if (force) throw;
            }
        }

        private static T EnsureAsset<T>(string path, System.Func<T> factory) where T : ScriptableObject
        {
            var existing = AssetDatabase.LoadAssetAtPath<T>(path);
            if (existing != null) return existing;

            var asset = factory();
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            return asset;
        }

        private static void CreateMenuScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = SceneIds.MainMenu;

            CreateCamera();
            CreateMenuUi();

            EditorSceneManager.SaveScene(scene, MenuScenePath);
        }

        private static void CreateCityScene(GameConfig config, CityTheme theme, AmbientProfile ambientProfile)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = SceneIds.City;

            CreateCamera();

            var bootstrapGo = new GameObject("GameBootstrap");
            var bootstrap = bootstrapGo.AddComponent<GameBootstrapper>();
            bootstrap.config = config;
            bootstrap.cityTheme = theme;
            bootstrap.autoStartNewGame = true;
            bootstrap.campaignLength = CampaignLength.Days14;

            var ambientGo = new GameObject("Audio");
            var ambient = ambientGo.AddComponent<AmbientAudioController>();
            ambient.bootstrapper = bootstrap;
            ambient.profile = ambientProfile;

            CreateCity(scene, bootstrap);
            CreateHudUi(scene, bootstrap);

            EditorSceneManager.SaveScene(scene, CityScenePath);
        }

        private static void CreateCamera()
        {
            var camGo = new GameObject("Main Camera");
            camGo.tag = "MainCamera";
            camGo.transform.position = new Vector3(0, 1.6f, 0);
            var cam = camGo.AddComponent<Camera>();
            cam.orthographic = false;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Color.black;

            camGo.AddComponent<AudioListener>();
        }

        private static void CreateMenuUi()
        {
            var canvasGo = new GameObject("Canvas");
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGo.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasGo.AddComponent<GraphicRaycaster>();

            var eventSystemGo = new GameObject("EventSystem");
            eventSystemGo.AddComponent<EventSystem>();
            eventSystemGo.AddComponent<StandaloneInputModule>();

            var root = CreatePanel(canvas.transform, "Root", new Vector2(0, 0), new Vector2(1, 1));
            var title = CreateText(root, "Title", "Lox Quest 3D", 34, TextAnchor.UpperCenter, new Vector2(0.05f, 0.90f), new Vector2(0.95f, 0.98f));

            var left = CreatePanel(root, "Left", new Vector2(0.05f, 0.15f), new Vector2(0.50f, 0.88f));
            var right = CreatePanel(root, "Right", new Vector2(0.52f, 0.15f), new Vector2(0.95f, 0.88f));

            CreateText(left, "ModeTitle", "Режим", 18, TextAnchor.UpperLeft, new Vector2(0.05f, 0.92f), new Vector2(0.95f, 0.98f));
            var playSolo = CreateButton(left, "PlaySolo", "Играть (одиночный)", new Vector2(0.05f, 0.70f), new Vector2(0.95f, 0.82f));

            var hostLan = CreateButton(left, "HostLan", "Создать (LAN)", new Vector2(0.05f, 0.56f), new Vector2(0.95f, 0.68f));
            var joinLan = CreateButton(left, "JoinLan", "Войти (LAN)", new Vector2(0.05f, 0.42f), new Vector2(0.95f, 0.54f));
            var addr = CreateInput(left, "Address", "127.0.0.1", new Vector2(0.05f, 0.34f), new Vector2(0.95f, 0.40f));
            var port = CreateInput(left, "Port", "7777", new Vector2(0.05f, 0.26f), new Vector2(0.95f, 0.32f));

            var ddGo = CreateDropdown(left, "Campaign", new[] { "7 дней", "14 дней", "30 дней", "42 дня" }, new Vector2(0.05f, 0.16f), new Vector2(0.95f, 0.24f));
            var toggle = CreateToggle(left, "NewGame", "Новая игра (стереть сейв)", new Vector2(0.05f, 0.10f), new Vector2(0.95f, 0.16f));
            var status = CreateText(left, "Status", "Сейв: ...", 14, TextAnchor.UpperLeft, new Vector2(0.05f, 0.02f), new Vector2(0.95f, 0.10f));

            CreateText(right, "SettingsTitle", "Настройки", 18, TextAnchor.UpperLeft, new Vector2(0.05f, 0.92f), new Vector2(0.95f, 0.98f));
            var vol = CreateLabeledSlider(right, "Громкость", new Vector2(0.05f, 0.74f), new Vector2(0.95f, 0.86f));
            var sens = CreateLabeledSlider(right, "Чувствительность", new Vector2(0.05f, 0.58f), new Vector2(0.95f, 0.70f));
            var fpsDd = CreateDropdown(right, "FPS", new[] { "60 FPS", "120 FPS", "Без лимита" }, new Vector2(0.05f, 0.44f), new Vector2(0.95f, 0.56f));
            var apply = CreateButton(right, "Apply", "Применить", new Vector2(0.05f, 0.28f), new Vector2(0.95f, 0.40f));

            var presenterGo = new GameObject("MainMenuPresenter");
            presenterGo.transform.SetParent(root, false);
            var presenter = presenterGo.AddComponent<MainMenuPresenter>();
            presenter.campaignDropdown = ddGo;
            presenter.newGameToggle = toggle;
            presenter.playSoloButton = playSolo;
            presenter.hostLanButton = hostLan;
            presenter.joinLanButton = joinLan;
            presenter.addressInput = addr;
            presenter.portInput = port;
            presenter.volumeSlider = vol;
            presenter.sensitivitySlider = sens;
            presenter.fpsDropdown = fpsDd;
            presenter.applySettingsButton = apply;
            presenter.statusText = status;
        }

        private static void CreateCity(Scene scene, GameBootstrapper bootstrap)
        {
            // Ground
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = new Vector3(8, 1, 8);

            // Simple blocks (panel houses / kiosks / DK)
            CreateBlock("PanelHouse", new Vector3(-18, 2, -6), new Vector3(8, 4, 12));
            CreateBlock("PanelHouse2", new Vector3(-6, 2, -18), new Vector3(12, 4, 8));
            CreateBlock("DK", new Vector3(16, 2, 16), new Vector3(10, 4, 10));
            CreateBlock("Kiosk", new Vector3(6, 1, -10), new Vector3(3, 2, 3));
            CreateBlock("Bar", new Vector3(14, 1, -6), new Vector3(4, 2, 6));

            // Player
            var player = new GameObject("Player");
            player.transform.position = new Vector3(0, 1.2f, 0);
            var cc = player.AddComponent<CharacterController>();
            cc.height = 1.8f;
            cc.radius = 0.35f;
            cc.center = new Vector3(0, 0.9f, 0);
            var camGo = new GameObject("PlayerCamera");
            camGo.transform.SetParent(player.transform, false);
            camGo.transform.localPosition = new Vector3(0, 1.6f, 0);
            var cam = camGo.AddComponent<Camera>();
            cam.tag = "MainCamera";
            camGo.AddComponent<AudioListener>();

            var controller = player.AddComponent<LoxQuest3D.FPS.FpsPlayerController>();
            controller.playerCamera = cam;

            var interactor = player.AddComponent<LoxQuest3D.FPS.WorldInteractor>();
            interactor.cam = cam;

            // Hint UI for E
            var hintCanvas = new GameObject("HintCanvas");
            var hCanvas = hintCanvas.AddComponent<Canvas>();
            hCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            hintCanvas.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            hintCanvas.AddComponent<GraphicRaycaster>();
            var hint = CreateText(hintCanvas.transform, "Hint", "", 16, TextAnchor.LowerCenter, new Vector2(0.2f, 0.02f), new Vector2(0.8f, 0.08f));
            interactor.hintText = hint;

            // World interact targets (walk up + E)
            CreateTarget("Квартира", new Vector3(-18, 0.5f, -6), LocationId.Apartment);
            CreateTarget("Подъезд", new Vector3(-14, 0.5f, -6), LocationId.Entrance);
            CreateTarget("Двор", new Vector3(-10, 0.5f, -2), LocationId.Yard);
            CreateTarget("Остановка", new Vector3(0, 0.5f, -16), LocationId.BusStop);
            CreateTarget("Киоск", new Vector3(6, 0.5f, -10), LocationId.Kiosk);
            CreateTarget("Бар", new Vector3(14, 0.5f, -6), LocationId.Bar);
            CreateTarget("Рынок", new Vector3(10, 0.5f, 6), LocationId.Market);
            CreateTarget("ДК", new Vector3(16, 0.5f, 16), LocationId.PalaceOfCulture);
            CreateTarget("Переезд", new Vector3(-2, 0.5f, 18), LocationId.RailCrossing);
        }

        private static void CreateBlock(string name, Vector3 pos, Vector3 size)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.position = pos;
            go.transform.localScale = size;
        }

        private static void CreateTarget(string display, Vector3 pos, LocationId loc)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            go.name = $"Target_{display}";
            go.transform.position = pos;
            go.transform.localScale = new Vector3(1.2f, 0.2f, 1.2f);
            var t = go.AddComponent<LoxQuest3D.FPS.InteractableWorldTarget>();
            t.displayName = display;
            t.setLocationOnInteract = loc;
        }

        private static void CreateHudUi(Scene scene, GameBootstrapper bootstrap)
        {
            var canvasGo = new GameObject("Canvas");
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGo.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasGo.AddComponent<GraphicRaycaster>();

            var eventSystemGo = new GameObject("EventSystem");
            eventSystemGo.AddComponent<EventSystem>();
            eventSystemGo.AddComponent<StandaloneInputModule>();

            var root = CreatePanel(canvas.transform, "Root", new Vector2(0, 0), new Vector2(1, 1));
            var left = CreatePanel(root, "LeftPanel", new Vector2(0, 0), new Vector2(0.45f, 1));
            var right = CreatePanel(root, "RightPanel", new Vector2(0.45f, 0), new Vector2(1, 1));

            // Encounter UI (right, top)
            var encounterPanel = CreatePanel(right, "EncounterPanel", new Vector2(0, 0.35f), new Vector2(1, 1));
            var encTitle = CreateText(encounterPanel, "EncounterTitle", "Событие", 22, TextAnchor.UpperLeft, new Vector2(0.02f, 0.98f), new Vector2(0.98f, 0.90f));
            var encBody = CreateText(encounterPanel, "EncounterBody", "Текст события...", 16, TextAnchor.UpperLeft, new Vector2(0.02f, 0.88f), new Vector2(0.98f, 0.55f));
            var encResult = CreateText(encounterPanel, "EncounterResult", "", 16, TextAnchor.UpperLeft, new Vector2(0.02f, 0.52f), new Vector2(0.98f, 0.40f));
            var choiceRow = CreatePanel(encounterPanel, "Choices", new Vector2(0.02f, 0.02f), new Vector2(0.98f, 0.38f));

            var choiceButtons = new Button[4];
            for (int i = 0; i < choiceButtons.Length; i++)
            {
                var y0 = 1f - (i + 1) * 0.24f;
                var y1 = y0 + 0.22f;
                choiceButtons[i] = CreateButton(choiceRow, $"Choice{i + 1}", $"Вариант {i + 1}", new Vector2(0f, y0), new Vector2(1f, y1));
            }

            var encounterPresenterGo = new GameObject("EncounterPresenter");
            encounterPresenterGo.transform.SetParent(encounterPanel, false);
            var encounterPresenter = encounterPresenterGo.AddComponent<EncounterPresenter>();
            encounterPresenter.bootstrapper = bootstrap;
            encounterPresenter.titleText = encTitle;
            encounterPresenter.bodyText = encBody;
            encounterPresenter.resultText = encResult;
            encounterPresenter.choiceButtons = choiceButtons;

            // Location menu (left, top)
            var locPanel = CreatePanel(left, "LocationPanel", new Vector2(0, 0.55f), new Vector2(1, 1));
            var locText = CreateText(locPanel, "LocationText", "Локация: Apartment", 18, TextAnchor.UpperLeft, new Vector2(0.02f, 0.98f), new Vector2(0.98f, 0.88f));
            var locButtonsPanel = CreatePanel(locPanel, "LocationButtons", new Vector2(0.02f, 0.02f), new Vector2(0.98f, 0.86f));

            var btnApartment = CreateButton(locButtonsPanel, "ToApartment", "Квартира", new Vector2(0f, 0.84f), new Vector2(1f, 0.98f));
            var btnEntrance = CreateButton(locButtonsPanel, "ToEntrance", "Подъезд", new Vector2(0f, 0.68f), new Vector2(1f, 0.82f));
            var btnYard = CreateButton(locButtonsPanel, "ToYard", "Двор", new Vector2(0f, 0.52f), new Vector2(1f, 0.66f));
            var btnStop = CreateButton(locButtonsPanel, "ToStop", "Остановка", new Vector2(0f, 0.36f), new Vector2(1f, 0.50f));
            var btnKiosk = CreateButton(locButtonsPanel, "ToKiosk", "Киоск", new Vector2(0f, 0.20f), new Vector2(1f, 0.34f));
            var btnBar = CreateButton(locButtonsPanel, "ToBar", "Бар", new Vector2(0f, 0.04f), new Vector2(1f, 0.18f));

            var locPresenterGo = new GameObject("LocationMenuPresenter");
            locPresenterGo.transform.SetParent(locPanel, false);
            var locPresenter = locPresenterGo.AddComponent<LocationMenuPresenter>();
            locPresenter.bootstrapper = bootstrap;
            locPresenter.locationText = locText;
            locPresenter.toApartmentButton = btnApartment;
            locPresenter.toYardButton = btnYard;
            locPresenter.toBusStopButton = btnStop;
            locPresenter.toKioskButton = btnKiosk;
            locPresenter.toBarButton = btnBar;
            locPresenter.toStoreButton = null;
            locPresenter.toParkButton = null;
            locPresenter.toOfficeButton = null;
            locPresenter.toPharmacyButton = null;

            // Interactables (right, bottom-left-ish)
            var interPanel = CreatePanel(right, "InteractablePanel", new Vector2(0, 0), new Vector2(0.5f, 0.35f));
            var itTitle = CreateText(interPanel, "InteractTitle", "Объект", 18, TextAnchor.UpperLeft, new Vector2(0.02f, 0.98f), new Vector2(0.98f, 0.86f));
            var itBody = CreateText(interPanel, "InteractBody", "Описание...", 14, TextAnchor.UpperLeft, new Vector2(0.02f, 0.84f), new Vector2(0.98f, 0.52f));
            var itResult = CreateText(interPanel, "InteractResult", "", 14, TextAnchor.UpperLeft, new Vector2(0.02f, 0.50f), new Vector2(0.98f, 0.40f));
            var itButtonsPanel = CreatePanel(interPanel, "InteractButtons", new Vector2(0.02f, 0.02f), new Vector2(0.98f, 0.38f));
            var itButtons = new Button[3];
            for (int i = 0; i < itButtons.Length; i++)
            {
                var y0 = 1f - (i + 1) * 0.33f;
                var y1 = y0 + 0.30f;
                itButtons[i] = CreateButton(itButtonsPanel, $"Action{i + 1}", $"Действие {i + 1}", new Vector2(0f, y0), new Vector2(1f, y1));
            }

            var interactPresenterGo = new GameObject("InteractablePresenter");
            interactPresenterGo.transform.SetParent(interPanel, false);
            var interactPresenter = interactPresenterGo.AddComponent<InteractablePresenter>();
            interactPresenter.bootstrapper = bootstrap;
            interactPresenter.titleText = itTitle;
            interactPresenter.bodyText = itBody;
            interactPresenter.resultText = itResult;
            interactPresenter.actionButtons = itButtons;

            // Inventory (left, bottom)
            var invPanel = CreatePanel(left, "InventoryPanel", new Vector2(0, 0), new Vector2(1, 0.55f));
            var invText = CreateText(invPanel, "InventoryText", "Инвентарь", 14, TextAnchor.UpperLeft, new Vector2(0.02f, 0.98f), new Vector2(0.98f, 0.55f));
            var invButtons = CreatePanel(invPanel, "InvButtons", new Vector2(0.02f, 0.30f), new Vector2(0.98f, 0.52f));
            var btnCigs = CreateButton(invButtons, "UseCigs", "Исп. сигареты", new Vector2(0f, 0.52f), new Vector2(1f, 1f));
            var btnBeer = CreateButton(invButtons, "UseBeer", "Исп. пиво", new Vector2(0f, 0.00f), new Vector2(1f, 0.48f));
            var invButtons2 = CreatePanel(invPanel, "InvButtons2", new Vector2(0.02f, 0.06f), new Vector2(0.98f, 0.28f));
            var btnVodka = CreateButton(invButtons2, "UseVodka", "Исп. водку", new Vector2(0f, 0.52f), new Vector2(1f, 1f));
            var btnNoodles = CreateButton(invButtons2, "UseNoodles", "Исп. лапшу", new Vector2(0f, 0.00f), new Vector2(1f, 0.48f));
            var useRes = CreateText(invPanel, "UseResult", "", 14, TextAnchor.UpperLeft, new Vector2(0.02f, 0.02f), new Vector2(0.98f, 0.06f));

            var invPresenterGo = new GameObject("InventoryPresenter");
            invPresenterGo.transform.SetParent(invPanel, false);
            var invPresenter = invPresenterGo.AddComponent<InventoryPresenter>();
            invPresenter.bootstrapper = bootstrap;
            invPresenter.inventoryText = invText;
            invPresenter.useCigarettesButton = btnCigs;
            invPresenter.useBeerButton = btnBeer;
            invPresenter.useVodkaButton = btnVodka;
            invPresenter.useNoodlesButton = btnNoodles;
            invPresenter.useResultText = useRes;

            // Ambient spawn (right, bottom-right)
            var ambPanel = CreatePanel(right, "AmbientPanel", new Vector2(0.5f, 0), new Vector2(1, 0.35f));
            var npcText = CreateText(ambPanel, "NpcText", "NPC:", 14, TextAnchor.UpperLeft, new Vector2(0.02f, 0.98f), new Vector2(0.98f, 0.60f));
            var vehText = CreateText(ambPanel, "VehicleText", "Машина:", 14, TextAnchor.UpperLeft, new Vector2(0.02f, 0.58f), new Vector2(0.98f, 0.20f));
            var reroll = CreateButton(ambPanel, "Reroll", "Обновить окружение", new Vector2(0.02f, 0.02f), new Vector2(0.98f, 0.18f));

            var ambPresenterGo = new GameObject("AmbientSpawnPresenter");
            ambPresenterGo.transform.SetParent(ambPanel, false);
            var ambPresenter = ambPresenterGo.AddComponent<AmbientSpawnPresenter>();
            ambPresenter.bootstrapper = bootstrap;
            ambPresenter.npcText = npcText;
            ambPresenter.vehicleText = vehText;
            ambPresenter.rerollButton = reroll;

            // Kickstart presenters that don't auto-run in Start
            interactPresenter.ShowNext();
            invPresenter.Refresh();
        }

        private static RectTransform CreatePanel(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rt = go.AddComponent<RectTransform>();
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            var img = go.AddComponent<Image>();
            img.color = new Color(0f, 0f, 0f, 0.35f);
            return rt;
        }

        private static Text CreateText(Transform parent, string name, string text, int fontSize, TextAnchor align,
            Vector2 anchorMin, Vector2 anchorMax)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rt = go.AddComponent<RectTransform>();
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            var t = go.AddComponent<Text>();
            t.text = text;
            t.font = GetDefaultFont();
            t.fontSize = fontSize;
            t.alignment = align;
            t.horizontalOverflow = HorizontalWrapMode.Wrap;
            t.verticalOverflow = VerticalWrapMode.Overflow;
            t.color = Color.white;
            return t;
        }

        private static Button CreateButton(Transform parent, string name, string label, Vector2 anchorMin, Vector2 anchorMax)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rt = go.AddComponent<RectTransform>();
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            var img = go.AddComponent<Image>();
            img.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);

            var btn = go.AddComponent<Button>();

            var textGo = new GameObject("Text");
            textGo.transform.SetParent(go.transform, false);
            var trt = textGo.AddComponent<RectTransform>();
            trt.anchorMin = new Vector2(0.05f, 0.05f);
            trt.anchorMax = new Vector2(0.95f, 0.95f);
            trt.offsetMin = Vector2.zero;
            trt.offsetMax = Vector2.zero;

            var t = textGo.AddComponent<Text>();
            t.text = label;
            t.font = GetDefaultFont();
            t.fontSize = 14;
            t.alignment = TextAnchor.MiddleCenter;
            t.color = Color.white;

            var colors = btn.colors;
            colors.highlightedColor = new Color(0.3f, 0.3f, 0.3f, 1f);
            colors.pressedColor = new Color(0.15f, 0.15f, 0.15f, 1f);
            btn.colors = colors;

            return btn;
        }

        private static Font GetDefaultFont()
        {
            // Unity 6: Arial.ttf is no longer a valid built-in font.
            var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (font != null) return font;
            return Resources.GetBuiltinResource<Font>("Arial.ttf");
        }

        private static Dropdown CreateDropdown(Transform parent, string name, string[] options, Vector2 anchorMin, Vector2 anchorMax)
        {
            var root = new GameObject(name);
            root.transform.SetParent(parent, false);
            var rt = root.AddComponent<RectTransform>();
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            var img = root.AddComponent<Image>();
            img.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);

            var dd = root.AddComponent<Dropdown>();
            dd.targetGraphic = img;

            var label = CreateText(root.transform, "Label", options.Length > 0 ? options[0] : "", 14, TextAnchor.MiddleLeft,
                new Vector2(0.05f, 0.1f), new Vector2(0.85f, 0.9f));
            dd.captionText = label;

            var arrowGo = new GameObject("Arrow");
            arrowGo.transform.SetParent(root.transform, false);
            var art = arrowGo.AddComponent<RectTransform>();
            art.anchorMin = new Vector2(0.88f, 0.2f);
            art.anchorMax = new Vector2(0.98f, 0.8f);
            art.offsetMin = Vector2.zero;
            art.offsetMax = Vector2.zero;
            var arrowText = arrowGo.AddComponent<Text>();
            arrowText.font = GetDefaultFont();
            arrowText.text = "▼";
            arrowText.alignment = TextAnchor.MiddleCenter;
            arrowText.color = Color.white;

            // Template (hidden)
            var templateGo = CreatePanel(root.transform, "Template", new Vector2(0, 0), new Vector2(1, 0));
            templateGo.gameObject.SetActive(false);
            var scroll = templateGo.gameObject.AddComponent<ScrollRect>();
            var viewport = CreatePanel(templateGo, "Viewport", new Vector2(0, 0), new Vector2(1, 1));
            viewport.gameObject.AddComponent<Mask>().showMaskGraphic = false;
            var vpImg = viewport.gameObject.AddComponent<Image>();
            vpImg.color = new Color(0, 0, 0, 0.6f);
            scroll.viewport = viewport;
            var content = CreatePanel(viewport, "Content", new Vector2(0, 1), new Vector2(1, 1));
            content.pivot = new Vector2(0.5f, 1);
            scroll.content = content;

            dd.template = templateGo;

            dd.options.Clear();
            foreach (var o in options)
                dd.options.Add(new Dropdown.OptionData(o));
            dd.value = 0;
            dd.RefreshShownValue();

            return dd;
        }

        private static Toggle CreateToggle(Transform parent, string name, string label, Vector2 anchorMin, Vector2 anchorMax)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rt = go.AddComponent<RectTransform>();
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            var toggle = go.AddComponent<Toggle>();

            var bg = new GameObject("Background");
            bg.transform.SetParent(go.transform, false);
            var bgRt = bg.AddComponent<RectTransform>();
            bgRt.anchorMin = new Vector2(0, 0.1f);
            bgRt.anchorMax = new Vector2(0.08f, 0.9f);
            bgRt.offsetMin = Vector2.zero;
            bgRt.offsetMax = Vector2.zero;
            var bgImg = bg.AddComponent<Image>();
            bgImg.color = new Color(0.2f, 0.2f, 0.2f, 1f);

            var check = new GameObject("Checkmark");
            check.transform.SetParent(bg.transform, false);
            var ckRt = check.AddComponent<RectTransform>();
            ckRt.anchorMin = new Vector2(0.15f, 0.15f);
            ckRt.anchorMax = new Vector2(0.85f, 0.85f);
            ckRt.offsetMin = Vector2.zero;
            ckRt.offsetMax = Vector2.zero;
            var ckImg = check.AddComponent<Image>();
            ckImg.color = new Color(0.8f, 0.8f, 0.8f, 1f);

            toggle.targetGraphic = bgImg;
            toggle.graphic = ckImg;

            var text = CreateText(go.transform, "Label", label, 14, TextAnchor.MiddleLeft, new Vector2(0.10f, 0), new Vector2(1, 1));
            return toggle;
        }

        private static Slider CreateLabeledSlider(Transform parent, string label, Vector2 anchorMin, Vector2 anchorMax)
        {
            var panel = CreatePanel(parent, $"Slider_{label}", anchorMin, anchorMax);
            CreateText(panel, "Label", label, 14, TextAnchor.UpperLeft, new Vector2(0.02f, 0.55f), new Vector2(0.98f, 0.98f));

            var sliderGo = new GameObject("Slider");
            sliderGo.transform.SetParent(panel, false);
            var rt = sliderGo.AddComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.05f, 0.10f);
            rt.anchorMax = new Vector2(0.95f, 0.45f);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            var slider = sliderGo.AddComponent<Slider>();
            slider.minValue = 0;
            slider.maxValue = 1;
            slider.value = 0.8f;

            var bg = new GameObject("Background");
            bg.transform.SetParent(sliderGo.transform, false);
            var bgRt = bg.AddComponent<RectTransform>();
            bgRt.anchorMin = new Vector2(0, 0.25f);
            bgRt.anchorMax = new Vector2(1, 0.75f);
            bgRt.offsetMin = Vector2.zero;
            bgRt.offsetMax = Vector2.zero;
            var bgImg = bg.AddComponent<Image>();
            bgImg.color = new Color(0.15f, 0.15f, 0.15f, 1f);

            var fillArea = new GameObject("Fill Area");
            fillArea.transform.SetParent(sliderGo.transform, false);
            var faRt = fillArea.AddComponent<RectTransform>();
            faRt.anchorMin = new Vector2(0, 0.25f);
            faRt.anchorMax = new Vector2(1, 0.75f);
            faRt.offsetMin = new Vector2(5, 0);
            faRt.offsetMax = new Vector2(-5, 0);

            var fill = new GameObject("Fill");
            fill.transform.SetParent(fillArea.transform, false);
            var fRt = fill.AddComponent<RectTransform>();
            fRt.anchorMin = new Vector2(0, 0);
            fRt.anchorMax = new Vector2(1, 1);
            fRt.offsetMin = Vector2.zero;
            fRt.offsetMax = Vector2.zero;
            var fImg = fill.AddComponent<Image>();
            fImg.color = new Color(0.6f, 0.6f, 0.6f, 1f);

            var handle = new GameObject("Handle");
            handle.transform.SetParent(sliderGo.transform, false);
            var hRt = handle.AddComponent<RectTransform>();
            hRt.sizeDelta = new Vector2(16, 16);
            var hImg = handle.AddComponent<Image>();
            hImg.color = new Color(0.9f, 0.9f, 0.9f, 1f);

            slider.targetGraphic = hImg;
            slider.fillRect = fRt;
            slider.handleRect = hRt;
            slider.direction = Slider.Direction.LeftToRight;

            return slider;
        }

        private static InputField CreateInput(Transform parent, string name, string initial, Vector2 anchorMin, Vector2 anchorMax)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rt = go.AddComponent<RectTransform>();
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            var img = go.AddComponent<Image>();
            img.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);

            var input = go.AddComponent<InputField>();
            input.targetGraphic = img;

            var placeholder = CreateText(go.transform, "Placeholder", initial, 14, TextAnchor.MiddleLeft, new Vector2(0.05f, 0.1f), new Vector2(0.95f, 0.9f));
            placeholder.color = new Color(1, 1, 1, 0.4f);
            input.placeholder = placeholder;

            var text = CreateText(go.transform, "Text", initial, 14, TextAnchor.MiddleLeft, new Vector2(0.05f, 0.1f), new Vector2(0.95f, 0.9f));
            input.textComponent = text;
            input.text = initial;

            return input;
        }

        private static void AddScenesToBuildSettings()
        {
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(MenuScenePath, true),
                new EditorBuildSettingsScene(CityScenePath, true)
            };
        }
    }
}
