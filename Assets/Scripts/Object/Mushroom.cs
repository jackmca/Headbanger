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
                DOTween.Sequence()
                    .Append(neckTrans.DOScale(Vector3.one * enlargeFactor, enlargeDuration))
                    .Append(neckTrans.DOScale(Vector3.one, shrinkDuration));
            }
        }
    }
}