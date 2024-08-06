using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadSlot : MonoBehaviour
{
    Button button;
    TextMeshProUGUI buttonText;

    public int slotNumber;

    void Awake()
    {
        button = GetComponent<Button>();
        buttonText = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        button.onClick.AddListener(()=>
        {
            if (SaveManager.Instance.IsSlotEmpty(slotNumber) == false)
            {
                SaveManager.Instance.StartLoadedGame(slotNumber);
                SaveManager.Instance.DeselectButton();
            }
            else
            {
                // Do nothing
            }
        });
    }

    void Update()
    {
        if (SaveManager.Instance.IsSlotEmpty(slotNumber))
        {
            buttonText.text = "Empty";
        }
        else
        {
            buttonText.text = PlayerPrefs.GetString("Slot" + slotNumber + "Description");
        }
    }
}
