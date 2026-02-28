using System.Text;
using LoxQuest3D.Gameplay;
using LoxQuest3D.Items;
using UnityEngine;
using UnityEngine.UI;

namespace LoxQuest3D.UI
{
    public sealed class InventoryPresenter : MonoBehaviour
    {
        public LoxQuest3D.Scenes.GameBootstrapper bootstrapper;

        [Header("UI")]
        public Text inventoryText;
        public Button useCigarettesButton;
        public Button useBeerButton;
        public Button useVodkaButton;
        public Button useNoodlesButton;
        public Text useResultText;

        private void Start()
        {
            Wire(useCigarettesButton, ItemId.Cigarettes);
            Wire(useBeerButton, ItemId.CheapBeer);
            Wire(useVodkaButton, ItemId.VodkaSmall);
            Wire(useNoodlesButton, ItemId.InstantNoodles);

            Refresh();
        }

        private void Wire(Button button, ItemId item)
        {
            if (button == null) return;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                var state = bootstrapper.Context.State;
                if (ConsumableSystem.TryUse(state, item, bootstrapper.config.maxStress, out var result))
                    useResultText.text = result;
                else
                    useResultText.text = result;
                Refresh();
            });
        }

        public void Refresh()
        {
            if (inventoryText == null || bootstrapper == null) return;
            var state = bootstrapper.Context.State;
            var sb = new StringBuilder();
            sb.AppendLine($"Деньги: {state.money}");
            sb.AppendLine($"День: {state.currentDay}/{state.targetDayCount} ({state.currentSlot})");
            sb.AppendLine($"Стресс: {state.stress}/{bootstrapper.config.maxStress}");
            sb.AppendLine($"Локация: {(LoxQuest3D.World.LocationId)state.locationId}");
            sb.AppendLine("Инвентарь:");
            if (state.inventory.items.Count == 0)
            {
                sb.AppendLine("- пусто");
            }
            else
            {
                for (int i = 0; i < state.inventory.items.Count; i++)
                {
                    var stack = state.inventory.items[i];
                    sb.AppendLine($"- {(ItemId)stack.id} x{stack.count}");
                }
            }
            inventoryText.text = sb.ToString();
        }
    }
}

