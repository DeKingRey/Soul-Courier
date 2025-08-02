using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulCourier.Abilities;
using UnityEngine.UI;

public class UseAbility : MonoBehaviour
{
    public AbilityType type;
    public IUseAbility abilityLogic;
    public string id;
    public string abilityInfo; // In the format "Ability Name - Ability Power"
                               // Could add a change in colour to the popup later
    private EntryPopup abilityPopup;
    private bool playerHas;

    void Start()
    {
        abilityLogic = GetComponent<IUseAbility>();

        abilityPopup = FindObjectOfType<EntryPopup>(true);
    }

    void Update()
    {
        if (type == AbilityType.Active) UpdateActive();
        if (type == AbilityType.OneShot) UpdateOneShot();
    }

    void PickUpAbility()
    {
        // Player pickup ability and add
        if (type == AbilityType.Passive)
        {
            abilityLogic.Use();
        }
        else
        {
            playerHas = true;
        }
        CompendiumTracker.Instance.UnlockEntry(id);

        abilityPopup.gameObject.SetActive(true);
        abilityPopup.UpdateEntryText(abilityInfo);
    }
    
    void UpdateActive()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerHas)
        {
            abilityLogic.Use(); // Will start a countdown in the use too
        }
    }

    void UpdateOneShot()
    {
        if (Input.GetKeyDown(KeyCode.Q) && playerHas)
        {
            abilityLogic.Use(); // Will destroy the ability in the use function
        }
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.CompareTag("Player"))
        {
            PickUpAbility();
        }
    }
}
