using System;

namespace Assets.SpaceModel.DangerSpaceObjects
{
    internal class BigAsteroid : DangerSpaceObject
    {
        public BigAsteroid() : base(SpaceObjectType.bigAsteroid) { }

        public override Boolean CollideWithObject(SpaceObject spaceObject)
        {
            Boolean result = base.CollideWithObject(spaceObject);

            //При попадании пули в большой астероид,
            //он должен расколоться на маленькие.
            if(spaceObject.type == SpaceObjectType.simpleBullet)
            {
                result = true;
            }

            return result;
        }
    }
}
