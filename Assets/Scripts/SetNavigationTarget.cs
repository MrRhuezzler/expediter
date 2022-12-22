using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class SetNavigationTarget : MonoBehaviour {

    [SerializeField]
    private TMP_Dropdown navigationTargetsDropdown;

    [SerializeField]
    private GameObject navigationTargetParent;

    [SerializeField]
    private TMP_Text errorText;

    private readonly List<NavigationTarget> navigationTargets = new List<NavigationTarget>();

    private NavMeshPath path;
    private LineRenderer line;
    private Vector3 targetPosition = Vector3.zero;

    private bool lineVisibility = false;

    private void Start ()
    {
        path = new NavMeshPath();
        line = transform.GetComponent<LineRenderer>();
        line.enabled = lineVisibility;

        navigationTargets.Clear();
        foreach (NavigationTarget child in navigationTargetParent.GetComponentsInChildren<NavigationTarget>())
        {
            navigationTargets.Add(child);
            navigationTargetsDropdown.options.Add(new TMP_Dropdown.OptionData(child.Name));
        }

        errorText.text = "No target selected";

    }

    private void Update ()
    {
        if (lineVisibility && targetPosition != Vector3.zero)
        {
            NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);
            line.positionCount = path.corners.Length;
            line.SetPositions(path.corners);
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
        } else
        {
            errorText.text = "No target selected";
        }
    }

    public void NavigationLineVisibilityToggle()
    {
        lineVisibility = !lineVisibility;
        line.enabled = lineVisibility;
    }

}