using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioMixer Mixer=null;
    AudioMixerGroup[] audioGroups;
    public Sound[] sounds;
    public static AudioController instance;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else {
            Destroy(gameObject);
             }
        audioGroups = Mixer.FindMatchingGroups("Master");
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.file;
            sound.source.loop = sound.looping;
            sound.source.pitch = sound.pitch;
            sound.source.volume = sound.volume;
            sound.initVol = sound.volume;
            if (Mixer != null)
                foreach(var m in audioGroups)
                {
                    if (m.name.Equals(sound.soundType))
                    {
                        sound.source.outputAudioMixerGroup = m;
                    }
                }
            //if(sound.soundType == "Music");
        }
    }
    private void Start()
    {
        PlaySound("Music");
    }
    
    public void PlaySound(string name)
    {
        foreach(Sound sound in sounds)
        {
            if (sound.name.Equals(name))
            {
                sound.source.Play();
                break;
            }
        }
        
    }
    public void StopSound(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name.Equals(name))
            {
                sound.source.Stop();
                break;
            }
        }
    }
    public bool isPlaying(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name.Equals(name))
            {
                return sound.source.isPlaying;
            }
        }
        Debug.Log("Sound " + name + " not found");
        return false;
    }
    public void ChangeVol(string name,float vol)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name.Equals(name))
            {
                sound.source.volume = vol;
                break;
            }
        }
    }
    public void ChangePitch(string name, float pitch)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name.Equals(name))
            {
                sound.source.pitch = pitch;
                break;
            }
        }
    }
    public void ChangeGroup_MasterVol(float val)
    {
        val = Mathf.Clamp(val, -80, 20);
        Mixer.SetFloat("MasterVol", val);
    }
    public void ChangeGroup_MusicVol(float val)
    {
        val = Mathf.Clamp(val, -80, 20);
        Mixer.SetFloat("MusicVol", val);
    }
    public void ChangeGroup_EffectVol(float val)
    {
        val = Mathf.Clamp(val, -80, 20);
        Mixer.SetFloat("EffectVol", val);
    }
}
