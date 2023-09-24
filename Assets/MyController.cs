using UnityEngine;

public class MyController : MonoBehaviour
{
    public ControllerAction _controllerAction;
    private int overlappingCount = 0;
    private MeshRenderer _meshRenderer;
    
    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            var myHand = other.GetComponentInParent<MyHand>();
            overlappingCount++;
            myHand.SetCurrentController(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other))
        {
            if (overlappingCount > 0)
            {
                overlappingCount--;
            }else
            {
                other.GetComponentInParent<MyHand>().SetCurrentController(null);
            }
        }
    }

    private bool IsPlayer(Collider other)
    {
        return other.tag.Equals("Player");
    }
    public Color GetMaterialColor()
    {
        return _meshRenderer.material.color;
    }
}

public enum ControllerAction
{
    planetScale,
    slapForce,
    gravityForce,
    musicVolume,
    //worldScale,
}
