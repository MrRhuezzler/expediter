using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocationService : MonoBehaviour
{
    [SerializeField]
    private TMP_Text errorText;

    [SerializeField]
    private TMP_Text locationText;

    // Start is called before the first frame update
    IEnumerator Start()
    {

        if(!Input.location.isEnabledByUser) {
            errorText.text = "Please enable location service";
            print("Please enable location service");
            yield break;
        }

        Input.location.Start(1, 1);

        int maxWait = 20;
        while(Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if(maxWait < 1)
        {
            errorText.text = "Timed Out";
            print("Timed Out");
            yield break;
        }

        if(Input.location.status == LocationServiceStatus.Failed)
        {
            errorText.text = "Unable to determine device location";
            print("Unable to determine device location");
            yield break;
        } else
        {
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        //Input.location.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        //locationText.text = Input.location.lastData.latitude + "\n" + Input.location.lastData.longitude + "\n" + Input.location.lastData.altitude + "\n" + Input.location.lastData.horizontalAccuracy + "\n" + Input.location.lastData.timestamp;
    }
}
