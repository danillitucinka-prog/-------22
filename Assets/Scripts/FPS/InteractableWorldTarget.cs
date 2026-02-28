using LoxQuest3D.Scenes;
using LoxQuest3D.World;
using UnityEngine;

namespace LoxQuest3D.FPS
{
    public sealed class InteractableWorldTarget : MonoBehaviour
    {
        public string displayName = "Объект";
        public LocationId setLocationOnInteract = LocationId.Apartment;

        [Header("Mode")]
        public bool openUiPanels = true;

        public void Interact()
        {
            var bootstrap = FindFirstObjectByType<GameBootstrapper>();
            if (bootstrap == null) return;

            bootstrap.Context.State.locationId = (int)setLocationOnInteract;

            if (!openUiPanels) return;

            var interactUi = FindFirstObjectByType<LoxQuest3D.UI.InteractablePresenter>();
            if (interactUi != null) interactUi.ShowNext();

            var encounterUi = FindFirstObjectByType<LoxQuest3D.UI.EncounterPresenter>();
            if (encounterUi != null) encounterUi.ShowNextEncounter();

            var invUi = FindFirstObjectByType<LoxQuest3D.UI.InventoryPresenter>();
            if (invUi != null) invUi.Refresh();
        }
    }
}

