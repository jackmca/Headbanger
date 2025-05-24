using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Object
{
    public class ObjectBase : MonoBehaviour
    {
        /// <summary>
        /// Closes the object
        /// </summary>
        public virtual void Close()
        {
            Destroy(gameObject);
        }
    }
}