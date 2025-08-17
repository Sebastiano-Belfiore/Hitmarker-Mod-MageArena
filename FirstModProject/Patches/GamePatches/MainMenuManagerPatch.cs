using HitMarkerMod.Utils;
using HarmonyLib;

namespace HitMarkerMod.Patches.GamePatches
{
    [HarmonyPatch(typeof(MainMenuManager), "ActuallyStartGameActually")]
    public static class MainMenuManagerPatch
    {
        static void Postfix()
        {
            LoggerUtils.LogInfo("MainMenuManager", "Game started, initializing hitmarker system");

            if (Mod.Instance?.DefaultHitmarkerTexture == null)
            {
                LoggerUtils.LogError("MainMenuManagerPatch", "Cannot initialize: missing mod instance or texture");
                return;
            }

            try
            {
                Mod.Instance.StartCoroutine(Mod.Instance.InitializeHitmarkerCoroutine(Mod.Instance.DefaultHitmarkerTexture));
                LoggerUtils.LogInfo("MainMenuManager", "Hitmarker initialization started");
            }
            catch (System.Exception ex)
            {
                LoggerUtils.LogCriticalError("MainMenuManagerPatch", "Failed to start hitmarker initialization", ex);
            }
        }
    }
}