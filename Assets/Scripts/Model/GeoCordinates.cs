using System;
using UnityEngine;
using static System.Math;


[Serializable]
public class GeoCordinates
{
    // In Meters
    private const double RadiusOfEarth = 6371e3;
    private const double Deg2Rad = PI / 180.0;

    [SerializeField]
    public double latitude;

    [SerializeField]
    public double longitude;

    public GeoCordinates(double latitude, double longitude)
    {
        this.latitude = latitude;
        this.longitude = longitude;
    }

    public static bool IsInsideRect(GeoCordinates bl, GeoCordinates tr, GeoCordinates point)
    {
        if (point.longitude > bl.longitude && point.longitude < tr.longitude && point.latitude > bl.latitude && point.latitude < tr.latitude)
            return true;
        return false;
    }

    public double DistanceTo(GeoCordinates other)
    {
        double deltaLatitudes = (latitude - other.latitude) / 2.0;
        double deltaLongitudes = (longitude - other.longitude) / 2.0;
        double a = Pow(Sin(Deg2Rad * deltaLatitudes), 2) + (Cos(latitude * Deg2Rad) * Cos(other.latitude * Deg2Rad) * Pow(Sin(Deg2Rad * deltaLongitudes), 2));
        return 2 * RadiusOfEarth * Asin(Sqrt(a));
    }

}
