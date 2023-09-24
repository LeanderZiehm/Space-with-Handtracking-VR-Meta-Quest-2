using System;
using UnityEngine;

public class HandCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
     Planet planet =   collision.gameObject.GetComponent<Planet>();
        if (planet != null)
        {
           // var lifeTimeOfPlanet = Time.timeSinceLevelLoad - planet.spawnedTime;
            //if (lifeTimeOfPlanet > 1f)//wait a time before can get slapped
            //{
                planet.RemoveFromGettingPulled();
            //}

          //  References.instance.text2.text = lifeTimeOfPlanet.ToString();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        Planet planet =   collision.gameObject.GetComponent<Planet>();
        if (planet != null)
        {
            planet.AddToGettingPulled();
        }
    }
}
