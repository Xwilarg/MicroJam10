using System;
using UnityEngine;

namespace MicroJam10.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerInfo", fileName = "PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        [Range(0f, 10f)]
        [Tooltip("Speed of the player")]
        public float ForceMultiplier = 1f;

        [Tooltip("Gravity multiplier to make the player fall")]
        public float GravityMultiplicator;
    }
}