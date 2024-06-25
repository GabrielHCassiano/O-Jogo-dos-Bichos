using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider sliderMaster;
    [SerializeField] private Toggle toggle;
    [SerializeField] private TMP_Dropdown dropResulution;

    // Start is called before the first frame update
    void Start()
    {
        sliderMaster.value = PlayerPrefs.GetFloat("Master", 0.5f);
        toggle.isOn = PlayerPrefs.GetInt("Full") != 1;
        dropResulution.value = PlayerPrefs.GetInt("Resolution");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMaster(float volume)
    {
        PlayerPrefs.SetFloat("Master", volume);
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        PlayerPrefs.SetInt("Full", isFullscreen ? 0 : 1);
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolution)
    {

        PlayerPrefs.SetInt("Resolution", resolution);

        switch (resolution)
        {
            case 0:
                Screen.SetResolution(1920, 1080, Screen.fullScreen = toggle.isOn);
                break;
            case 1:
                Screen.SetResolution(1366, 768, Screen.fullScreen = toggle.isOn);
                break;
            case 2:
                Screen.SetResolution(1280, 720, Screen.fullScreen = toggle.isOn);
                break;
            case 3:
                Screen.SetResolution(1024, 768, Screen.fullScreen = toggle.isOn);
                break;
            case 4:
                Screen.SetResolution(800, 480, Screen.fullScreen = toggle.isOn);
                break;
        }
    }
}
