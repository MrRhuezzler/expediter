using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointArrow : MonoBehaviour
{

    private MeshRenderer[] children;

    void Start()
    {
        children = GetComponentsInChildren<MeshRenderer>();
        Hide();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Show();
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (MeshRenderer child in children)
        {
            child.enabled = true;
        }
    }

    private void Hide()
    {
        foreach (MeshRenderer child in children)
        {
            child.enabled = false;
        }
    }

}
