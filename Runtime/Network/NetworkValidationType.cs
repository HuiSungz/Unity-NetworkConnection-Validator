
using System;

namespace NetworkConnector
{
    [Flags]
    public enum NetworkValidationType
    {
        None = 0,
        UnityNetwork = 1 << 0,
        Http = 1 << 1,
        Ping = 1 << 2,
        
        Default = UnityNetwork,
        All = UnityNetwork | Http | Ping,
        WebCheck = UnityNetwork | Http,
        PingCheck = UnityNetwork | Ping
    }
}