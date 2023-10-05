using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Planet : MonoBehaviour
{
    [SerializeField] float miltiplayer = 1f;
   // [SerializeField] bool randomizeSize = true;
   // private float g = 0.1f;
    //private float startVelocity = 1;
    private float minification = 0.1f;
    private Rigidbody myRb;
    public static List<Rigidbody> rigidbodies = new List<Rigidbody>();

    private Transform transform;
    [NonSerialized]
    public float spawnedTime;

    private bool shouldPull = true;
    void Awake()
    {
        transform = base.transform;
        myRb = GetComponent<Rigidbody>();
        myRb.velocity = Vector3.zero;
        spawnedTime = Time.timeSinceLevelLoad;
    }

    private void OnEnable()
    {
        AddToGettingPulled();
    }

    public void AddToGettingPulled()
    {
        rigidbodies.Add(myRb);
    }
    public void RemoveFromGettingPulled()
    {
        rigidbodies.Remove(myRb);
    }

    private void OnDisable()
    {
       RemoveFromGettingPulled();
    }
    void FixedUpdate()
    {
        if (shouldPull)
        {
            Pull();
        }
       
    }

    public void MakeStationary()
    {
        myRb.constraints = RigidbodyConstraints.FreezeAll;
    }
    public void MakeMovable()
    {
        myRb.constraints = RigidbodyConstraints.None;
    }

    public void RandomizeMassAndSize()
    {
        var randMass = 1f;//Random.Range(0.5f, 3);
        SetMassAndSize(randMass);
    }

    private void SetMassAndSize(float value)
    {
        value *= minification;
        myRb.mass = value;
        transform.localScale = value * Vector3.one;
        GetComponent<TrailRenderer>().startWidth = transform.localScale.x;
    }
    private void Pull()
    {
        foreach (var rb in rigidbodies)
        {
            if (rb == myRb) continue;
            Vector3 distVec = transform.position - rb.transform.position;
            var distance = distVec.magnitude;
            if (distance == 0) return;

            float gravityForceMagnitude = miltiplayer*References.instance.gravitationalForce* (myRb.mass * rb.mass) / (distance * distance);
            Vector3 gravityForce = distVec * gravityForceMagnitude;
            rb.AddForce(gravityForce);
        }
    }

    public void SetStartDirection(Vector3 transformPosition)
    {
        myRb.velocity =transformPosition;//*startVelocity
    }

    public void SetSize(float spawnSize)
    {

       // References.instance.text2.text = spawnSize.ToString();
       // float s;
        if (spawnSize == 0)//float.TryParse(References.instance.text.text, out s)
        {
            RandomizeMassAndSize();
        }
        else
        {
            SetMassAndSize(spawnSize);
        }
    }

    public void GetSlapped(Vector3 slapPos)
    {
        RemoveFromGettingPulled();
        var dir = transform.position - slapPos;
        float s;
        myRb.velocity = dir*References.instance.slapForce;
       Invoke(nameof(AddToGettingPulled),1f);
    }
    
    public void DontPullOtherPlanets()
    {
        shouldPull = false;
    }

    public void DestroyPlanet()
    {
        RemoveFromGettingPulled();
        Destroy(gameObject);
    }
}