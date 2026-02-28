using LoxQuest3D.Scenes;
using LoxQuest3D.World;
using UnityEngine;
using UnityEngine.UI;

namespace LoxQuest3D.UI
{
    public sealed class LocationMenuPresenter : MonoBehaviour
    {
        public GameBootstrapper bootstrapper;

        [Header("UI")]
        public Text locationText;
        public Button toApartmentButton;
        public Button toYardButton;
        public Button toBusStopButton;
        public Button toStoreButton;
        public Button toParkButton;
        public Button toOfficeButton;

        private void Start()
        {
            if (bootstrapper == null)
            {
                Debug.LogError("LocationMenuPresenter: missing bootstrapper");
                enabled = false;
                return;
            }

            Wire(toApartmentButton, LocationId.Apartment);
            Wire(toYardButton, LocationId.Yard);
            Wire(toBusStopButton, LocationId.BusStop);
            Wire(toStoreButton, LocationId.Store);
            Wire(toParkButton, LocationId.Park);
            Wire(toOfficeButton, LocationId.Office);

            Refresh();
        }

        private void Wire(Button button, LocationId id)
        {
            if (button == null) return;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                bootstrapper.Context.State.locationId = (int)id;
                Refresh();
            });
        }

        private void Refresh()
        {
            if (locationText == null) return;
            locationText.text = $"Локация: {(LocationId)bootstrapper.Context.State.locationId}";
        }
    }
}

