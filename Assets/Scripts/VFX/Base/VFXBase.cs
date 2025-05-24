using MyBox;
using System.Collections;
using UnityEngine;

namespace Core.VFX
{
    public class VFXBase : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private bool hasCloseDelay = true;
        [ConditionalField(nameof(hasCloseDelay))][SerializeField] private float closeDelay = 3.5f;

        private void Start()
        {
            if (hasCloseDelay)
                StartCoroutine(CloseCountdown());
        }

        /// <summary>
        /// Closes the VFX after close delay has been reached
        /// </summary>
        /// <returns></returns>
        private IEnumerator CloseCountdown()
        {
            yield return new WaitForSeconds(closeDelay);
            Close();
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