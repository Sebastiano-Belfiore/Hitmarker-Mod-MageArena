using FishNet.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FirstModProject.Utils
{
    public static class NetworkUtils
    {

        public static bool IsLocalPlayerOwner(GameObject gameObject)
        {
            if(gameObject == null)
            {
                LoggerUtils.LogWarning("NetworkUtils", "GameObject is null in IsLocalPlayerOwner check");
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
