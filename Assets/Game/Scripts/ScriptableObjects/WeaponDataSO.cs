using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperlabCase.ScriptableObjects
{
    [CreateAssetMenu(fileName ="Weapon Data", menuName ="Scriptable Objects/Weapon Data")]
    public class WeaponDataSO : ScriptableObject
    {
        public int Level;
        public float FireRate;
        public float Damage;
    } 
}
