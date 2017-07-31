using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDump : MonoBehaviour {

    private Dictionary<string, int> dumbStupidDump;
    private static DataDump instance;

	void Awake () {
        dumbStupidDump = new Dictionary<string, int>();
        instance = this;
	}

    public static int GetInt(string key)
    {
        if (!instance)
        {
            instance = GameObject.Find("GameDirector").GetComponent<DataDump>();
        }
        if (!instance.dumbStupidDump.ContainsKey(key))
        {
            instance.dumbStupidDump.Add(key, 0);
        }
        return instance.dumbStupidDump[key];
    }

    public static void SetInt(string key, int value)
    {
        if (!instance)
        {
            instance = GameObject.Find("GameDirector").GetComponent<DataDump>();
        }
        if (!instance.dumbStupidDump.ContainsKey(key))
        {
            instance.dumbStupidDump.Add(key, value);
        }
        else
        {
            instance.dumbStupidDump[key] = value;
        }
    }
}
