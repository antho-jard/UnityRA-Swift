using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class ToggleMenuBehaviour : MonoBehaviour {
    Toggle toggleItem;
    public Text ToggleText;

    GameObject canvasMenu; 

    void Start()
    {
        //Fetch the Toggle GameObject
        toggleItem = GetComponent<Toggle>();
        //Add listener for when the state of the Toggle changes, to take action
        toggleItem.onValueChanged.AddListener(delegate {
            toggleValueChanged(toggleItem);
        });
        canvasMenu = GameObject.Find("CanvasMenu");
        ToggleText.text = "Menu";
    }

    //Output the new state of the Toggle into Text
    void toggleValueChanged(Toggle change)
    {
        canvasMenu.SetActive(toggleItem.isOn);
        ToggleText.text = "Menu";
    }
}