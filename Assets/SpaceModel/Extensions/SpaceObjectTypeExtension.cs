using System;

namespace Assets.SpaceModel.Extensions
{
    /// <summary>
    /// Методы для типа космического объекта.
    /// </summary>
    public static class SpaceObjectTypeExtension
    {
        /// <summary>
        /// Возможно ил создание более одного экземпляра объекта.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Boolean IsMultyplyInstance(this SpaceObjectType type)
        {
            switch (type)
            {
                case SpaceObjectType.nlo: return false;
                case SpaceObjectType.player: return false;
                case SpaceObjectType.laser: return false;

                case SpaceObjectType.simpleBullet: return true;
                case SpaceObjectType.bigAsteroid: return true;
                case SpaceObjectType.asteroidShard: return true;

                default: return false;
            }
        }
        /// <summary>
        /// Опасен ли объект этого типа игроку.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Boolean IsDangerObjectType(this SpaceObjectType type)
        {
            Int32 intType = (Int32)type;
            switch (intType)
            {
                case (Int32)SpaceObjectType.nlo: return true;
                case (Int32)SpaceObjectType.player: return false;
                case (Int32)SpaceObjectType.laser: return false;

                case (Int32)SpaceObjectType.simpleBullet: return false;
                case (Int32)SpaceObjectType.bigAsteroid: return true;
                case (Int32)SpaceObjectType.asteroidShard: return true;

                default: return false;
            }
        }
    }
}
