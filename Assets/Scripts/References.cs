using System;
using TMPro;
using UnityEngine;

public class References : MonoBehaviour
{
    public TextMeshProUGUI text,text2;
  //  public Transform gravityBall;
    //public Transform[] balls;

    public Transform countBall;
 //   public Transform[] fingerBalls;
   
    public GameObject fingerColliderPrefab;
    public GameObject sizeDisplay;
 //   public Transform drawBall1, drawBall2;
 
    public Material buttonSelectedMaterial;
    [NonSerialized]
    public float slapForce = 10f,gravitationalForce = 0.01f;
    public static References instance;
    public Transform gravityBall;
    
    public Transform[] drawBalls;
    public GameObject[] gravityBalls;
    public GameObject[] gravityHandBalls;
    public Material gravityHandMaterial,defaultHandMaterial;


    // public ButtonAction buttonAction;
   // public ControllerAction controllerAction;
    private void Awake()
    {
        instance = this;
    }

   
}
