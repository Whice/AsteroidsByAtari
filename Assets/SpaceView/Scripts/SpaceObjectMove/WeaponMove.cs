namespace View
{
    public abstract class WeaponMove: AbstractMoveWithTarget
    {
        public override void Init(in SpaceObjectMoveInfo info)
        {
            base.Init(info);
            this.position = this.target.position + this.target.direction * this.target.spaceObjectTransform.localScale;
            this.direction = this.target.direction;
            this.spaceObjectTransform.rotation = this.target.rotation;
        }
    }
}
