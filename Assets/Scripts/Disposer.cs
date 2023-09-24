using System.Collections.Generic;
using UnityEngine;

public class Disposer : MonoBehaviour
{
    public GameObject prefab;
    private List<GameObject> gameObjectsToDispose = new List<GameObject>();

    private Stack<GameObject> objectsReadyForUse = new Stack<GameObject>();

    // public void AddGameObjectToDispose(GameObject obj)
    // {
    //     gameObjectsToDispose.Add(obj);
    // }
    public void DisposeAllIstantiatedGameObjects()
    {
        foreach (GameObject obj in gameObjectsToDispose)
        {
            obj.SetActive(false);
            objectsReadyForUse.Push(obj);
        }
    }

    public GameObject GetGameObject()
    {
        GameObject g;
        if (objectsReadyForUse.Count == 0)
        {   g = Instantiate(prefab);
            gameObjectsToDispose.Add(g);
            return g;
        }
        else
        {
            g = objectsReadyForUse.Pop();
            g.SetActive(true);
            return g;
        }
    }
    protected void AddToDisposeList(GameObject obj)
    {
        gameObjectsToDispose.Add(obj);
    }
}