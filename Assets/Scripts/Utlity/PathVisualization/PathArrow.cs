using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(NavigationController))]
public class PathArrow : MonoBehaviour
{

    [SerializeField]
    private GameObject pathParent;

    [SerializeField]
    private GameObject arrowPrefab;

    private NavigationController controller;
    private readonly List<GameObject> arrows = new List<GameObject>();

    private float timer;

    [SerializeField]
    private float waitTimer = 1.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        ClearArrowObjects();
        controller = GetComponent<NavigationController>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > waitTimer)
        {
            ClearArrowObjects();
            CalculateInterpolatedPoints();
            Time.timeScale = 1.0f;
            timer -= waitTimer;
        }
    }

    private void ClearArrowObjects()
    {
        foreach(GameObject arrow in arrows)
        {
            Destroy(arrow);
        }
        arrows.Clear();
    }
    private Vector3 LerpPoint(float t, Vector3 start, Vector3 end)
    {
        return start + (end - start) * t;
    }

    private void CalculateInterpolatedPoints()
    {
        if(controller.CalculatedPath.corners.Length > 0) { 
            Vector3 start = controller.CalculatedPath.corners[0];
            for (int i = 1; i < Math.Min(controller.CalculatedPath.corners.Length, 3); i++)
            {
                Vector3 end = new(controller.CalculatedPath.corners[i].x, -0.5f, controller.CalculatedPath.corners[i].z);
                float distance = Vector3.Distance(start, end);
                int numOfArrows = (int)distance / 1;
                if (distance < 1)
                {
                    numOfArrows = 1;
                }
                float increment = (1.0f / numOfArrows);

                for (float t = increment; t <= 1.0f; t += increment)
                {
                    Vector3 point = LerpPoint(t, start, end);
                    GameObject indicatorArrow = SpawnIndicatorArrow(point, Quaternion.LookRotation(end - start, Vector3.up));
                    arrows.Add(indicatorArrow);
                }
                start = end;
            }
        }
    }

    private GameObject SpawnIndicatorArrow(Vector3 position, Quaternion rotation)
    {
        GameObject indicatorArrow = Instantiate(arrowPrefab, position, rotation);
        indicatorArrow.transform.parent = pathParent.transform;
        return indicatorArrow;
    }
}
