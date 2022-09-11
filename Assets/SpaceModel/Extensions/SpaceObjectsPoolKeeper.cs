using Assets.SpaceModel.Extensions;
using System;
using System.Collections.Generic;

namespace Assets.SpaceModel
{
    /// <summary>
    /// Хранитель пулов объектов, которые много раз пересоздаются.
    /// </summary>
    internal class SpaceObjectsPoolKeeper
    {
        private IModelLogger logger;
        public SpaceObjectsPoolKeeper(IModelLogger logger)
        {
            this.logger = logger;
            this.factory = new SpaceObjectsFactory(logger);

            //Создать сразу по 10
            SpaceObjectType type = SpaceObjectType.end;
            Int32 endTypes = (Int32)type;

            for (Int32 i = 0; i < endTypes; i++)
            {
                type = (SpaceObjectType)i;
                if (type.IsMultyplyInstance())
                {
                    PushManyObjects(type, 10);
                }
            }
        }

        #region Данные.

        /// <summary>
        /// Фабрика для создания космических объектов.
        /// </summary>
        private SpaceObjectsFactory factory;
        /// <summary>
        /// Словарь стеков объектов.
        /// В словаре каждый стек - пул объектов одного типа.
        /// </summary>
        private Dictionary<SpaceObjectType, Stack<SpaceObject>> pools = new Dictionary<SpaceObjectType, Stack<SpaceObject>>();

        #endregion Данные.

        #region Внутренние методы.

        /// <summary>
        /// Создать объект заданного типа через фабрику.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private SpaceObject CreateSpaceObjects(SpaceObjectType type)
        {
            return this.factory.CreateSpaceObjects(type);
        }
        /// <summary>
        /// Заполнить сразу много объектов заданного типа.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="count"></param>
        private void PushManyObjects(SpaceObjectType type, Int32 count)
        {
            for (Int32 i = 0; i < count; i++)
            {
                Push(CreateSpaceObjects(type));
            }
        }

        #endregion Внутренние методы.

        #region Внешние методы.

        /// <summary>
        /// Выданные объекты, которые могут иметь только один экземляр.
        /// </summary>
        private HashSet<SpaceObjectType> issuedSingleInstanceObjects = new HashSet<SpaceObjectType>();
        /// <summary>
        /// Добавить в пул объект.
        /// </summary>
        /// <param name="spaceObject"></param>
        public void Push(SpaceObject spaceObject)
        {
            this.pools[spaceObject.type].Push(spaceObject);
        }
        /// <summary>
        /// Вытащить из пула объект заданного типа.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public SpaceObject Pop(SpaceObjectType type)
        {
            SpaceObject objectForPop=null;
            if (this.pools[type].Count > 0)
            {
                objectForPop= this.pools[type].Pop();
            }
            else
            {
                objectForPop = CreateSpaceObjects(type);
            }

            objectForPop.OnDestroed += Push;
            return objectForPop;
        }

        #endregion Внешние методы.
    }
}
