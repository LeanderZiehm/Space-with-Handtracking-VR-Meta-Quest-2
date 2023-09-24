using System;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;
public class MyGrabHand : MyHandBase
{
     MyReferences references;
     protected SphereCollider[] fingerTriggers = new SphereCollider[5];

     [SerializeField] private float lazerDistance;
     [SerializeField] private float forwardAmount;
     [SerializeField] private float upAmount;

    // [SerializeField] Vector3 lazerDirection = new Vector3();
     //  private Dictionary<Finger,List<MyGrabbable>> grabbedObjects = new Dictionary<Finger,List<MyGrabbable>>();

    // private Transform[] handColliders;
    void Awake()
    {
        AwakeBase();
    }

    private void Start()
    {
        
        references = MyReferences.instance;
        if (isRightHand)
        {
         //   references.DebugConsole.parent = GetBoneTransform(0);
        }
        else
        {
            // handIndex = 1;
        }
        
        for (int i = 0; i < fingerTriggers.Length; i++)
        {
            Instantiate(references.fingerTriggerPrefab).GetComponent<MyFingerTrigger>().Setup(this,(Finger)i);
           //grabbedObjects[(Finger)i] = new List<MyGrabbable>();
           //fingerTriggers[i] = Instantiate(references.fingerTriggerPrefab,GetBoneTransform(i,true)).GetComponent<SphereCollider>();
        }
        
        MyLine.MakeLine(handIndex);
    }

    private void Update()
    {
        UpdateBase();

        var handCenter = GetBonePos(0);
        MyLine.UpdateLinePosition(handIndex,handCenter,handCenter +(transform.forward*forwardAmount +transform.up*upAmount)*lazerDistance);
    }

    protected override void OnPinchStart(Finger pinchedFinger)//this gets called twice once for pointy and thumb
    {
        
   //      Collider currentfingerCollider = fingerTriggers[(int)pinchedFinger];
   //
   //      RaycastHit hit;
   //      Ray ray = new Ray(Vector3.zero, currentfingerCollider.transform.position);
   //      currentfingerCollider.Raycast(ray, out hit, 100);
   //     
   // //     bool intersectsWithGrabable = false;
   //      MyGrabbable grabbedObject = hit.collider.GetComponent<MyGrabbable>();
   //      if (grabbedObject == null)//intersectsWithGrabable 
   //      {
   //          grabbedObject.StartGrabbed(currentfingerCollider.transform);
   //          grabbedObjects[pinchedFinger].Add(grabbedObject);
   //      }
    }
    protected override void OnPinchEnd(Finger pinchedFinger)
    {
        // for (int i = 0; i <  grabbedObjects[pinchedFinger].Count; i++)
        // {
        //     grabbedObjects[pinchedFinger][i].EndGrabbed();
        // }
        // grabbedObjects[pinchedFinger].Clear();
        //
    }

 


    
}
