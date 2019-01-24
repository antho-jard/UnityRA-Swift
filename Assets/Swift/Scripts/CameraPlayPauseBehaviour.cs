using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

//Attach this script to a Toggle GameObject. To do this, go to Create>UI>Toggle.
//Set your own Text in the Inspector window

/// <summary>
/// Manage the Toggle button
/// </summary>
public class CameraPlayPauseBehaviour : MonoBehaviour {
    Toggle toggleItem;
    public Text ToggleText;

    void Start()
    {
        //Fetch the Toggle GameObject
        toggleItem = GetComponent<Toggle>();
        //Add listener for when the state of the Toggle changes, to take action
        toggleItem.onValueChanged.AddListener(delegate {
            toggleValueChanged(toggleItem);
        });

        updateTextStatus();
    }

    //Output the new state of the Toggle into Text
    void toggleValueChanged(Toggle change)
    {
        // Play or pause the camera flow
        VuforiaRenderer.Instance.Pause(!toggleItem.isOn);

        updateTextStatus();
    }

    //Output the new state of the Toggle into Text
    void updateTextStatus()
    {
        ToggleText.text = toggleItem.isOn ? "Playing" : "Paused";
    }
}



