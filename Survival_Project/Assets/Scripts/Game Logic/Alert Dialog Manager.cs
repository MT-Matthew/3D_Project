using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertDialogManager : MonoBehaviour
{
    public GameObject dialogBox;
    public Text messageText;
    public Button okButton;
    public Button cancelButton;

    System.Action<bool> responceCallback;

    void Start()
    {
        dialogBox.SetActive(false);

        okButton.onClick.AddListener(()=> HandleResponce(true));
        cancelButton.onClick.AddListener(()=> HandleResponce(false));
    }

    
    public void ShowDialog(string message, System.Action<bool> callback)
    {
        responceCallback = callback;
        messageText.text = message;
        dialogBox.SetActive(true);
    }

    private void HandleResponce(bool responce)
    {
        dialogBox.SetActive(false);
        responceCallback?.Invoke(responce);
    }




}
