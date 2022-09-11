namespace Assets.SpaceModel.DangerSpaceObjects
{
    /// <summary>
    /// Корабль пришельцев. Назвал стереотипно НЛО.
    /// </summary>
    internal class NLO : DangerSpaceObject
    {
        public NLO(IModelLogger logger) : base(SpaceObjectType.nlo, logger)
        { }

        public override int GetScore()
        {
            return 5;
        }
    }
}
