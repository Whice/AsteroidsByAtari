using UnityEngine;

namespace View
{
    public class LazerMove : WeaponMove
    {
        public override void Init(in SpaceObjectMoveInfo info)
        {
            base.Init(info);
            this.speed = 0;
            this.type = Assets.SpaceModel.SpaceObjectType.laser;
        }
    }
}