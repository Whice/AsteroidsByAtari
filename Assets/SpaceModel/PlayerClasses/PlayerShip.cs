using System;

namespace Assets.SpaceModel.PlayerClasses
{
    /// <summary>
    /// Модельный класс игрока. 
    /// <br/>Обозван кораблем игрока, т.к. в основном будет
    /// восприниматься именно в качестве космического корабля.
    /// </summary>
    public class PlayerShip : SpaceObject
    {
        #region Singleton

        private PlayerShip() :base(SpaceObjectType.player) { }
        private static PlayerShip instancePrivate = null;
        public static PlayerShip instance
        {
            get
            {
                if (instancePrivate == null)
                {
                    instancePrivate = new PlayerShip();
                }

                return instancePrivate;
            }
        }

        #endregion Singleton

        /// <summary>
        /// Счет игрока.
        /// </summary>
        public Int64 scorePrivate = 0;
        /// <summary>
        /// Счет игрока. Он может только увеличиваться.
        /// </summary>
        public Int64 score
        {
            get
            {
                return this.scorePrivate;
            }
            set
            {
                if (value > this.scorePrivate)
                {
                    this.scorePrivate = value;
                }
            }
        }


        public override Boolean CollideWithObject(SpaceObject spaceObject)
        {
            SpaceObjectType type = spaceObject.type;
            Boolean dangerType =
                type == SpaceObjectType.nlo ||
                type == SpaceObjectType.asteroidShard ||
                type == SpaceObjectType.bigAsteroid;

            //при столкновении с опасным объектом игрок проигрывает,
            //а значит надо выполнить соответсвующие действия.
            if(dangerType)
            {
                return true;
            }

            return false;
        }
    }
}
