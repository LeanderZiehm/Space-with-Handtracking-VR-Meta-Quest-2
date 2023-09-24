using System;
using System.Collections.Generic;
using UnityEngine;

public class MyFingerTrigger : MonoBehaviour
{
     private MyGrabHand grabHand;
     private MyHandBase.Finger finger;
     private SphereCollider sphereCollider;
     private List<MyGrabbable> grabbed = new List<MyGrabbable>();
     private List<MyGrabbable> imInside = new List<MyGrabbable>();
     
 public void Setup(MyGrabHand myGrabHand, MyHandBase.Finger finger1)
 {
     grabHand = myGrabHand;
     finger = finger1;
     transform.parent = grabHand.GetBoneTransform((int)finger,true);
     transform.localPosition = Vector3.zero;
     MyHandBase.onPinchStart += OnPinchStart;
     MyHandBase.onPinchEnd += OnPinchEnd;

     //   endGrab = myGraber.isRightHand;
 }
 private void OnPinchStart(MyHandBase.Finger pinchedfinger, bool isrighthand)
 {
     if (grabHand.isRightHand == isrighthand && finger == pinchedfinger)
     {
         foreach (var i in imInside)
         {
             i.StartGrabbed(transform);
             if (!grabbed.Contains(i))
             {
                 grabbed.Add(i);
             }
         }
        
     }
 }
 private void OnPinchEnd(MyHandBase.Finger pinchedfinger, bool isrighthand)
 {
     if (grabHand.isRightHand == isrighthand && finger == pinchedfinger)
     {
         foreach (var g in grabbed)
         {
             g.EndGrabbed();
         }
         grabbed.Clear();
     }
 }
 
 private void OnTriggerEnter(Collider other)
 {
     var collidedWithGrabbable = other.GetComponent<MyGrabbable>();
     if (collidedWithGrabbable)
     {
         if (!imInside.Contains(collidedWithGrabbable))
         {
             imInside.Add(collidedWithGrabbable);
         }
        
     }
   
 }
 private void OnTriggerExit(Collider other)
 {
     var collidedWithGrabbable = other.GetComponent<MyGrabbable>();
     if (collidedWithGrabbable)
     {
         if (imInside.Contains(collidedWithGrabbable))
         {
             imInside.Remove(collidedWithGrabbable);
         }
     }
 }
 
 
 
 // private void OnTriggerStay(Collider other)
 //    {
 //        var collidedWithGrabbable = other.GetComponent<MyGrabbable>();
 //        if (collidedWithGrabbable)
 //        {
 //            if (graber.IsPinched(finger))
 //            {
 //                if (!grabbed.Contains(collidedWithGrabbable))
 //                {
 //                    grabbed.Add(collidedWithGrabbable);
 //                    collidedWithGrabbable.StartGrabbed(transform);
 //                }
 //            }
 //
 //        }
 //        
 //    }
 //    
    




}
