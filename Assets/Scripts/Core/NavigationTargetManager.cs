using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NavigationTargetManager : MonoBehaviour
{

    [SerializeField]
    private TMP_Dropdown NavigationTargetsDropdown;

    [SerializeField]
    private GameObject NavigationTargetsParent;

    private List<NavigationTarget> NavigationTargets = new List<NavigationTarget>();

    public GameObject currentTarget { get; private set; }

    void Start()
    {
        NavigationTargets.Clear();
        foreach (NavigationTarget child in NavigationTargetsParent.GetComponentsInChildren<NavigationTarget>())
        {
            NavigationTargets.Add(child);
            NavigationTargetsDropdown.options.Add(new TMP_Dropdown.OptionData(child.Name));
        }
        currentTarget = null;
        NavigationTargetsDropdown.onValueChanged.AddListener(SetCurrentNavigationTarget);
    }

    public void SetCurrentNavigationTarget(int selected)
    {
        currentTarget = null;
        string selectedText = NavigationTargetsDropdown.options[selected].text;
        NavigationTarget selectedTarget = NavigationTargets.Find(x => x.Name.Equals(selectedText));
        if (selectedTarget != null)
        {
            currentTarget = selectedTarget.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
