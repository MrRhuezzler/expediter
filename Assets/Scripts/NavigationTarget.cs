using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationTarget : MonoBehaviour
{
    [SerializeField]
    public string Name;

    public NavigationTarget(string name)
    {
        Name = name;
    }
}
