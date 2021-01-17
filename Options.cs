using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Options : MonoBehaviour
{
    AudioController audioController;
    Resolution[] resolutions;
    [SerializeField]TMP_Dropdown resDropdown;
    private void Start()
    {
        audioController = FindObjectOfType<AudioController>();
        ResolutionSetup();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LevelController.Instance.UnloadOptions();
        }
    }

    private void ResolutionSetup()
    {
        Resolution[] possibleResolutions = Screen.resolutions;
        var actualResolutions = new List<Resolution>();
        resDropdown.ClearOptions();
        List<string> resStrings = new List<string>();
        int currentResIndex=0;
        foreach (var r in possibleResolutions)
        {
            if (r.refreshRate != 60 && r.refreshRate != 59 && r.refreshRate != 144 && r.refreshRate != 143) continue;
            actualResolutions.Add(r);
            if (r.refreshRate % 2 != 0) resStrings.Add(r.width + "x" + r.height + " @" + (r.refreshRate+1));
            else resStrings.Add(r.width + "x" + r.height+" @"+ r.refreshRate);
            if (r.width==Screen.currentResolution.width && r.height==Screen.currentResolution.height) currentResIndex = resStrings.Count - 1;
        }
        resolutions = actualResolutions.ToArray();
        resDropdown.AddOptions(resStrings);
        resDropdown.value = currentResIndex;
        resDropdown.RefreshShownValue();
    }

    public void SetResoution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void ChangeMaster(float MasterVol)
    {
        audioController.ChangeGroup_MasterVol(MasterVol);
    }
    public void ChangeMusic(float MusicVol)
    {
        audioController.ChangeGroup_MusicVol(MusicVol);
    }
    public void ChangeEffects(float EffectVol)
    {
        audioController.ChangeGroup_EffectVol(EffectVol);
    }
    public void ToggleFullScreen(bool toggle)
    {
        Screen.fullScreen = toggle;
    }
    public void UnloadOptions()
    {
        FindObjectOfType<LevelController>().UnloadOptions();
    }
}
