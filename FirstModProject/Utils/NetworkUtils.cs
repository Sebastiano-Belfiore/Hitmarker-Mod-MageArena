using FishNet.Object;
using HitMarkerMod.Utils;
using UnityEngine;

namespace HitMarkerMod.Utils
{
    public static class NetworkUtils
    {
        public static bool IsLocalPlayerOwner(GameObject gameObject)
        {
            if (gameObject == null)
            {
                LoggerUtils.LogDebug("NetworkUtils", "GameObject is null in ownership check");
                return false;
            }

            NetworkObject networkObject = gameObject.GetComponent<NetworkObject>();
            return networkObject != null && networkObject.IsOwner;
        }

        public static NetworkObject GetNetworkObject(GameObject gameObject)
        {
            return gameObject?.GetComponent<NetworkObject>();
        }

        public static bool IsValidLocalOwner(NetworkObject networkObject)
        {
            return networkObject != null && networkObject.IsOwner;
        }
    }
}