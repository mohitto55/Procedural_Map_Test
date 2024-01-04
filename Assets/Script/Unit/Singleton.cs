using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Singleton<T> where T : class, new()
{
    // Start is called before the first frame update
    public static T Instance
    {
        get {
            if (Singleton<T>.instance == null)
                Singleton<T>.instance = Activator.CreateInstance<T>();
            return instance; }
    }
    static T instance;
}
