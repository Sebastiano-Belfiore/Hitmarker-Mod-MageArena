using FishNet.Managing;
using HitMarker.Utils;
using System.Linq;
using UnityEngine;

namespace HitMarker.Console.Commands.Give
{
    public static class ModSpawnHelper
    {
        // Questo metodo è chiamato dal CommandManager e viene eseguito solo sull'host
        public static void SpawnItem(string itemId)
        {
            var network = NetworkManager.Instances.FirstOrDefault();
            if (network == null || !network.IsHostStarted)
            {
                LoggerUtils.LogError("SpawnItem", "Failed to spawn item: Not running as host.");
                return;
            }

            var forges = GameObject.FindObjectsOfType<CraftingForge>();
            GameObject prefab = null;
            CraftingForge foundForge = null;

            int itemIndex;
            if (!int.TryParse(itemId, out itemIndex))
            {
                LoggerUtils.LogError("SpawnItem", $"Item id '{itemId}' non valido.");
                return;
            }

            foreach (var forge in forges)
            {
                if (itemIndex >= 0 && itemIndex < forge.CraftablePrefabs.Length)
                {
                    prefab = forge.CraftablePrefabs[itemIndex];
                    if (prefab != null)
                    {
                        foundForge = forge;
                        break;
                    }
                }
            }

            if (prefab == null || foundForge == null)
            {
                LoggerUtils.LogError("SpawnItem", $"Item '{itemId}' non trovato.");
                return;
            }

            GameObject player = GameObject.FindWithTag("Player");
            Vector3 spawnPosition = player != null
                ? player.transform.position + player.transform.forward * 2f
                : foundForge.itemSpawnPoint != null
                    ? foundForge.itemSpawnPoint.position
                    : Vector3.zero;

            var spawned = UnityEngine.Object.Instantiate(prefab, spawnPosition, Quaternion.identity);

            foundForge.ServerManager.Spawn(spawned);
            LoggerUtils.LogInfo("SpawnItem", $"Item '{itemId}' spawnato con successo.");
        }
    }
}
