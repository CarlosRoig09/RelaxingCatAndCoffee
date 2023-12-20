using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace personalLibrary
{
    public static class EnumLibrary
    {
        public enum TypeOfEvent
        {
            AttackFinish,
            AddACofee,
            StopCofeeProduction
        }

        public enum Inputs
        {
            OnScroll,
            OnScrollCancel,
            OnRightClick,
            OnLeftClick,
        }

        public enum PunType
        {
            Positive,
            Negative,
            Cero
        }

        public enum CatForceState
        {
            Expand,
            Destroy
        }
    }
}
