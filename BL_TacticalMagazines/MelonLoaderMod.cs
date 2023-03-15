using MelonLoader;
using BoneLib;
using UnityEngine;
using SLZ.Interaction;
using SLZ.Rig;
using TMPro;
using SLZ.Props.Weapons;
using System.Collections;
using SLZ.Marrow.Input;
using SLZ.Marrow.Data;
using static MelonLoader.MelonLogger;

namespace BL_TacticalMagazines
{
    public static class BuildInfo
    {
        public const string Name = "BL_TacticalMagazines"; // Name of the Mod.  (MUST BE SET)
        public const string Author = "L4rs"; // Author of the Mod.  (Set as null if none)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.0.0"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }

    public class BL_TacticalMagazines : MelonMod
    {
        private MagInfoUI leftHandInfo, rightHandInfo;
        private GameObject magInfoUIPrefab;

        private static TextPack currentTextPack = TextPacks.original;

        public static BL_TacticalMagazines instance;

        public override void OnInitializeMelon()
        {
            instance = this;

            MelonPreferences.CreateCategory(BuildInfo.Name);
            MelonPreferences.CreateEntry<Color>(BuildInfo.Name, "TextColor", Color.yellow);
            MelonPreferences.CreateEntry<bool>(BuildInfo.Name, "ShowExactValue", true);
            MelonPreferences.CreateEntry<bool>(BuildInfo.Name, "ColorLerp", true);
            MelonPreferences.CreateEntry<int>(BuildInfo.Name, "TextPack", 0);

            Hooking.OnGrabObject += Hooking_OnGrabObject;
            Hooking.OnReleaseObject += Hooking_OnReleaseObject;

            var bundle = AssetBundle.LoadFromFile(System.IO.Path.Combine(MelonUtils.UserDataDirectory, "maginfo.bundle"));
            magInfoUIPrefab = bundle.LoadAsset("Assets/MagInfoUI.prefab").Cast<GameObject>();
            magInfoUIPrefab.hideFlags = HideFlags.DontUnloadUnusedAsset;
        }

        public void OnRigManagerAwake()
        {
            leftHandInfo = rightHandInfo = null;
        }

        public override void OnLateInitializeMelon()
        {
            int index = MelonPreferences.GetEntryValue<int>(BuildInfo.Name, "TextPack");
            currentTextPack = TextPacks.textPacks[index];

            LoggerInstance.Msg($"Using TextPack {index}, {currentTextPack.almostFull}");
        }

        private void Hooking_OnGrabObject(GameObject obj, Hand hand)
        {
            if (obj.GetComponent<Magazine>() != null && Player.GetRigManager() != null)
            {
                if (hand.handedness == SLZ.Handedness.LEFT)
                {
                    if (leftHandInfo == null)
                        leftHandInfo = CreateMagInfoUI();

                    var mag = obj.GetComponent<Magazine>();
                    leftHandInfo.SetMag(mag);
                    leftHandInfo.SetHand(hand);

                    leftHandInfo.CancelCurrentTransition();
                    leftHandInfo.Show();
                }

                if (hand.handedness == SLZ.Handedness.RIGHT)
                {
                    if (rightHandInfo == null)
                        rightHandInfo = CreateMagInfoUI();

                    var mag = obj.GetComponent<Magazine>();
                    rightHandInfo.SetMag(mag);
                    rightHandInfo.SetHand(hand);

                    rightHandInfo.CancelCurrentTransition();
                    rightHandInfo.Show();
                }
            }
        }

        private void Hooking_OnReleaseObject(Hand hand)
        {
            if (hand.handedness == SLZ.Handedness.LEFT)
            {
                if (leftHandInfo != null)
                {
                    leftHandInfo.CancelCurrentTransition();
                    leftHandInfo.Hide();
                }
            }

            if (hand.handedness == SLZ.Handedness.RIGHT)
            {
                if (rightHandInfo != null)
                {
                    rightHandInfo.CancelCurrentTransition();
                    rightHandInfo.Hide();
                }
            }
        }

        private MagInfoUI CreateMagInfoUI()
        {
            if (magInfoUIPrefab != null)
            {
                var obj = GameObject.Instantiate(magInfoUIPrefab);
                var magInfo = obj.AddComponent<MagInfoUI>();

                magInfo.ammoInfoText = obj.transform.Find("AmmoInfoText").GetComponent<TextMeshPro>();
                magInfo.ammoExactText = obj.transform.Find("AmmoText").GetComponent<TextMeshPro>();

                return magInfo;
            }

            LoggerInstance.Error("magInfoUIPrefab is null");

            return null;
        }

        internal static string GetAmmoInfoText(float num)
        {
            string text = string.Empty;

            if (num > 0.95f)
            {
                //text = "Full";
                text = currentTextPack.full;
            }
            else if (num <= 0.95f && num > 0.85f)
            {
                //text = "Almost Full";
                text = currentTextPack.almostFull;
            }
            else if (num <= 0.85f && num > 0.6f)
            {
                //text = "More Than Half";
                text = currentTextPack.moreThanHalf;
            }
            else if (num <= 0.6f && num > 0.4f)
            {
                //text = "About Half";
                text = currentTextPack.aboutHalf;
            }
            else if (num <= 0.4f && num > 0.15f)
            {
                //text = "Less Than Half";
                text = currentTextPack.lessThanHalf;
            }
            else if (num <= 0.15f && num > 0f)
            {
                //text = "Almost Empty";
                text = currentTextPack.almostEmpty;
            }
            else if (num == 0f)
            {
                //text = "Empty";
                text = currentTextPack.empty;
            }

            return text;
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(RigManager))]
    [HarmonyLib.HarmonyPatch(nameof(RigManager.Awake))]
    public static class P_RigManager_Awake
    {
        public static void Postfix(RigManager __instance)
        {
            BL_TacticalMagazines.instance.OnRigManagerAwake();
        }
    }
}