using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settigsHandler : MonoBehaviour
{
    public Camera mainCam;
    public Camera zoomCam;
    public GameObject dataBox;
    public Button settingsButton;
    public Sprite settingsOnImage;
    public Sprite settingsOffImage;
    bool settingsBool = false;
    static public bool zoomBool = false;
    // Start is called before the first frame update
    void Start() { 
        mainCam.enabled = true;
        zoomCam.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleDataBox()
    {
        if (settingsBool)
        {
            dataBox.SetActive(false);
            settingsButton.GetComponent<Image>().sprite = settingsOnImage;
            settingsBool = !settingsBool; 
        }
        else
        {
            dataBox.SetActive(true);
            settingsButton.GetComponent<Image>().sprite = settingsOffImage;
            settingsBool = !settingsBool;
        }
    }

    public void toggleZoom()
    {
        zoomBool = !zoomBool;
        mainCam.enabled = !mainCam.enabled;
        zoomCam.enabled = !zoomCam.enabled;
    }
}
