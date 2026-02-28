using LoxQuest3D.NPC;
using LoxQuest3D.Vehicles;
using UnityEngine;
using UnityEngine.UI;

namespace LoxQuest3D.UI
{
    public sealed class AmbientSpawnPresenter : MonoBehaviour
    {
        public LoxQuest3D.Scenes.GameBootstrapper bootstrapper;

        [Header("UI")]
        public Text npcText;
        public Text vehicleText;
        public Button rerollButton;

        private void Start()
        {
            if (bootstrapper == null)
            {
                Debug.LogError("AmbientSpawnPresenter: missing bootstrapper");
                enabled = false;
                return;
            }

            if (rerollButton != null)
            {
                rerollButton.onClick.RemoveAllListeners();
                rerollButton.onClick.AddListener(Reroll);
            }

            Reroll();
        }

        public void Reroll()
        {
            var state = bootstrapper.Context.State;

            if (npcText != null)
            {
                if (bootstrapper.Npcs != null && bootstrapper.Npcs.TryPick(state, out var npc))
                    npcText.text = $"NPC: {npc.displayName}\n{npc.description}";
                else
                    npcText.text = "NPC: (никого)";
            }

            if (vehicleText != null)
            {
                if (bootstrapper.Vehicles != null && bootstrapper.Vehicles.TryPick(state, out var v))
                    vehicleText.text = $"Машина: {v.displayName}\n{v.description}";
                else
                    vehicleText.text = "Машина: (тишина)";
            }
        }
    }
}

