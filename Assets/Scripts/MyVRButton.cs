using UnityEngine;

public class MyVRButton : MonoBehaviour
{
    [SerializeField] private ButtonAction _buttonAction;
    public static MyVRButton lastSelectedButton;
    private MeshRenderer _meshRenderer;
    private Material defaultMaterial;
    private float startScale;
 
    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        defaultMaterial = _meshRenderer.material;
        startScale = transform.localScale.x;


       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            ImpactButton();
            OnClick(other);
        }
    }

    private void ImpactButton()
    {
        SetSelectMaterial();
        ScaleUp();
    }
    private void SetSelectMaterial()
    {
        ReturnToOldState();
        _meshRenderer.material = References.instance.buttonSelectedMaterial;//set selected material
    }

    private void ScaleUp()
    {
        transform.localScale = Vector3.one*startScale*1.5f;
    }
    
    private void ScaleBack()
    {
        transform.localScale = Vector3.one*startScale;
    }

    protected void ReturnToOldState()
    {
        if (lastSelectedButton != null)
        {
            lastSelectedButton._meshRenderer.material = lastSelectedButton.defaultMaterial;//give back material
            lastSelectedButton.ScaleBack();
        }
        lastSelectedButton = this;
    }

    
    
    
    private void OnClick(Collider other)
    {
        // if (_buttonAction == ButtonAction.exit)
        // {
        //     Application.Quit();
        // }else if (_buttonAction == ButtonAction.twoStationaryGravityCristals)
        // {
        //     // References.instance.gravityBalls[0].SetActive(true);
        //     // References.instance.gravityBalls[1].SetActive(true);
        // }
        // else 
        // {
        //     // References.instance.gravityBalls[0].SetActive(false);
        //     // References.instance.gravityBalls[1].SetActive(false);
        //     //
        //     if (_buttonAction == ButtonAction.draw)
        //     {
        //         // References.instance.drawBalls[0].gameObject.SetActive(true);
        //         // References.instance.drawBalls[1].gameObject.SetActive(true);
        //         
        //         
        //     }
        //   
        // }
        
        if (_buttonAction == ButtonAction.draw)
        {

            if (GetComponent<Animator>().enabled)
            {
                GetComponent<Animator>().enabled = false;
                MyHand.CreateArrowsForBothHands();
                References.instance.planetButton.SetActive(true);
            }
          
        }
        
        MyHand._buttonAction = _buttonAction;
    }
}

public enum ButtonAction
{
    nothing,
    exit,
    spawnStatonaryPlanet,
    draw,
    spawnPlanet,
    spawnDoesNotGetPulled,
    twoStationaryGravityCristals,
    spawnDoesNotPull,
    countingMode,
    gravityHands,
    pinchMode,
    restart
}
