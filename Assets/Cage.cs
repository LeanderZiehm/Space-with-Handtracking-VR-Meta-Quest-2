using UnityEngine;

public class Cage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Planet p = other.GetComponent<Planet>();
        if (p != null)
        {
            p.DestroyPlanet();
        }
       
    }
}
