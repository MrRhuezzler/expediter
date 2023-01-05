using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NavigationController : MonoBehaviour
{
    [SerializeField]
    private NavigationTargetManager targetManager;

    public NavMeshPath CalculatedPath { get; private set; }

    public float playerSpeedMin;

    public TMP_Text destinationName;
    public TMP_Text blockName;
    public TMP_Text distanceText;
    public TMP_Text timeText;


    // Start is called before the first frame update
    private void Start()
    {
        CalculatedPath = new NavMeshPath();
    }

    public float GetPathLength()
    {
        float lng = 0.0f;

        if(CalculatedPath.corners.Length > 1)
        {
            for (int i = 1; i < CalculatedPath.corners.Length; ++i)
            {
                lng += Vector3.Distance(CalculatedPath.corners[i - 1], CalculatedPath.corners[i]);
            }
            
            return lng;
        }

        return 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(targetManager.currentTarget != null)
        {
            NavMesh.CalculatePath(transform.position, targetManager.currentTarget.transform.position, NavMesh.AllAreas, CalculatedPath);
            destinationName.text = targetManager.currentTarget.GetComponent<NavigationTarget>().Name;
            blockName.text = "M Block";
            float totalDistance = Vector3.Distance(transform.position, CalculatedPath.corners[0]) + GetPathLength();
            distanceText.text = Mathf.RoundToInt(totalDistance) + "m";

            string timeString = "";
            float timeTaken = totalDistance / (playerSpeedMin * 60);

            if(timeTaken < 1)
            {
                timeString += (timeTaken * 60).ToString("F0") + "s";
            } else
            {
                timeString += timeTaken.ToString("F0") + "min";
            }

            timeText.text = timeString;
        }
    }
}
