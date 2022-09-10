using System;
using System.Collections.Generic;

namespace Assets.SpaceModel.Extensions
{
    /// <summary>
    /// Хранитель пулов объектов, которые много раз пересоздаются.
    /// </summary>
    public class SpaceObjectsPoolKeeper
    {
        public SpaceObjectsPoolKeeper()
        {
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
        private SpaceObjectsFactory factory = new SpaceObjectsFactory();
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
            if (this.pools[type].Count > 0)
            {
                return this.pools[type].Pop();
            }
            else
            {
                return CreateSpaceObjects(type);
            }
        }

        #endregion Внешние методы.
    }
}
