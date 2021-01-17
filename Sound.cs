using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip file;
    [HideInInspector]
    public AudioSource source;
    [HideInInspector]
    public float initVol;
    public string soundType;
    [Min(0)]
    public float volume=1;
    [Min(0)]
    public float pitch=1;
    public bool looping;

}
