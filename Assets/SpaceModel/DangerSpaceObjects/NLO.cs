namespace Assets.SpaceModel.DangerSpaceObjects
{
    /// <summary>
    /// Корабль пришельцев. Назвал стереотипно НЛО.
    /// </summary>
    public class NLO : DangerSpaceObject
    {
        public NLO(IModelLogger logger) : base(SpaceObjectType.nlo, logger)
        { }
    }
}
