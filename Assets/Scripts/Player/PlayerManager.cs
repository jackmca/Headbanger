using Core.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Player
{
    public class PlayerManager : SingletonBase<PlayerManager>
    {
        [Header("Setup")]
        [SerializeField] private Fist leftFist = null;
        [SerializeField] private Fist rightFist = null;

    }
}