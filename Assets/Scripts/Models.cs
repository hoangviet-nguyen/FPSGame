using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Models : MonoBehaviour
{
    #region -WeaponModel-

    public enum WeaponFireType
    {
        SemiAuto,
        FullyAuto,
        Pistols
    }
    [Serializable]
    public class WeaponModel
    {
        [Header("Sway")] 
        public float SwayAmount;
        public float SwaySmoothing;
        public float SwayResetSmooting;
        public float SwayClampX;
        public float SwayClampY;
    }
    #endregion
}
