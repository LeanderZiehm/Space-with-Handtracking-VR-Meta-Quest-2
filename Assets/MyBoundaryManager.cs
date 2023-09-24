using System;
using UnityEngine;

public class MyBoundaryManager : Disposer
{
    public static MyBoundaryManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnOuterBoundary()
    {
        Vector3[] points = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.OuterBoundary);
        SpawnBoundary(points);
        var geometry = OVRManager.boundary.GetGeometry( OVRBoundary.BoundaryType.OuterBoundary );
        for( int i = 0; i < geometry.Length; i++ )
        {
            var sphere = GameObject.CreatePrimitive( PrimitiveType.Sphere );
            AddToDisposeList(sphere);
            sphere.transform.localScale = Vector3.one * 0.02f;
            sphere.GetComponent<Renderer>().material.color = Color.cyan;
            sphere.transform.position = geometry[i];
        }

    } 
    public void SpawnPlayAreaBoundary()
    {
        Vector3[] points = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);
        SpawnBoundary(points);
    }
    
    private void SpawnBoundary(Vector3[] points)
    {
        
        for (int i = 0; i < points.Length; i++)
        {
            GameObject g = GetGameObject();
            g.transform.position = points[i];
            g.transform.forward = points[i - 1] - points[i];
        }
    }
}