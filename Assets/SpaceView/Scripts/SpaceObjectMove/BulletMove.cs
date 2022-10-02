using UnityEngine;

namespace View
{
    public class BulletMove : WeaponMove
    {
        public override void Init(in SpaceObjectMoveInfo info)
        {
            base.Init(info);
            this.speed = 11;
            this.type = Assets.SpaceModel.SpaceObjectType.simpleBullet;
        }
    }
}