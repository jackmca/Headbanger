using Core.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Core.Player
{
    public class PlayerManager : SingletonBase<PlayerManager>
    {
        [Header("Setup")]
        [SerializeField] private ActionBasedController leftHand = null;
        [SerializeField] private ActionBasedController rightHand = null;

        [SerializeField] private Fist leftFist = null;
        [SerializeField] private Fist rightFist = null;

        public static ActionBasedController LeftHand { get { return Instance.leftHand; } }
        public static ActionBasedController RightHand { get { return Instance.rightHand; } }

        /// <summary>
        /// Triggers haptic feedback
        /// </summary>
        /// <param name="intensity">The intensity of the haptic</param>
        /// <param name="duration">The duration of the haptics</param>
        /// <param name="isLeft">Should trigger on left, else is right</param>
        public static void TriggerHaptics(float intensity, float duration, bool isLeft)
        {
            if (isLeft) LeftHand.SendHapticImpulse(intensity, duration);
            else RightHand.SendHapticImpulse(intensity, duration);
        }
    }
}