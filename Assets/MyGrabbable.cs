using UnityEngine;

public class MyGrabbable : MonoBehaviour
{
     //  public static MyGrabbable instance;
     //[SerializeField]
     private Rigidbody rb;
     private bool grabbed;
     private Transform previusParent;
     private Collider collider;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        //   instance = this;
    }

    public void StartGrabbed(Transform parent)
    {
        if (grabbed == false)
        {
            NewSoundManager.instance.PlaySound(NewSound.grab);
            grabbed = true;
            previusParent = transform.parent;
            transform.parent = parent;
            rb.useGravity = false;
            collider.isTrigger = true;
        }
    }
    public void EndGrabbed()
    {
        if (grabbed)
        {
            grabbed = false;
            NewSoundManager.instance.PlaySound(NewSound.drop);
            rb.useGravity = true;
            transform.parent = previusParent;
            collider.isTrigger = false;
        }
       
    }
    
}
