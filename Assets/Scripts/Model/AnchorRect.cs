using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorRect : MonoBehaviour
{

    public string Name;

    private List<AnchorPoint> points = new List<AnchorPoint>();

    // Start is called before the first frame update
    void Start()
    {
        points.Clear();
        foreach(AnchorPoint child in GetComponentsInChildren<AnchorPoint>())
        {
            points.Add(child);
        }
        points.Sort();
    }

    public bool IsInsideRect(GeoCordinates point)
    {
        GeoCordinates bl = points[3].worldCoordinates;
        GeoCordinates tr = points[1].worldCoordinates;
        return GeoCordinates.IsInsideRect(bl, tr, point);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
