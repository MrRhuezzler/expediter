using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.XR.ARFoundation;

public class LocationServiceManager : MonoBehaviour
{
    
    [SerializeField]
    private TMP_Text locationText;

    [SerializeField]
    private GameObject anchorsParent;

    [SerializeField]
    private GameObject possibleLocationPrefab;

    [SerializeField]
    private ARSessionOrigin sessionOrigin;

    [SerializeField]
    private ARSession session;

    [SerializeField]
    private float waitTimer;

    private List<AnchorRect> anchorsRects = new List<AnchorRect>();
    private float timer;

    private void Start()
    {
        GetAllAnchorRects();
        StartCoroutine(UpdateLocation());
        //TestLocation();
    }

    private void GetAllAnchorRects()
    {
        anchorsRects.Clear();
        foreach (AnchorRect ar in anchorsParent.GetComponentsInChildren<AnchorRect>())
        {
            anchorsRects.Add(ar);
            ar.Start();
        }
    }

    IEnumerator UpdateLocation()
    {
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
        {
            print("Enable Location Services !");
            Permission.RequestUserPermission(Permission.FineLocation);
        }

        // Starts the location service.
        Input.location.Start(0.01f,0.01f);
        Input.compass.enabled = true;

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }

        // Stops the location service if there is no need to query location updates continuously.
        ChangeCurrentLocation();
        //Input.location.Stop();
    }

    private void ReCenterArSessionOrigin(Vector3 position, Quaternion rotation)
    {
        session.Reset();
        sessionOrigin.transform.SetPositionAndRotation(position, rotation);
    }

    private void ChangeCurrentLocation()
    {
        foreach (AnchorRect ar in anchorsRects)
        {
            GeoCordinates pos = new GeoCordinates(Input.location.lastData.latitude, Input.location.lastData.longitude);
            if (ar.IsInsideRect(pos))
            {
                print("Changing up your location");
                //List<Vector3> possibleLocations = ar.TrileratePoint(pos);
                Vector3 possibleLocation = ar.TrileratePoint(pos);
                //ReCenterArSessionOrigin(possibleLocations[4], Quaternion.Euler(0, Input.compass.trueHeading - 80, 0));
                ReCenterArSessionOrigin(possibleLocation, Quaternion.Euler(0, Input.compass.trueHeading - 80, 0));
            }
        }
    }

    public void ResetLocation()
    {
        StartCoroutine(UpdateLocation());
    }

    private void TestLocation()
    {
        foreach (AnchorRect ar in anchorsRects)
        {
            GeoCordinates pos = new GeoCordinates(11.024414381312605, 77.00476120474491);
            if (ar.IsInsideRect(pos))
            {
                print("Changing up your location");
                /*
                List<Vector3> possibleLocations = ar.TrileratePoint(pos);
                foreach (Vector3 possibleLocation in possibleLocations)
                {
                    print(possibleLocation);
                    Instantiate(possibleLocationPrefab, possibleLocation, Quaternion.identity);
                }*/
            }
        }
    }

    void Update()
    {
        locationText.text = Input.location.lastData.latitude + "\n" + Input.location.lastData.longitude + "\n" + Input.location.lastData.altitude + "\n" + Input.location.lastData.horizontalAccuracy + "\n" + Input.location.lastData.timestamp + "\n" + Input.compass.trueHeading;
    }
}
