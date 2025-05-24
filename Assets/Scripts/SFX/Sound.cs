using UnityEngine;

namespace Core.SFX
{
    public class Sound : MonoBehaviour
    {
        [Header("Setup")]
        protected AudioSource audioSource = null;

        /// <summary>
        /// Initialises the sound
        /// </summary>
        /// <param name="pos">The position to start at</param>
        /// <param name="clip">The clip to play</param>
        public void Initialise(Vector3 pos, AudioClip clip)
        {
            audioSource = GetComponent<AudioSource>();

            transform.position = pos;
            audioSource.clip = clip;
            audioSource.Play();
        }

        private void Update()
        {
            if (audioSource.timeSamples == audioSource.clip.samples || audioSource.isPlaying == false)
            {
                Close();
            }
        }

        /// <summary>
        /// Closes the VFX base
        /// </summary>
        public virtual void Close()
        {
            Destroy(gameObject);
        }
    }
}