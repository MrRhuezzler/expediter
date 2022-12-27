using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationController : MonoBehaviour
{
    [SerializeField]
    private NavigationTargetManager targetManager;

    public NavMeshPath CalculatedPath { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        CalculatedPath = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
        if(targetManager.currentTarget != null)
        {
            NavMesh.CalculatePath(transform.position, targetManager.currentTarget.transform.position, NavMesh.AllAreas, CalculatedPath);
        }
    }
}
