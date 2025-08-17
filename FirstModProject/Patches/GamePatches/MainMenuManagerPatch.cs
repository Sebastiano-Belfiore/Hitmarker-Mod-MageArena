using FirstModProject.Utils;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstModProject.Patches.GamePatches
{
    [HarmonyPatch(typeof(MainMenuManager), "ActuallyStartGameActually")]
    public static class MainMenuManagerPatch
    {
        static void Postfix()
       
        {
            LoggerUtils.LogPatch("MainMenuManager", "Game started, initializing hitmarker system");
            if (Mod.Instance == null)
            {
                LoggerUtils.LogError("MainMenuManagerPatch", "Mod instance is null");
                return;
            }
            if (Mod.Instance.DefaultHitmarkerTexture == null)
            {
                LoggerUtils.LogError("MainMenuManagerPatch", "Default hitmarker texture is null");
                return;
            }
            Mod.Instance.StartCoroutine(Mod.Instance.InitializeHitmarkerCoroutine(Mod.Instance.DefaultHitmarkerTexture));

            LoggerUtils.LogPatch("MainMenuManager", "Hitmarker initialization coroutine started");
        }
    }
}
