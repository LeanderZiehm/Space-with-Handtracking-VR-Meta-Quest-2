
using UnityEngine;

public abstract class MyHandBase : MonoBehaviour
{
    protected OVRHand _ovrHand;
    protected OVRSkeleton _skeleton;
    public bool isRightHand;
    protected bool[] fingersPinched = new bool[5];
    public int handIndex;

   protected void AwakeBase()
    {
        _ovrHand = GetComponent<OVRHand>();
        _skeleton = GetComponent<OVRSkeleton>();
        isRightHand = _skeleton.GetSkeletonType() == OVRSkeleton.SkeletonType.HandRight;
        handIndex = (isRightHand) ? 0 : 1;
    }

   protected void UpdateBase()
    {
        for (int i = 0; i < fingersPinched.Length; i++)
        {
            SetPinchedFinger(i, _ovrHand.GetFingerIsPinching((OVRHand.HandFinger)i));
        }
    }
    
    private void SetPinchedFinger(int index, bool newBool)
    {
        if (fingersPinched[index] == newBool) return;

        fingersPinched[index] = newBool;
        Finger pinchedFinger = (Finger)index;
        if (newBool)
        {
            OnPinchStart(pinchedFinger);
            onPinchStart?.Invoke(pinchedFinger,isRightHand);
        }
        else
        {
            OnPinchEnd(pinchedFinger);
            onPinchEnd?.Invoke(pinchedFinger,isRightHand);
        }
    }
    
     public delegate void OnPinchChange(Finger pinchedFinger,bool isRightHand);
     public static event OnPinchChange onPinchStart;
     public static event OnPinchChange onPinchEnd;

    protected abstract void OnPinchEnd(Finger pinchedFinger);

    protected abstract void OnPinchStart(Finger pinchedFinger);


    public Transform GetBoneTransform(int boneIndex,bool wantFingertip = false)
    {
        if (wantFingertip)
        {
            boneIndex += 19;
        }
        return _skeleton.Bones[boneIndex].Transform;
    }
    public Vector3 GetBonePos(int boneIndex)
    {
        return GetBoneTransform(boneIndex).position;
    }

    public bool IsPinched(Finger finger)
    {
        return _ovrHand.GetFingerIsPinching((OVRHand.HandFinger)finger);
    }
    
    public enum Finger
    {
        Thumb = 0,
        Index = 1,
        Middle = 2,
        Ring = 3,
        Pinky = 4
    }
}
