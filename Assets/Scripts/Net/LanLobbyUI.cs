#if LOXQUEST_NETCODE
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

namespace LoxQuest3D.Net
{
    public sealed class LanLobbyUI : MonoBehaviour
    {
        public NetworkManager networkManager;
        public UnityTransport transport;

        [Header("UI")]
        public InputField addressInput;
        public InputField portInput;
        public Button hostButton;
        public Button joinButton;
        public Button stopButton;
        public Text statusText;

        private void Start()
        {
            if (networkManager == null) networkManager = FindFirstObjectByType<NetworkManager>();
            if (transport == null && networkManager != null) transport = networkManager.GetComponent<UnityTransport>();

            Wire(hostButton, Host);
            Wire(joinButton, Join);
            Wire(stopButton, Stop);

            if (addressInput != null) addressInput.text = LanBoot.Address;
            if (portInput != null) portInput.text = LanBoot.Port.ToString();

            // Auto-start if requested from menu
            if (LanBoot.CurrentMode == LanBoot.Mode.Host)
                Host();
            else if (LanBoot.CurrentMode == LanBoot.Mode.Client)
                Join();

            Refresh();
        }

        private void Wire(Button button, System.Action action)
        {
            if (button == null) return;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => action());
        }

        private void Host()
        {
            if (transport != null)
            {
                transport.SetConnectionData("0.0.0.0", ReadPort());
            }
            networkManager.StartHost();
            Refresh();
        }

        private void Join()
        {
            if (transport != null)
            {
                transport.SetConnectionData(ReadAddress(), ReadPort());
            }
            networkManager.StartClient();
            Refresh();
        }

        private void Stop()
        {
            if (networkManager == null) return;
            if (networkManager.IsHost) networkManager.Shutdown();
            else if (networkManager.IsClient) networkManager.Shutdown();
            Refresh();
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
            if (ushort.TryParse((portInput.text ?? "").Trim(), out var p)) return p;
            return 7777;
        }

        private void Refresh()
        {
            if (statusText == null || networkManager == null) return;
            statusText.text =
                networkManager.IsHost ? "LAN: Host" :
                networkManager.IsClient ? "LAN: Client" :
                "LAN: Offline";
        }
    }
}
#endif
