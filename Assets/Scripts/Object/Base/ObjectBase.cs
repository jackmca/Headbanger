using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Object
{
    public class ObjectBase : MonoBehaviour
    {
        protected virtual void Awake()
        {
            Initialise();
        }

        /// <summary>
        /// Initialises the animate hand
        /// </summary>
        protected virtual void Initialise()
        {

        }

        /// <summary>
        /// Closes the object
        /// </summary>
        public virtual void Close()
        {
            Destroy(gameObject);
        }
    }
}