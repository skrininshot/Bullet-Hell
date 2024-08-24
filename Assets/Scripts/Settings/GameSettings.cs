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
        public PlayerStatesSettings PlayerStates;

        [Serializable]
        public class PlayerStatesSettings
        {
            public PlayerStateBullet.Settings BulletState;
            public PlayerStateAiming.Settings AimingState;
        }

        [Serializable]
        public class ControlSettings
        {
            public float Sensitivity = 1f;
            public float AimSensitivity = 0.5f;
        }
    }

    [Serializable]
    public class BulletSettings
    {
        public float DestroyTime = 5f;
        public float Speed = 5f;

        public HitSettings Hit;

        [Serializable]
        public class HitSettings
        {
            public AnimationsSettings Animations;

            [Serializable]
            public class AnimationsSettings
            {
                public BulletHitAnimationOnlyTimeShift.Settings OnlyTimeShift;
            }
        }            
    }

    [Serializable]
    public class UISettings
    {
        public GameUISettings Game;

        [Serializable]
        public class GameUISettings
        {
            public PauseView.Settings PauseView;
        }
    }
}