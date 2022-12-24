using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointArrow : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(true);
    }
    void OnTriggerEnter(Collider collision)
    {
        print("sdkflksjkfjkfsjk");
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider collision)
    {
        print("sdkflksjkfjkfsjk");
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }

}
