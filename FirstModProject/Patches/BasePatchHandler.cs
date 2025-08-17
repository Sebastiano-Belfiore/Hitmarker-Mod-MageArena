using HitMarkerMod.Utils;
using System.Linq;
using UnityEngine;

namespace HitMarkerMod.Patches
{
    public abstract class BasePatchHandler
    {
        public abstract string[] ValidTags { get; }
        public abstract string PatchName { get; }

        public virtual bool IsLocalPlayerOwner(GameObject owner)
        {
            bool isOwner = NetworkUtils.IsLocalPlayerOwner(owner);
            if (!isOwner)
            {
                LoggerUtils.LogDebug(PatchName, "Not owned by local player, skipping");
            }
            return isOwner;
        }

        public virtual bool IsValidHit(GameObject target)
        {
            if (target == null) return false;

            bool isValidTag = ValidTags.Contains(target.tag);
            bool hasMoreChecks = MoreCheckHit(target);

            return isValidTag || hasMoreChecks;
        }

        public virtual bool ProcessHit(GameObject target, GameObject owner)
        {
            if (!IsLocalPlayerOwner(owner))
            {
                return false;
            }

            if (IsValidHit(target))
            {
                OnValidHit(target);
                return true;
            }
            else
            {
                OnInvalidHit(target);
                return false;
            }
        }

        // Metodo astratto per controlli specifici del patch
        public abstract bool MoreCheckHit(GameObject target);

        // Metodi di logging semplificati
        public void LogPatch(string message)
        {
            LoggerUtils.LogPatch(PatchName, message);
        }

        public void LogError(string message)
        {
            LoggerUtils.LogError(PatchName, message);
        }

        public void LogDebug(string message)
        {
            LoggerUtils.LogDebug(PatchName, message);
        }

        public virtual void OnValidHit(GameObject target)
        {
            try
            {
                Mod.Instance?.ShowHitmarkerInstance();
                LoggerUtils.LogHitDetection(PatchName, ColliderUtils.GetTargetInfo(target), true);
            }
            catch (System.Exception ex)
            {
                LoggerUtils.LogError(PatchName, $"Failed to show hitmarker: {ex.Message}");
            }
        }

        public virtual void OnInvalidHit(GameObject target)
        {
            LoggerUtils.LogHitDetection(PatchName, ColliderUtils.GetTargetInfo(target), false);
        }
    }
}