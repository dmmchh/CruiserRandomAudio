using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine.PlayerLoop;
using System;
using GameNetcodeStuff;
using Mathf = Unity.Mathematics.math;
using UnityEngine;
using Unity.Mathematics;
using LethalNetworkAPI;

namespace CruiserRandomAudio.Patches;

[HarmonyPatch(typeof(VehicleController))]
[BepInDependency("LethalNetworkAPI")]
public class NewCruiser
{
    private static LethalNetworkVariable<int> CruiserMessage = new LethalNetworkVariable<int>("CruiserMessage") {Value = 0};

    private static int currentClip = 0;

    [HarmonyPrefix]
    [HarmonyPatch("ChangeRadioStation")]
    private static void cruiser(VehicleController __instance)
    {
        currentClip = __instance.currentRadioClip;

        //Plugin.Logger.LogInfo("Clip1:" + currentClip);
    }


    [HarmonyPatch("ChangeRadioStation")]
    [HarmonyPostfix]
    private static void RandomCruiserRadio(VehicleController __instance)
    {
        int randomRadio = UnityEngine.Random.Range(0, __instance.radioClips.Length);

        while (randomRadio == currentClip)
        {
            randomRadio = UnityEngine.Random.Range(0, __instance.radioClips.Length);
        }

        if (!__instance.radioOn)
            __instance.SetRadioOnLocalClient(true, false);

        //LethalNetworkVariable<int> customInt = __instance.GetNetworkVariable<int>("CruiserMessage");

        CruiserMessage.Value = randomRadio;

        __instance.currentRadioClip = CruiserMessage.Value;

        //Plugin.Logger.LogInfo(CruiserMessage.Value);

        __instance.radioAudio.clip = __instance.radioClips[__instance.currentRadioClip];
        __instance.radioAudio.time = Mathf.clamp(__instance.currentSongTime % __instance.radioAudio.clip.length, 0.01f, __instance.radioAudio.clip.length - 0.1f);
        __instance.radioAudio.Play();
        //Plugin.Logger.LogInfo("Clip2:" + __instance.currentRadioClip);

        Plugin.Logger.LogInfo(__instance.currentRadioClip);
    }
}