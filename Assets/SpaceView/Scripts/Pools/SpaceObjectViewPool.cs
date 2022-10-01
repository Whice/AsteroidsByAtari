using Assets.SpaceModel;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class SpaceObjectViewPool
    {
        /// <summary>
        /// Шаблон объекта, по которому будут созданы прочие.
        /// </summary>
        private SpaceObjectView templateObject;
        /// <summary>
        /// Пулл объектов для переиспользования.
        /// </summary>
        private Stack<SpaceObjectView> pool = new Stack<SpaceObjectView>(100);
        public SpaceObjectViewPool(SpaceObjectView templateObject)
        {
            this.templateObject = templateObject;
        }

        /// <summary>
        /// Получить космический объект.
        /// </summary>
        /// <param name="typePrefab"></param>
        /// <returns></returns>
        public SpaceObjectView GetSpaceObjectView(SpaceObject spaceObject, Borders battleFieldborders)
        {
            SpaceObjectView soView = null;
            if (pool.Count > 0)
            {
                soView = this.pool.Pop();
            }
            else
            {
                soView = GameObject.Instantiate(this.templateObject);
            }

            soView.Initialize(spaceObject, battleFieldborders);
            soView.InitPool(this);
            soView.gameObject.SetActive(true);
            return soView;
        }
        /// <summary>
        /// Вернуть космический объект в пулл.
        /// </summary>
        public void PushSpaceObjectView(SpaceObjectView sObject)
        {
            sObject.gameObject.SetActive(false);
            this.pool.Push(sObject);
        }
    }
}