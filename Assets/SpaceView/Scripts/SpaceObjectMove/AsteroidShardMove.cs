using UnityEngine;

namespace View
{
    public class AsteroidShardMove : AbstractMoveWithTarget
    {
        public override void Init(in SpaceObjectMoveInfo info)
        {
            base.Init(info);
            this.position = this.target.position;
            this.direction = (this.target.direction + new Vector2(Random.Range(-1, 1),Random.Range(-1, 1))).normalized;
            this.speed = 2.3f;
            this.type = Assets.SpaceModel.SpaceObjectType.asteroidShard;
        }
    }
}