using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EntryPopup : MonoBehaviour
{
    public TextMeshProUGUI entryText;

    public void UpdateEntryText(string text)
    {
        entryText.text = text;
    }

    public void DisablePopup()
    {
        gameObject.SetActive(false);
    }
}
