using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    Button button;
    TextMeshProUGUI buttonText;

    public int slotNumber;
    public GameObject alertUI;
    Button yesBTN;
    Button noBTN;

    void Awake()
    {
        button = GetComponent<Button>();
        buttonText = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        yesBTN = alertUI.transform.Find("Yes Button").GetComponent<Button>();
        noBTN = alertUI.transform.Find("No Button").GetComponent<Button>();
    }

    void Start()
    {
        button.onClick.AddListener(()=>
        {
            if (SaveManager.Instance.IsSlotEmpty(slotNumber))
            {
                SaveGameConfirmed();
            }
            else
            {
                DisplayOverrideWarning();
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

    public void DisplayOverrideWarning()
    {
        alertUI.SetActive(true);

        yesBTN.onClick.AddListener(()=>
        {
            SaveGameConfirmed();
            alertUI.SetActive(false);
        });

        noBTN.onClick.AddListener(()=>
        {
            alertUI.SetActive(false);
        });
    }

    void SaveGameConfirmed()
    {
        SaveManager.Instance.SaveGame(slotNumber);

        DateTime dt = DateTime.Now;
        string time = dt.ToString("yyy-MM-dd HH:mm");
        string description = "Saved Game " + slotNumber + " | " + time;

        buttonText.text = description;

        PlayerPrefs.SetString("Slot" + slotNumber + "Description", description);
        SaveManager.Instance.DeselectButton();
    }
}
