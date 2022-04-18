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
        /// <summary>
        /// Счет игрока.
        /// </summary>
        public Int64 score = 0;
        public PlayerShip() : base(SpaceObjectType.player) { }

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
