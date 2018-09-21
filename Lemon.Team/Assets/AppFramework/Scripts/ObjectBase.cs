using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ObjectBase : MonoBehaviour
{
    public Dictionary<int, object> _Pars = new Dictionary<int, object>();
    protected virtual void Init()
    {
    }
    protected virtual void Free()
    {

    }
    public void ExcuteInit()
    {
        Init();
    }
    public void ExcuteFree()
    {
        Free();
        _Pars.Clear();
    }
    public void SetPar(int key, object val)
    {
        _Pars.Add(key, val);
    }
    public T GetPar<T>(int key)
    {
        if (!_Pars.ContainsKey(key))
            return default(T);
        return (T)_Pars[key];
    }
}
