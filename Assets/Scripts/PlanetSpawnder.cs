using System.Collections.Generic;
using UnityEngine;


public class PlanetSpawnder : MonoBehaviour
{
    public static PlanetSpawnder instance;

    [SerializeField] private GameObject planetPrefab;
   // [SerializeField] private Mesh[] planetMeshs;
    [SerializeField] private Material[] planetMaterials;
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private Stack<GameObject> readyToSpawn = new Stack<GameObject>();

    private void Awake()
    {
        instance = this;
    }
    private Planet CreatePlanet(Vector3 transformPosition, float spawnSize)
    {
        GameObject spawnedPlanet;
        if (readyToSpawn.Count == 0)
        {
            spawnedPlanet = Instantiate(planetPrefab, transformPosition, Quaternion.identity);
        }
        else
        {
            spawnedPlanet = readyToSpawn.Pop();
            spawnedPlanet.SetActive(true);
            spawnedPlanet.transform.position = transformPosition;
        }
        spawnedObjects.Add(spawnedPlanet);
        var planet = spawnedPlanet.GetComponent<Planet>();
        planet.SetSize(spawnSize);
        return planet;
    }

    public void SpawnStatonary(Vector3 transformPosition, float spawnSize)
    {
        Planet spawnedPlanet = CreatePlanet(transformPosition,spawnSize);
        spawnedPlanet.MakeStationary();
        spawnedPlanet.GetComponent<MeshRenderer>().material = planetMaterials[1];
    }

    public void SpawnPlanet(Vector3 transformPosition, Vector3 direction, float spawnSize) //
    {
        Planet spawnedPlanet = CreatePlanet(transformPosition,spawnSize);
        spawnedPlanet.SetStartDirection(direction);
        spawnedPlanet.GetComponent<MeshRenderer>().material = planetMaterials[0];
    }
    
    
    
    

    
    public void DisableAll()
    {
        foreach (var spawnedObject in spawnedObjects)
        {
            spawnedObject.SetActive(false);
            spawnedObject.GetComponent<Planet>().MakeMovable();
            readyToSpawn.Push(spawnedObject);
        }
        spawnedObjects = new List<GameObject>();
    }
    
    
    
    
    //OLD
    public void SpawnDoesNotGetPulled(Vector3 transformPosition, Vector3 direction, float spawnSize) //
    {
        Planet spawnedPlanet = CreatePlanet(transformPosition,spawnSize);
        spawnedPlanet.SetStartDirection(direction);
        spawnedPlanet.RemoveFromGettingPulled();
    }
    public void SpawnDoesNotPull(Vector3 transformPosition, Vector3 direction, float spawnSize) //
    {
        Planet spawnedPlanet = CreatePlanet(transformPosition,spawnSize);
        spawnedPlanet.SetStartDirection(direction);
        spawnedPlanet.DontPullOtherPlanets();
    }
}