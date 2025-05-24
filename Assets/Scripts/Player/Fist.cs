using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Player
{
    public class Fist : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private InputActionProperty gripAction = default;

        private readonly Vector3 normalSize = Vector3.one;
        private readonly Vector3 fistSize = new(1.8f, 1.8f, 1.8f);

        [Header("Runtime")]
        private SphereCollider sphereCollider = null;
        private Animator animator = null;

        public bool IsGripped { get; private set; } = false;

        private void Awake()
        {
            Initialise();
        }

        /// <summary>
        /// Initialises the animate hand
        /// </summary>
        private void Initialise()
        {
            animator = GetComponentInChildren<Animator>();
            sphereCollider = GetComponent<SphereCollider>();
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
                }
            }

            else
            {
                if (IsGripped)
                {
                    IsGripped = false;
                    sphereCollider.enabled = false;
                }
            }

            animator.transform.localScale = Vector3.Lerp(normalSize, fistSize, grip);
        }
    }
}