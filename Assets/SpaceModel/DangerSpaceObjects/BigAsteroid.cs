using System;

namespace SpaceModel.DangerSpaceObjects
{
    internal class BigAsteroid<Vector>:SpaceObject<Vector>
    {
        public BigAsteroid(Vector direction, Vector position) : base(direction, position) { }
    }
}
