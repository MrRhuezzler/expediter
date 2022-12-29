using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

public class LocationServiceManager : MonoBehaviour
{
    
    [SerializeField]
    private TMP_Text locationText;

    [SerializeField]
    private GameObject anchorsParent;

    [SerializeField]
    private GameObject possibleLocationPrefab;

    private List<AnchorRect> anchorsRects = new List<AnchorRect>();

    private void Start()
    {
        //StartCoroutine(StartLocationService());
        anchorsRects.Clear();
        foreach(AnchorRect ar in anchorsParent.GetComponentsInChildren<AnchorRect>())
        {
            anchorsRects.Add(ar);
            ar.Start();
        }

        foreach(AnchorRect ar in anchorsRects)
        {
            Vector3 possibleLocation = ar.TrileratePoint(new GeoCordinates(11.024633, 77.004824));
            Instantiate(possibleLocationPrefab, possibleLocation, Quaternion.identity);
        }
    }

    IEnumerator StartLocationService()
    {
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
            Permission.RequestUserPermission(Permission.FineLocation);

        // Starts the location service.
        Input.location.Start(1, 1);

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
        else
        {
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        // Stops the location service if there is no need to query location updates continuously.
        // Input.location.Stop();
    }

    void Update()
    {

    }
}
