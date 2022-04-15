using System;

namespace SpaceModel.DangerSpaceObjects
{
    internal class AsteroidShard<Vector> : SpaceObject<Vector>
    {
        public AsteroidShard(Vector direction, Vector position) : base(direction, position) { }
    }
}
