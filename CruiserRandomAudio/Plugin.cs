using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine.PlayerLoop;

namespace CruiserRandomAudio;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    internal static Harmony? Harmony { get; set; }

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;

        Patch();

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }
    internal static void Patch()
    {
        Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

        Logger.LogInfo("Patching...");

        Harmony.PatchAll();

        Logger.LogInfo("Finished patching!");
    }

    private void OnDestroy()
    {

    }
}
