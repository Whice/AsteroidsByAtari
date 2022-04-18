using System;

namespace Assets.SpaceModel.DangerSpaceObjects
{
    public class DangerSpaceObject:SpaceObject
    {
        public DangerSpaceObject(SpaceObjectType type) : base(type) { }

        public override Boolean CollideWithObject(SpaceObject spaceObject)
        {
            SpaceObjectType type = spaceObject.type;
            if (type == SpaceObjectType.simpleBullet || type == SpaceObjectType.laser)
            {
                --this.hp;
                --spaceObject.hp;
            }

            return false;
        }
    }
}
