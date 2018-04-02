using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonHelper
{
    public static T getJsonArray<T>(string json)
    {

        T box = JsonUtility.FromJson<T>(json);
        return box;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}


[System.Serializable]
public class Box
{
	public string id;
	public string x;
	public string y;
    public string z;
}
