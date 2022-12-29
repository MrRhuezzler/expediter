using UnityEngine;

[RequireComponent(typeof(NavigationController))]
[RequireComponent(typeof(LineRenderer))]
public class PathLine : MonoBehaviour
{
    private NavigationController controller;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<NavigationController>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        setLineRendererPositions();
    }

    private void setLineRendererPositions()
    {
        lineRenderer.positionCount = controller.CalculatedPath.corners.Length;
        lineRenderer.SetPositions(controller.CalculatedPath.corners);
    }

}
