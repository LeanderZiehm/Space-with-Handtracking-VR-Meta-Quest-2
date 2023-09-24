using System.Collections.Generic;
using UnityEngine;

public static class MyLine
{
    private static Dictionary<int, LineRenderer> lineDict = new Dictionary<int, LineRenderer>();
    private static Stack<LineRenderer> lineStack = new Stack<LineRenderer>();
    private static Material lineMaterial;
    private static Color defaultColor = Color.white;

    public static void SetColor(Color color)
    {
        defaultColor = color;
    }

    public static void MakeLine(int lineID, bool hasGradient = true, float startWidth = -1, float endWidth = -1,Transform parentTransform = null)
    {
        if (lineDict.ContainsKey(lineID))
        {
            DisposeLine(lineID);
        }
        lineDict[lineID] = InstantiateLine(defaultColor,  hasGradient,startWidth, endWidth,parentTransform);
    }

    public static void MakeLine(int lineID, Color color, bool hasGradient = true, float startWidth = -1, float endWidth = -1,Transform parentTransform = null)
    {
        if (lineDict.ContainsKey(lineID))
        {
            DisposeLine(lineID);
        }
        lineDict[lineID] = InstantiateLine(color,  hasGradient,startWidth, endWidth,parentTransform);
    }

    public static void UpdateLinePosition(int lineID, Vector3 start, Vector3 end) //add color
    {
        LineRenderer lr;
        if (!lineDict.ContainsKey(lineID))
        {
            lr = InstantiateLine();
            lineDict[lineID] = lr;
        }
        else
        {
            lr = lineDict[lineID];
        }

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    public static LineRenderer InstantiateLine() //Color color,
    {
        return InstantiateLine(defaultColor);
    }

    public static LineRenderer InstantiateLine(Color color, bool hasGradient = true, float startWidth = -1, float endWidth = -1,Transform parentTransform = null)
    {
        LineRenderer lr = GetLine();
        lr.transform.parent = parentTransform; //(parentTransform == null) ? defaultParentTransform :
        lr.material = GetMaterial();

        startWidth = (startWidth == -1) ? 0.05f : startWidth;
        if (hasGradient)
        {
            Color transparent = color;
            transparent.a = 0;
            lr.SetColors(color, transparent);
            endWidth = (endWidth == -1) ? startWidth * 2 : endWidth;
        }
        else
        {
            lr.SetColors(color, color);
            endWidth = (endWidth == -1) ? startWidth : endWidth;
        }

        lr.SetWidth(startWidth, endWidth);
        return lr;
    }

    public static void DisposeLine(int index)
    {
        LineRenderer lineRenderer = lineDict[index];
        lineDict.Remove(index);
        lineRenderer.gameObject.SetActive(false);
        lineStack.Push(lineRenderer);
    }


    private static LineRenderer GetLine()
    {
        if (lineStack.Count == 0)
        {
            GameObject myLine = new GameObject("line");
            myLine.AddComponent<LineRenderer>();
            return myLine.GetComponent<LineRenderer>();
        }
        else
        {
            LineRenderer lr = lineStack.Pop();
            lr.gameObject.SetActive(true);
            return lr;
        }
    }

    private static Material GetMaterial()
    {
        return (lineMaterial == null) ? new Material(Shader.Find("UI/Default")) : lineMaterial;
    }
    
}