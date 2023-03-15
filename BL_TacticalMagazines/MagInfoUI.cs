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
using Il2CppSystem;

namespace BL_TacticalMagazines
{
    [RegisterTypeInIl2Cpp]
    public class MagInfoUI : MonoBehaviour
    {
        public MagInfoUI(System.IntPtr ptr) : base(ptr)
        {
        }

        public TextMeshPro ammoExactText;
        public TextMeshPro ammoInfoText;

        private Hand hand;
        private float originalFontSize = 0.2f;
        private float originalFontSizeInfo = 0.3f;

        private bool doColorLerp = false;
        private bool showExactValue = true;

        private object currentTransition;

        public void Awake()
        {
            Color textColor = MelonPreferences.GetEntryValue<Color>(BuildInfo.Name, "TextColor");
            showExactValue = MelonPreferences.GetEntryValue<bool>(BuildInfo.Name, "ShowExactValue");
            doColorLerp = MelonPreferences.GetEntryValue<bool>(BuildInfo.Name, "ColorLerp");

            ammoInfoText.color = textColor;
        }

        public void Show()
        {
            ammoExactText.enabled = true;
            ammoInfoText.enabled = true;
            currentTransition = MelonCoroutines.Start(Co_ScaleTransition(false, 0.3f));
        }

        public void Hide()
        {
            currentTransition = MelonCoroutines.Start(Co_ScaleTransition(true, 0.3f));
        }

        public void CancelCurrentTransition()
        {
            if (currentTransition != null)
                MelonCoroutines.Stop(currentTransition);
        }

        private IEnumerator Co_ScaleTransition(bool hide, float duration)
        {
            float timeElapsed = 0;
            do
            {
                timeElapsed += Time.deltaTime;
                float normalizedTime = timeElapsed / duration;

                if (hide)
                {
                    ammoExactText.fontSize = Mathf.Lerp(ammoExactText.fontSize, 0f, Easing.Cubic.In(normalizedTime));
                    ammoInfoText.fontSize = Mathf.Lerp(ammoInfoText.fontSize, 0f, Easing.Cubic.In(normalizedTime));
                }
                else
                {
                    ammoExactText.fontSize = Mathf.Lerp(ammoExactText.fontSize, originalFontSize, Easing.Cubic.Out(normalizedTime));
                    ammoInfoText.fontSize = Mathf.Lerp(ammoInfoText.fontSize, originalFontSizeInfo, Easing.Cubic.Out(normalizedTime));
                }

                yield return null;
            }
            while (timeElapsed < duration);

            ammoExactText.enabled = !hide;
            ammoInfoText.enabled = !hide;
        }

        public void Update()
        {
            if (hand != null)
            {
                transform.position = hand.m_CurrentAttachedGO.transform.position + Vector3.up * 0.15f;
                transform.rotation = Quaternion.LookRotation(transform.position - Player.GetPlayerHead().transform.position);
            }
        }

        public void SetMag(Magazine magazine)
        {
            if (magazine != null)
            {
                float num = (float)magazine.magazineState.AmmoCount / (float)magazine.magazineState.magazineData.rounds;
                ammoInfoText.text = BL_TacticalMagazines.GetAmmoInfoText(num);
                ammoExactText.text = magazine.magazineState.AmmoCount.ToString() + "/" + magazine.magazineState.magazineData.rounds;

                if (doColorLerp)
                    ammoInfoText.color = Color.Lerp(Color.red, Color.green, num);
            }
        }

        public void SetHand(Hand hand)
        {
            if (hand != null)
            {
                this.hand = hand;
            }
        }
    }
}