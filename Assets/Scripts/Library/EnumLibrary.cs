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
            StopCofeeProduction,
            EmergencyState
        }

        public enum ButtonType
        {
            A,
            D,
            Enter,
            Shift,
            Esc
        }

        public enum Inputs
        {
            OnScroll,
            OnScrollCancel,
            OnRightClick,
            OnLeftClick,
            OnEscClick
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
        public enum Scene
        {
            GameMenu,
            LastPlayed,
            Ranking,
            ChooseName,
            CinematicScreen,
            GameScreen,
            GameOverScreen
        }
    }
}
