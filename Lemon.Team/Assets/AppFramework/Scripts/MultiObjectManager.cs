using UnityEngine;
using System.Collections;
using PathologicalGames;
using System.Linq;

public class MultiObjectManager
{
    static MultiObjectManager()
    {
        SpawnPool spawnPool=PoolManager.Pools["PoolManager"];
        Transform poolTemplate= spawnPool.transform.Find("PoolTemplate");
        for(int i=0;i<poolTemplate.childCount;i++)
        {
            CreatePrefabPool(spawnPool, poolTemplate.GetChild(i), 20);
        }
    }

    public static Transform Spawn(string objName)
    {
        if (!PoolManager.Pools["PoolManager"].prefabs.ContainsKey(objName))
            return null;
        return PoolManager.Pools["PoolManager"].Spawn(objName);
    }

    public static void Despawn(Transform obj)
    {
        obj.GetComponent<RectTransform>().SetParent(PoolManager.Pools["PoolManager"].gameObject.transform);
        PoolManager.Pools["PoolManager"].Despawn(obj);
    }

    static void CreatePrefabPool(SpawnPool spawnPool, Transform prefab, int num)
    {
        if (spawnPool._perPrefabPoolOptions.Any(c => c.prefab.name == prefab.name))
            return;
        PrefabPool refabPool = new PrefabPool(prefab);
        refabPool.preloadTime = false;
        refabPool.preloadFrames = 2;
        refabPool.preloadDelay = 1.0f;
        //默认初始化两个Prefab
        refabPool.preloadAmount = num;
        //开启限制
        refabPool.limitInstances = true;
        //关闭无限取Prefab
        refabPool.limitFIFO = true;
        //限制池子里最大的Prefab数量
        refabPool.limitAmount = num * 10;
        //开启自动清理池子
        refabPool.cullDespawned = false;
        //最终保留
        refabPool.cullAbove = 0;
        //多久清理一次
        refabPool.cullDelay = 5;
        //每次清理几个
        refabPool.cullMaxPerPass = num;
        spawnPool._perPrefabPoolOptions.Add(refabPool);
        spawnPool.CreatePrefabPool(refabPool);
    }
}
