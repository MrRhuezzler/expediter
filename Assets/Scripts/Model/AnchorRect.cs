using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnchorRect : MonoBehaviour
{

    public string Name;

    private List<AnchorPoint> points = new List<AnchorPoint>();

    // Start is called before the first frame update
    public void Start()
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

    private Vector3 Trilerate(Vector3 p1, Vector3 p2, Vector3 p3, float d1, float d2, float d3)
    {
        Vector3 ex = (p2 - p1).normalized;
        float i = Vector3.Dot(ex, (p3 - p1));

        Vector3 a = p3 - p1 - (ex * i);
        Vector3 ey = a.normalized;
        Vector3 ez = Vector3.Cross(ex, ey);
        float d = (p2-p1).magnitude;
        float j = Vector3.Dot(ey, p3 - p1);

        float x = (Mathf.Pow(d1, 2) - Mathf.Pow(d2, 2) + Mathf.Pow(d, 2)) / (2 * d);
        float y = (Mathf.Pow(d1, 2) - Mathf.Pow(d3, 2) + Mathf.Pow(i, 2) + Mathf.Pow(j, 2)) / (2 * j) - (i / j) * x;
        float b = Mathf.Pow(d1, 2) - Mathf.Pow(x, 2) - Mathf.Pow(y, 2);

        if(Math.Abs(b) < 1e-8)
        {
            b = 0;
        }

        float z = Mathf.Sqrt(b);

        if (float.IsNaN(z))
        {
            //print(Vector3.zero);
            return Vector3.zero;
        }

        a = p1 + ((ex * x) + (ey * y));
        //Vector3 p4a = a + (ez * z);
        //Vector3 p4b = a - (ez * z);

        return a;

    }

    public Vector3 TrileratePoint(GeoCordinates point)
    {
        List<Vector3> locations = new(); 
        int MAX_ITERATIONS = 2;
        for(int i = 0; i < MAX_ITERATIONS; i++)
        {
            int a = (i + 0) % 4;
            int b = (i + 1) % 4;
            int c = (i + 2) % 4;

            float d1 = (float)points[a].worldCoordinates.DistanceTo(point);
            float d2 = (float)points[b].worldCoordinates.DistanceTo(point);
            float d3 = (float)points[c].worldCoordinates.DistanceTo(point);

            Vector3 p1 = points[a].gameObject.transform.position;
            Vector3 p2 = points[b].gameObject.transform.position;
            Vector3 p3 = points[c].gameObject.transform.position;

            locations.Add(Trilerate(p1, p2, p3, d1, d2, d3));
        }

        Vector3 averageLocation = Vector3.zero;
        foreach(Vector3 location in locations)
        {
            averageLocation += location;
        }

        return averageLocation / MAX_ITERATIONS;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
