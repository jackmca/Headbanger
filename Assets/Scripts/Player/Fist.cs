using Core.SFX;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Player
{
    public class Fist : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private bool isLeft = false;
        [SerializeField] private InputActionProperty gripAction = default;

        [SerializeField] private Color32 fistColour = default;

        private readonly Vector3 normalSize = Vector3.one;
        private readonly Vector3 fistSize = new(1.8f, 1.8f, 1.8f);

        private readonly float hapticIntensity = 0.8f;
        private readonly float hapticDuration = 0.25f;

        [Header("Runtime")]
        private SphereCollider sphereCollider = null;
        private Animator animator = null;
        private SkinnedMeshRenderer skinnedMeshRenderer = null;
        private TrailRenderer trailRenderer = null;

        public bool IsGripped { get; private set; } = false;

        private Color32 skinColour = default;

        private void Awake()
        {
            Initialise();
        }

        /// <summary>
        /// Initialises the animate hand
        /// </summary>
        private void Initialise()
        {
            sphereCollider = GetComponent<SphereCollider>();
            animator = GetComponentInChildren<Animator>();
            skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            trailRenderer = GetComponentInChildren<TrailRenderer>();

            skinColour = skinnedMeshRenderer.material.color;
        }

        private void Update()
        {
            float grip = gripAction.action.ReadValue<float>();
            animator.SetFloat("Grip", grip);

            if (grip >= 1f)
            {
                if (!IsGripped)
                {
                    IsGripped = true;
                    sphereCollider.enabled = true;
                    trailRenderer.gameObject.SetActive(true);
                    SFXManager.Play("fist_activate_01", gameObject.transform.position);
                }
            }

            else
            {
                if (IsGripped)
                {
                    IsGripped = false;
                    sphereCollider.enabled = false;
                    trailRenderer.gameObject.SetActive(false);
                    SFXManager.Play("fist_deactivate_01", gameObject.transform.position);
                }
            }

            animator.transform.localScale = Vector3.Lerp(normalSize, fistSize, grip);
            skinnedMeshRenderer.material.color = Color32.Lerp(skinColour, fistColour, grip);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (IsGripped)
            {
                PlayerManager.TriggerHaptics(hapticIntensity, hapticDuration, isLeft);
            }
        }
    }
}