﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPI311.GameEngine
{
    public class GameConstants
    {
        //camera constants
        public const float CameraHeight = 25000.0f;
        public const float PlayfieldSizeX = 19000f;
        public const float PlayfieldSizeY = 11000f;

        //asteroid constants
        public const int NumAsteroids = 10;
        public const int NumBullets = 20;
        public const float AsteroidMinSpeed = 100.0f;
        public const float AsteroidMaxSpeed = 300.0f;
        public const float AsteroidSpeedAdjustment = 5.0f;

        public const float AsteroidBoundingSphereScale = 0.95f;  //95% size
        public const float ShipBoundingSphereScale = 0.5f;  //50% size

        public const float BulletSpeedAdjustment = 100.0f;

        //scores
        public const int ShotPenalty = 10;
        public const int DeathPenalty = 100;
        public const int KillBonus = 50;

    }
}
