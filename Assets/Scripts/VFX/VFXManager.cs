using Core.Singleton;
using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core.VFX
{
    public class VFXManager : SingletonBase<VFXManager>
    {
        /// <summary>
        /// The class for storing the key and value for effects
        /// </summary>
        [Serializable]
        public class EffectKVP
        {
            public string Key;
            public VFXBase Value;
        }

        /// <summary>
        /// The bank for storing all effect
        /// </summary>
        [Serializable]
        public class EffectBank
        {
            [SerializeField] private EffectKVP[] kvps;
            private readonly Dictionary<string, VFXBase> dictionary = new();

            public bool IsBuilt { get; private set; } = false;

            /// <summary>
            /// Validates the effects
            /// </summary>
            /// <returns></returns>
            public bool Validate()
            {
                if (kvps.Length == 0) return false;

                List<string> keys = new();
                foreach (var kvp in kvps)
                {
                    if (keys.Contains(kvp.Key)) return false;
                    keys.Add(kvp.Key);
                }
                return true;
            }

            /// <summary>
            /// Builds the effects
            /// </summary>
            public void Build()
            {
                if (!IsBuilt)
                {
                    if (Validate())
                    {
                        for (int i = 0; i < kvps.Length; i++)
                        {
                            dictionary.Add(kvps[i].Key, kvps[i].Value);
                        }
                    }

                    IsBuilt = true;
                }
            }

            /// <summary>
            /// Tries to get effect
            /// </summary>
            /// <param name="key">The effect key</param>
            /// <param name="effect">The out effect</param>
            /// <returns>Returns success status of request</returns>
            public bool TryGetEffect(string key, out VFXBase effect)
            {
                return dictionary.TryGetValue(key, out effect);
            }
        }

#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(EffectBank))]
        public class EffectBankDrawer : PropertyDrawer
        {
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("kvps"));
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                EditorGUI.BeginProperty(position, label, property);
                EditorGUI.PropertyField(position, property.FindPropertyRelative("kvps"), label, true);
                EditorGUI.EndProperty();
            }
        }

        [CustomPropertyDrawer(typeof(EffectKVP))]
        public class BankKVPDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {

                EditorGUI.BeginProperty(position, label, property);

                Rect rect1 = new(position.x, position.y, position.width / 2 - 4, position.height);
                Rect rect2 = new(position.center.x + 2, position.y, position.width / 2 - 4, position.height);

                EditorGUI.PropertyField(rect1, property.FindPropertyRelative("Key"), GUIContent.none);
                EditorGUI.PropertyField(rect2, property.FindPropertyRelative("Value"), GUIContent.none);

                EditorGUI.EndProperty();
            }
        }
#endif

        [Header("Prefabs")]
        [SerializeField] private EffectBank effectBank;

        protected override void Awake()
        {
            base.Awake();
            Initialise();
        }

        /// <summary>
        /// Initialises the SFX manager
        /// </summary>
        private static void Initialise()
        {
            Instance.effectBank.Build();
        }

        /// <summary>
        /// Creates a particle
        /// </summary>
        /// <param name="particle">The particle key</param>
        /// <param name="position">The position to create at</param>
        /// <param name="rotation">The rotation to create at</param>
        /// <param name="parent">The parent for the particle, optional</param>
        public static void CreateParticle(string particle, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (Instance.effectBank.TryGetEffect(particle, out VFXBase vfx))
            {
                if (parent != null) Instantiate(vfx, position, rotation, parent);
                else Instantiate(vfx, position, rotation);
            }

            else
            {
                Debug.LogWarning($"[VFX] Particle '{particle}' not present in particle bank");
            }
        }
    }
}