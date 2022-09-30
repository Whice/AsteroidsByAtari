﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Пулл оъектов для боевых объектов.
/// </summary>
public class BattlePrefabPool
{
    private BattlePrefabProvider provider;
    public BattlePrefabPool(BattlePrefabProvider provider)
    {
        this.provider = provider;
    }

    private Dictionary<Int32, List<GameObject>> pool = new Dictionary<Int32, List<GameObject>>();
    private Int32 GetLastIndex(List<GameObject> list)
    {
        return list.Count - 1;
    }
    /// <summary>
    /// Получить боевой объект.
    /// </summary>
    /// <param name="typePrefab"></param>
    /// <returns></returns>
    public GameObject GetBattlePrefab(Int32 typePrefab)
    {
        GameObject prefab = null;
        if (this.pool.ContainsKey(typePrefab))
        {
            if (this.pool[typePrefab].Count > 0)
            {
                List<GameObject> prefabs = this.pool[typePrefab];
                Int32 lastIndex = GetLastIndex(prefabs);
                prefab = prefabs[lastIndex];
                prefabs.RemoveAt(lastIndex);
            }
        }

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
        this.pool[typePrefab].Add(prefab);
    }
}
