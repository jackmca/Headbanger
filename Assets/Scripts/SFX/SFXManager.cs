using Core.Singleton;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core.SFX
{
    public class SFXManager : SingletonBase<SFXManager>
    {
        /// <summary>
        /// The class for storing the key and value for sounds
        /// </summary>
        [Serializable]
        public class ClipKVP
        {
            public string Key;
            public AudioClip Value;
        }

        /// <summary>
        /// The bank for storing all audio
        /// </summary>
        [Serializable]
        public class AudioBank
        {
            [SerializeField] private ClipKVP[] kvps;
            private readonly Dictionary<string, AudioClip> dictionary = new();

            public bool IsBuilt { get; private set; } = false;

            /// <summary>
            /// Valiates the sounds
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
            /// Builds the sounds
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
            /// Tries to get audio
            /// </summary>
            /// <param name="key">The audio key</param>
            /// <param name="audio">The out audio clip</param>
            /// <returns>Returns success status of request</returns>
            public bool TryGetAudio(string key, out AudioClip audio)
            {
                return dictionary.TryGetValue(key, out audio);
            }
        }

#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(AudioBank))]
        public class AudioBankDrawer : PropertyDrawer
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

        [CustomPropertyDrawer(typeof(ClipKVP))]
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
        [SerializeField] private Sound soundPrefab = null;

        [SerializeField] private AudioBank soundBank;

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
            Instance.soundBank.Build();
        }

        /// <summary>
        /// Plays a sound clip from array of clips
        /// </summary>
        /// <param name="clips">The sound clip names</param>
        /// <param name="position">The position to play the sound at</param>
        public static void Play(string[] clips, Vector3? position = null)
        {
            string clip = GetRandomClip(clips);
            Play(clip, position);
        }

        /// <summary>
        /// Plays a sound clip
        /// </summary>
        /// <param name="clip">The sound clip name</param>
        /// <param name="position">The position to play the sound at</param>
        public static void Play(string clip, Vector3? position = null)
        {
            if (Instance.soundBank.TryGetAudio(clip, out AudioClip audioClip))
            {
                Sound sound = Instantiate(Instance.soundPrefab);

                Vector3 pos;
                if (position.HasValue) pos = position.Value;
                else pos = Camera.main.transform.position;

                sound.Initialise(pos, audioClip);
            }

            else
            {
                Debug.LogWarning($"[SFX] AudioClip '{clip}' not present in audio bank");
            }
        }

        /// <summary>
        /// Gets a random clip
        /// </summary>
        /// <param name="clips">The clips to choose from</param>
        /// <returns>Returns a random clip</returns>
        private static string GetRandomClip(string[] clips)
        {
            int index = Random.Range(0, clips.Length);
            return clips[index];
        }
    }
}