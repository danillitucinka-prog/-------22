#if LOXQUEST_NETCODE
namespace LoxQuest3D.Net
{
    public static class LanBoot
    {
        public enum Mode
        {
            None = 0,
            Host = 1,
            Client = 2
        }

        public static Mode CurrentMode { get; private set; } = Mode.None;
        public static string Address { get; private set; } = "127.0.0.1";
        public static ushort Port { get; private set; } = 7777;

        public static void Reset()
        {
            CurrentMode = Mode.None;
            Address = "127.0.0.1";
            Port = 7777;
        }

        public static void SetHost(ushort port)
        {
            CurrentMode = Mode.Host;
            Address = "0.0.0.0";
            Port = port;
        }

        public static void SetClient(string address, ushort port)
        {
            CurrentMode = Mode.Client;
            Address = address;
            Port = port;
        }
    }
}
#endif

