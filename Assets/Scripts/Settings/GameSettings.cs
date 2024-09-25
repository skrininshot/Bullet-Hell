using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    public UISettings UI;
    public PlayerSetttings Player;
    public CameraZoom.Settings CameraZoom;
    public BulletSettings Bullet;
    public TimeShifter.Settings TimeShifter;

    [Serializable]
    public class PlayerSetttings
    {
        public ControlSettings Control;
        public PlayerStatesSettings States;

        [Serializable]
        public class PlayerStatesSettings
        {
            public PlayerStateAiming.Settings AimingState;
        }

        [Serializable]
        public class ControlSettings
        {
            public float Sensitivity = 0.15f;
            public float AimSensitivity = 0.5f;
        }
    }

    [Serializable]
    public class BulletSettings
    {
        public float Speed = 75f;

        public BulletController.Settings Controller;

        public HitSettings Hit;

        public AirFlowSettings AirFlow;

        [Serializable]
        public class HitSettings
        {
            public AnimationsSettings Animations;

            [Serializable]
            public class AnimationsSettings
            {
                public BulletAnimationController.Settings Controller;
                public BulletHitAnimationOnlyTimeShift.Settings OnlyTimeShift;
                public BulletHitAnimationOrbit.Settings Orbit;
            }
        }

        [Serializable]
        public class AirFlowSettings
        {
            public int MaxCount = 10;
            public float Frequency = 0.001f;
            public float SinMultiply = 720f;
            public AirFlowEffect.Settings Effect;
        }
    }

    [Serializable]
    public class UISettings
    {
        public GameUISettings Game;

        [Serializable]
        public class GameUISettings
        {
            public FloatingText.Settings FloatingText;
            public PauseBaseView.BaseSettings PauseBaseView;
            public AwardView.Settings AwardView;
        }
    }
}
