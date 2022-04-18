using Assets.SpaceModel.DangerSpaceObjects;
using Assets.SpaceModel.PlayerClasses;

namespace Assets.SpaceModel.Extensions
{
    /// <summary>
    /// Фабрика для создания космических объектов.
    /// </summary>
    public class SpaceObjectsFactory : ModelLogger
    {
        /// <summary>
        /// Создать космический объект по типу.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public SpaceObject CreateSpaceObjects(SpaceObjectType type)
        {
            switch (type)
            {
                case SpaceObjectType.nlo: return new NLO();
                case SpaceObjectType.asteroidShard: return new AsteroidShard();
                case SpaceObjectType.bigAsteroid: return new BigAsteroid();

                case SpaceObjectType.player: return PlayerShip.instance;
                case SpaceObjectType.laser: return new Laser(PlayerShip.instance);
                case SpaceObjectType.simpleBullet: return new Bullet(PlayerShip.instance);

                default:
                    {
                        ErrorMessage("In method " + nameof(CreateSpaceObjects) 
                            + " was provided invalid " + nameof(SpaceObjectType) + "!");
                        return null;
                    }
            }
        }
    }
}
