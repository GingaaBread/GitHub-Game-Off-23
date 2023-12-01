using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelFunctions : MonoBehaviour
{
    public Toggle musicToggle;
    public Toggle soundToggle;
    public Toggle cameraFollowToggle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMusicToggle(){
        Debug.Log("Music Toggle status:" + musicToggle.isOn);
    }

    public void OnSoundToggle(){
        Debug.Log("Sound Toggle status:" + soundToggle.isOn);
    }

    public void OnCameraFollowToggle(){
        Debug.Log("Camera Follow Toggle status:" + cameraFollowToggle.isOn);
    }
}
