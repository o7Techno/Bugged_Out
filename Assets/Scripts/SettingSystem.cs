using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class SettingSystem : MonoBehaviour
{
    public bool isOpen;
    public GameObject craftingUI;
    public GameObject inventoryUI;
    public GameObject settingsUI;


    private void Start()
    {
        isOpen = false;
        settingsUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isOpen)
        {
            settingsUI.SetActive(true);
            craftingUI.SetActive(false);
            inventoryUI.SetActive(false);
            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            settingsUI.SetActive(false);
            isOpen = false;
        }

    }
}
