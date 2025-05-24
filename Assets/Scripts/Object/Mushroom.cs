using Core.SFX;
using Core.Utils;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Object
{
    public class Mushroom : ObjectBase
    {
        [Header("Setup")]
        [SerializeField] private Transform neckTrans = null;

        private readonly float enlargeDuration = 0.1f;
        private readonly float shrinkDuration = 0.2f;
        private readonly float enlargeFactor = 1.35f; 

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(TagUtils.FistTag))
            {
                PlayRandomHitClip();
                DOTween.Sequence()
                    .Append(neckTrans.DOScale(Vector3.one * enlargeFactor, enlargeDuration))
                    .Append(neckTrans.DOScale(Vector3.one, shrinkDuration));
            }
        }

        #region SFX
        /// <summary>
        /// Plays a random hit clip
        /// </summary>
        private void PlayRandomHitClip()
        {
            string[] hitClips = new string[3]
            {
                "mushroom_hit_01",
                "mushroom_hit_02",
                "mushroom_hit_03"
            };
            int selection = Random.Range(0, hitClips.Length);
            SFXManager.Play(hitClips[selection], neckTrans.transform.position);
        }
        #endregion
    }
}