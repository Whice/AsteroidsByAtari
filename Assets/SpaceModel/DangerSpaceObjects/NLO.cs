namespace Assets.SpaceModel.DangerSpaceObjects
{
    /// <summary>
    /// Корабль пришельцев. Назвал стереотипно НЛО.
    /// </summary>
    internal class NLO : DangerSpaceObject
    {
        public NLO(IModelLogger logger) : base(SpaceObjectType.nlo, logger)
        { }
        public override void SetMaxHP()
        {
            this.hp = 1;
        }
        public override int GetScore()
        {
            return this.isNeedGetScore ? 5 : 0;
        }
    }
}
