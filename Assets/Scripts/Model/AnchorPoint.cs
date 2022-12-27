using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPoint : MonoBehaviour, IComparable<AnchorPoint>
{

    public string Name;
    public GeoCordinates worldCoordinates;

    [SerializeField]
    private int index;

    public int CompareTo(AnchorPoint other)
    {
        if (other == null || other.index == index) return 0;
        return index > other.index ? 1 : -1;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
