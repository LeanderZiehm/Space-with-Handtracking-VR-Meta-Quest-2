using System;
using UnityEngine;

public class MyReferences : MonoBehaviour
{
  //  public Transform DebugConsole;
    public GameObject fingerTriggerPrefab;
    public static MyReferences instance;
    private void Awake()
    {
        instance = this;
    }
}
