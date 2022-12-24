using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SetNavigationTarget : MonoBehaviour {

    [SerializeField]
    private TMP_Dropdown navigationTargetsDropdown;

    [SerializeField]
    private GameObject navigationTargetParent;

    [SerializeField]
    private TMP_Text errorText;

    [SerializeField]
    private GameObject pathParent;

    [SerializeField]
    private GameObject arrowPrefab;

    private readonly List<NavigationTarget> navigationTargets = new List<NavigationTarget>();

    private readonly List<GameObject> trackArrows = new List<GameObject>();

    private NavMeshPath path;
    private Vector3 targetPosition = Vector3.zero;

    private void Start ()
    {
        path = new NavMeshPath();
        navigationTargets.Clear();
        foreach (NavigationTarget child in navigationTargetParent.GetComponentsInChildren<NavigationTarget>())
        {
            navigationTargets.Add(child);
            navigationTargetsDropdown.options.Add(new TMP_Dropdown.OptionData(child.Name));
        }

    }

    public class Lerp
    {
        private Vector3 start, end;
        private float t;
        private readonly float increment;

        public Lerp GetEnumerator()
        {
            return this;
        }

        public Lerp(Vector3 start, Vector3 end, int steps = 10)
        {
            this.start = start;
            this.end = end;
            increment = 1 / steps;
            t = 0;
        }

        private float LerpPoint(float t, float a, float b)
        {
            return a + (b - a) * t;
        }

        public bool MoveNext()
        {
            t += increment;
            return (t > 1);
        }

        public Vector3 Current => new(LerpPoint(t, start.x, end.x), LerpPoint(t, start.y, start.y), LerpPoint(t, start.z, end.z));
    }

    private Vector3 LerpPoint(float t, Vector3 start, Vector3 end)
    {
        return start + (end - start) * t;
    }

    private void CalculateTrackPoints()
    {
        trackArrows.Clear();
        Vector3 start = path.corners[0];
        for (int i = 1; i < path.corners.Length; i++)
        {
            Vector3 end = new(path.corners[i].x, -0.5f, path.corners[i].z);
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
                GameObject indicatorArrow = Instantiate(arrowPrefab, point, Quaternion.LookRotation(end - start, Vector3.up));
                indicatorArrow.transform.parent = pathParent.transform;
                trackArrows.Add(indicatorArrow);
            }
            start = end;
        }
    }

    private void Update ()
    {
        if (targetPosition != Vector3.zero)
        {

        }
    }

    public void SetCurrentNavigationTarget(int selected)
    {
        targetPosition = Vector3.zero;
        string selectedText = navigationTargetsDropdown.options[selected].text;
        NavigationTarget currentTarget = navigationTargets.Find(x => x.Name.Equals(selectedText));
        if(currentTarget!= null)
        {
            errorText.text = "";
            targetPosition = currentTarget.gameObject.transform.position;
            NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);
            CalculateTrackPoints();
        } else
        {
            errorText.text = "No target selected";
        }
    }

}