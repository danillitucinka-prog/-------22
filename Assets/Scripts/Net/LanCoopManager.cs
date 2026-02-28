#if LOXQUEST_NETCODE
using LoxQuest3D.Core;
using LoxQuest3D.Encounters;
using LoxQuest3D.Gameplay;
using Unity.Netcode;
using UnityEngine;

namespace LoxQuest3D.Net
{
    public sealed class LanCoopManager : NetworkBehaviour
    {
        [Header("Refs")]
        public LoxQuest3D.Scenes.GameBootstrapper bootstrapper;

        private readonly NetworkVariable<int> _money = new(writePerm: NetworkVariableWritePermission.Server);
        private readonly NetworkVariable<int> _day = new(writePerm: NetworkVariableWritePermission.Server);
        private readonly NetworkVariable<int> _slot = new(writePerm: NetworkVariableWritePermission.Server);
        private readonly NetworkVariable<int> _location = new(writePerm: NetworkVariableWritePermission.Server);

        public override void OnNetworkSpawn()
        {
            if (bootstrapper == null)
            {
                Debug.LogError("LanCoopManager: missing bootstrapper");
                enabled = false;
                return;
            }

            _money.OnValueChanged += (_, __) => PullToLocal();
            _day.OnValueChanged += (_, __) => PullToLocal();
            _slot.OnValueChanged += (_, __) => PullToLocal();
            _location.OnValueChanged += (_, __) => PullToLocal();

            if (IsServer)
                PushFromLocal();
            else
                PullToLocal();
        }

        public void PushFromLocal()
        {
            if (!IsServer) return;
            var s = bootstrapper.Context.State;
            _money.Value = s.money;
            _day.Value = s.currentDay;
            _slot.Value = (int)s.currentSlot;
            _location.Value = s.locationId;
        }

        private void PullToLocal()
        {
            var s = bootstrapper.Context.State;
            s.money = _money.Value;
            s.currentDay = _day.Value;
            s.currentSlot = (DaySlot)_slot.Value;
            s.locationId = _location.Value;
        }

        [ServerRpc(RequireOwnership = false)]
        public void ApplyChoiceServerRpc(int moneyDelta, int stressDelta)
        {
            var state = bootstrapper.Context.State;
            var choice = new EncounterChoice { moneyDelta = moneyDelta, stressDelta = stressDelta };
            ChoiceApplier.Apply(state, choice, bootstrapper.config.maxStress);
            DayCycle.AdvanceSlot(state);
            PushFromLocal();
        }
    }
}
#endif

