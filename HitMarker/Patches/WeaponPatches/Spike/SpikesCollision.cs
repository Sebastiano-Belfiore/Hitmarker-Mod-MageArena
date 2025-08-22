using HitMarker.Utils;
using UnityEngine;

namespace HitMarker.Patches.WeaponPatches.Spike
{
    public class SpikesCollision : MonoBehaviour
    {
        public GameObject owner;

        private void OnEnable()
        {
            LoggerUtils.LogDebug("SpikesCollision", "SpikesCollision component enabled.");
        }

        private void OnDisable()
        {
            LoggerUtils.LogDebug("SpikesCollision", "SpikesCollision component disabled. Clearing owner reference.");
            if (owner != null)
            {
                owner = null;
            }
        }

        private void OnDestroy()
        {
            LoggerUtils.LogDebug("SpikesCollision", "SpikesCollision component destroyed.");
            // Non è necessario fare owner = null qui, lo fa già OnDisable se chiamato.
        }

        private void OnTriggerEnter(Collider other)
        {
            LoggerUtils.LogDebug("SpikesCollision", $"Trigger detected with {other.gameObject.name}.");

            if (owner == null)
            {
                LoggerUtils.LogWarning("SpikesCollision", "Owner is null. Cannot process collision.");
                return;
            }
            LoggerUtils.LogDebug("SpikesCollision", $"Owner is {owner.name}.");

            if (!other.gameObject.CompareTag("Player"))
            {
                LoggerUtils.LogDebug("SpikesCollision", $"Collision target is not a player. Skipping.");
                return;
            }

            LoggerUtils.LogDebug("SpikesCollision", $"Collision target is player: {other.gameObject.name}.");

            if (other.gameObject.GetInstanceID() == owner.GetInstanceID())
            {
                LoggerUtils.LogDebug("SpikesCollision", "Collided with self. Skipping.");
                return;
            }

            bool isLocalPlayer = NetworkUtils.IsLocalPlayerOwner(owner);
            LoggerUtils.LogDebug("SpikesCollision", $"Is local player owner: {isLocalPlayer}");

            if (isLocalPlayer)
            {
                LoggerUtils.LogHitmarker($"Hit registered! Attacker: {owner.name}, Victim: {other.gameObject.name}.");
                Mod.Instance?.ShowHitmarkerInstance();
            }
            else
            {
                LoggerUtils.LogDebug("SpikesCollision", $"Not local player owner. Hitmarker not shown.");
            }
        }
    }
}