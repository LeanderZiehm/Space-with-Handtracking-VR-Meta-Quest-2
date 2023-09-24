using System;
using UnityEngine;

public class SizeDisplay : MonoBehaviour
{
    private void Update()
    {
        transform.localScale = Vector3.one * MyHand.planetSize;
    }
}
