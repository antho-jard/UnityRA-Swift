using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swift
{
    public class JsonUtils : MonoBehaviour {

        public GameObject PopUpNotif;
        public Button ButtonTemplate; //For the save_selection button
        public GameObject GridForButtons; //Canvas with every buttons
        GameObject[] GOmachines;
        GameObject dataManager;
        GameObject loadConfig;
        GameObject scrollView;
        Animator popupNotifAnim;

        bool configLoaded = false;
        private void Awake()
        {
            dataManager = GameObject.Find("DataManager");
            loadConfig = GameObject.Find("Button_LoadConfig");
            scrollView = GameObject.Find("ScrollView");
            popupNotifAnim = PopUpNotif.GetComponent<Animator>();
        }
        void Start () {
            BetterStreamingAssets.Initialize(); 
	        if(GOmachines == null)
            {
                GOmachines = GameObject.FindGameObjectsWithTag("Machine");
            }
            scrollView.SetActive(false);
        }

        /// <summary>
        /// Generates a file name using the following convention : Swift YYYY MM DD – HH mm ss
        /// </summary>
        /// <returns>string fileName</returns>
        string GenerateFileName()
        {
            string fileName = "Swift ";
            DateTime date = DateTime.Now;
            fileName += date.ToString("yyyy MM dd - HH mm ss");
            fileName += ".json";
            return fileName;
        }

        IEnumerator addDelay(float delay)
        {
            yield return new WaitForSeconds(2f);
            popupNotifAnim.SetBool("active", false);
        }

        /// <summary>
        /// Display the list of saved configuration files on the selection pannel
        /// </summary>
        public void LoadAndDisplayMachineConfigs()
        {
            //Deactivate the Load & Save buttons and activate the ScrollView
            loadConfig.SetActive(false);
            scrollView.SetActive(true);
            
            if(!configLoaded)
            {
                configLoaded = true;
                //Gets all the json files in the StreamingAssets/SavedLayout/ repertory
                //Desktop: string[] configFiles = Directory.GetFiles(Application.streamingAssetsPath + "/SavedLayout/", "*.json");
                string[] configFiles = BetterStreamingAssets.GetFiles("/SavedLayout/", "*.json");
                //For each config file we create a button with the name of the file
                foreach (var filePath in configFiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    Button newButton = Instantiate(ButtonTemplate) as Button;
                    //Set the parent element of the button
                    newButton.transform.SetParent(GridForButtons.transform, false);
                    newButton.GetComponentInChildren<TextMeshProUGUI>().text = fileName;
                    //Puts a listener on the button to call LoadSelectedMachineConfig if the button is clicked
                    newButton.onClick.AddListener(() => LoadSelectedMachineConfig(filePath));
                }
            }
        }

        /// <summary>
        /// Load the selected file and move the machines
        /// </summary>
        public void LoadSelectedMachineConfig(string filePath)
        { 
            Debug.Log(filePath);
           if (BetterStreamingAssets.FileExists(filePath))
            {
                //Read the json file and put it in dataAsJson
                string dataAsJson = BetterStreamingAssets.ReadAllText(filePath); //Desktop: File.ReadAllText(filePath);
                
                //Pass the json to JsonUtility and create a RootObject (the list of every machines in the savefile)
                RootObject machinesJson = JsonUtility.FromJson<RootObject>(dataAsJson);
                //For each machine saved we change the Pos/Rot values of the corresponding GameObject
                foreach (var machine in machinesJson.machinesList)
                {
                    var tempMachine = GameObject.Find(machine.MachineName);
                    tempMachine.transform.localPosition = machine.MachinePosition;
                    tempMachine.transform.localRotation = machine.MachineRotation;
                }
            }
            else
            {
                Debug.Log("Path given not found");
            }
            loadConfig.SetActive(true);
            scrollView.SetActive(false);
        }

    }

    [Serializable]
    public class Machine
    {
        public string MachineName;
        public Vector3 MachinePosition;
        public Quaternion MachineRotation;
    }

    [Serializable]
    public class RootObject
    {
        public List<Machine> machinesList;
    }
}
