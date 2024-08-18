using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    public PlayerSetttings Player;
    public CameraZoom.Settings CameraZoom;
    public Bullet.Settings Bullet;

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
}