using LoxQuest3D.Interactables;
using LoxQuest3D.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace LoxQuest3D.UI
{
    public sealed class InteractablePresenter : MonoBehaviour
    {
        [Header("Dependencies")]
        public LoxQuest3D.Scenes.GameBootstrapper bootstrapper;

        [Header("UI")]
        public Text titleText;
        public Text bodyText;
        public Button[] actionButtons;
        public Text resultText;

        private InteractableDefinition _current;

        public void ShowNext()
        {
            resultText.text = "";
            var state = bootstrapper.Context.State;

            if (!bootstrapper.InteractableSystem.TryGetInteractable(state, out _current))
            {
                titleText.text = "Нечего трогать";
                bodyText.text = "Ты огляделся. Тут всё давно трогали до тебя.";
                SetButtons(false);
                return;
            }

            titleText.text = _current.title;
            bodyText.text = _current.body;

            for (int i = 0; i < actionButtons.Length; i++)
            {
                if (i >= _current.actions.Count)
                {
                    actionButtons[i].gameObject.SetActive(false);
                    continue;
                }

                var idx = i;
                var action = _current.actions[i];
                actionButtons[i].gameObject.SetActive(true);

                var btnText = actionButtons[i].GetComponentInChildren<Text>();
                if (btnText != null) btnText.text = action.label;

                actionButtons[i].interactable = InteractableApplier.CanApply(state, action);
                actionButtons[i].onClick.RemoveAllListeners();
                actionButtons[i].onClick.AddListener(() => Choose(idx));
            }
        }

        private void Choose(int index)
        {
            var state = bootstrapper.Context.State;
            var config = bootstrapper.config;
            var action = _current.actions[index];

            if (!InteractableApplier.CanApply(state, action))
            {
                resultText.text = "Не получается. Не хватает вещей.";
                return;
            }

            InteractableApplier.Apply(state, action, config.maxStress);
            resultText.text = action.resultText;
            ShowNext();
        }

        private void SetButtons(bool enabledState)
        {
            for (int i = 0; i < actionButtons.Length; i++)
                actionButtons[i].interactable = enabledState;
        }
    }
}

