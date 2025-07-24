using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityPopup : MonoBehaviour
{
    public TextMeshProUGUI abilityText;

    public void UpdateAbilityText(string text)
    {
        abilityText.text = text;
    }

    public void DisablePopup()
    {
        gameObject.SetActive(false);
    }
}
