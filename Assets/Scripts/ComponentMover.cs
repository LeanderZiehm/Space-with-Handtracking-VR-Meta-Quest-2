using System;
using System.Collections.Generic;
using UnityEngine;

public class ComponentMover : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] List<Component> moveUpComponents;
    
    [ContextMenu( "MoveListComponentsUp" )]
    public void MoveListComponentsUp()
    {
        foreach (var component in moveUpComponents)
        {
            for (int i = 0; i < 10; i++)
            {
                UnityEditorInternal.ComponentUtility.MoveComponentUp(component); 
            }
        }

    }

    private void OnValidate()
    {
        MoveListComponentsUp();
    }
#endif
}
