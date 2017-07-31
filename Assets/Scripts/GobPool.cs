using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobPool : MonoBehaviour {

    [System.NonSerialized]
    private static GobPool instance;
    private Dictionary<string, Stack<GameObject>> pool;


    void Start()
    {
        if (instance != null) Clear();
        instance = this;
        pool = new Dictionary<string, Stack<GameObject>>();
    }

	public static GameObject Instantiate(GameObject o)
    {
        if (instance == null) instance = GameObject.Find("GameDirector").GetComponent<GobPool>();
        GameObject newObject = instance.pool.ContainsKey(o.tag) && instance.pool[o.tag].Count > 0 ?
            instance.pool[o.tag].Pop() :
            GameObject.Instantiate(o);
        newObject.SetActive(true);
        return newObject;
    }

    public static void Destroy(GameObject o)
    {
        if (instance == null) instance = GameObject.Find("GameDirector").GetComponent<GobPool>();
        if (!instance.pool.ContainsKey(o.tag))
        {
            instance.pool.Add(o.tag, new Stack<GameObject>());
        }
        instance.pool[o.tag].Push(o);
        o.SetActive(false);
    }

    public static void Clear()
    {
        if (instance == null) return;
        foreach (var p in instance.pool.Values) {
            while (p.Count > 0)
            {
                Destroy(p.Pop());
            }
        }
        instance.pool.Clear();
        instance = null;
    }

    public static void ClearPool(string pool)
    {
        if (instance == null) return;

        var p = instance.pool[pool];
        while (p.Count > 0)
        {
            Destroy(p.Pop());
        }
        instance.pool.Clear();
    }
}
