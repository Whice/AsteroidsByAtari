using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Пулл оъектов для боевых объектов.
/// </summary>
public class BattlePrefabPool
{
    private BattlePrefabProvider provider;

    ///Место в hierarchy для неактивных объектов.
    private Transform nonActiveObjectPlace ;
    public BattlePrefabPool(BattlePrefabProvider provider, Transform nonActiveObjectPlace)
    {
        this.provider = provider;
        this.nonActiveObjectPlace = nonActiveObjectPlace;
    }

    private Dictionary<Int32, Stack<GameObject>> pool = new Dictionary<Int32, Stack<GameObject>>();
    /// <summary>
    /// Получить боевой объект.
    /// </summary>
    /// <param name="typePrefab">Тип SpaceObjectType</param>
    /// <returns></returns>
    public GameObject GetBattlePrefab(Int32 typePrefab)
    {
        GameObject prefab = null;
        if (this.pool.ContainsKey(typePrefab))
        {
            if (this.pool[typePrefab].Count > 0)
            {
                prefab = this.pool[typePrefab].Pop();
            }
        }
        if(prefab == null)
            prefab = this.provider.GetPrefabClone(typePrefab);

        prefab.SetActive(true);
        return prefab;
    }
    /// <summary>
    /// Вернуть боевой объект в пулл.
    /// </summary>
    /// <param name="typePrefab"></param>
    /// <param name="prefab"></param>
    public void PushBattlePrefab(Int32 typePrefab, GameObject prefab)
    {
        prefab.SetActive(false);
        prefab.transform.SetParent(this.nonActiveObjectPlace, false);
        if (this.pool.ContainsKey(typePrefab))
        {
            this.pool[typePrefab].Push(prefab);
        }
        else
        {
            Stack<GameObject> stack = new Stack<GameObject>();
            stack.Push(prefab);
            this.pool.Add(typePrefab, stack);
        }
    }
}
