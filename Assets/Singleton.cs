using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    static readonly Singleton _instance = new Singleton();
    public static Singleton Instance
    {
        get
        {
            return _instance;
        }
    }
}
