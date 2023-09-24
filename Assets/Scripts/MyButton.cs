using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public abstract class MyButton : MonoBehaviour
{


      private Button button;


    private Button GetButton()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }
            return button;
        
    }


    void OnEnable()
    {
         GetButton().onClick.AddListener( OnClick);
    }

    void OnDisable()
    {
         GetButton().onClick.RemoveListener(OnClick);
    }

    protected abstract void OnClick();

}
