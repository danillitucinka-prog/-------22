using LoxQuest3D.Core;
using LoxQuest3D.Encounters;
using LoxQuest3D.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace LoxQuest3D.UI
{
    public sealed class EncounterPresenter : MonoBehaviour
    {
        [Header("Dependencies")]
        public LoxQuest3D.Scenes.GameBootstrapper bootstrapper;

        [Header("UI")]
        public Text titleText;
        public Text bodyText;
        public Button[] choiceButtons;
        public Text resultText;

        private EncounterDefinition _current;

        private void Start()
        {
            if (bootstrapper == null)
            {
                Debug.LogError("EncounterPresenter: missing bootstrapper");
                enabled = false;
                return;
            }

            ShowNextEncounter();
        }

        public void ShowNextEncounter()
        {
            resultText.text = "";
            var state = bootstrapper.Context.State;

            if (state.currentDay > state.targetDayCount || state.money <= 0)
            {
                titleText.text = "Конец";
                bodyText.text = state.money <= 0 ? "Деньги закончились. Уныльск победил." : "Ты дожил до зарплаты. Почти.";
                SetChoicesEnabled(false);
                return;
            }

            if (!bootstrapper.EncounterSystem.TryGetEncounter(state, out _current))
            {
                titleText.text = "Тишина";
                bodyText.text = "Никто не подошёл. Подозрительно.";
                SetChoicesEnabled(false);
                return;
            }

            titleText.text = _current.title;
            bodyText.text = _current.body;

            for (int i = 0; i < choiceButtons.Length; i++)
            {
                if (i >= _current.choices.Count)
                {
                    choiceButtons[i].gameObject.SetActive(false);
                    continue;
                }

                var idx = i;
                choiceButtons[i].gameObject.SetActive(true);
                var btnText = choiceButtons[i].GetComponentInChildren<Text>();
                if (btnText != null) btnText.text = _current.choices[i].label;

                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => Choose(idx));
            }
        }

        private void Choose(int index)
        {
            var state = bootstrapper.Context.State;
            var config = bootstrapper.config;
            var choice = _current.choices[index];

            ChoiceApplier.Apply(state, choice, config.maxStress);
            resultText.text = choice.resultText;

            // Move time forward after each encounter (simple MVP pacing)
            DayCycle.AdvanceSlot(state);

            ShowNextEncounter();
        }

        private void SetChoicesEnabled(bool enabledState)
        {
            for (int i = 0; i < choiceButtons.Length; i++)
                choiceButtons[i].interactable = enabledState;
        }
    }
}

