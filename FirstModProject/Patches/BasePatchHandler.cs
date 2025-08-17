using FirstModProject.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace FirstModProject.Patches
{
    public abstract class BasePatchHandler
    {
        public abstract string[] ValidTags { get; }
        public abstract string PatchName { get; }

        public virtual bool IsLocalPlayerOwner(GameObject owner)
        {
            bool b = NetworkUtils.IsLocalPlayerOwner(owner);
                if(!b) LoggerUtils.LogPatch("TorchHitDetectionPatch", "Torch not owned by local player, skipping");
            return b;
        }
        public virtual bool IsValidHit(GameObject target)
        {
            if (target == null) return false;
            bool isValid = ValidTags.Contains(target.tag);
            return isValid || MoreCheckHit(target);
            
        }
        public virtual bool ProcessHit(GameObject target, GameObject owner)
        {
            /*
              if (!IsLocalPlayerOwner(owner))
            {
                LogPatch("Weapon not owned by local player, skipping");
                return false;
            }*/
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
        public abstract bool MoreCheckHit(GameObject target);

        public virtual void LogPatch(string message)
        {
            LoggerUtils.LogPatch(PatchName, message);
        }

        public virtual void LogError(string message)
        {
            LoggerUtils.LogError(PatchName, message);
        }

        public virtual void OnValidHit(GameObject target)
        {
            Mod.Instance.ShowHitmarkerInstance();
            LoggerUtils.LogHitDetection(PatchName, ColliderUtils.GetTargetInfo(target), true);
        }

        public virtual void OnInvalidHit(GameObject target)
        {
            LoggerUtils.LogHitDetection(PatchName, ColliderUtils.GetTargetInfo(target), false);
        }


    }
}
