using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompendiumPage : MonoBehaviour
{
    public string id;
    private bool hidden = true;
    private GameObject pageInfo;
    private GameObject pageHide;

    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Info"))
            {
                pageInfo = child.gameObject;
            }
            else if (child.CompareTag("Hide"))
            {
                pageHide = child.gameObject;
            }
        }

        pageInfo.SetActive(false);
        pageHide.SetActive(true);
    }

    void Update()
    {
        if (CompendiumTracker.Instance.IsEntryUnlocked(id) && hidden)
        {
            Show();
        }
    }

    void Show()
    {
        pageInfo.SetActive(true);
        pageHide.SetActive(false);
        hidden = false;
    }
}
