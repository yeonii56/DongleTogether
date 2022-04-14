using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public Dictionary<Define.SFX, AudioClip> Clips = new Dictionary<Define.SFX, AudioClip>();   

    void LoadClip()
    {
        foreach (Define.SFX sfx in System.Enum.GetValues(typeof(Define.SFX)))
            Clips[sfx] = Resources.Load<AudioClip>("Sounds/" + sfx.ToString());
    }

    public Dictionary<Define.Color, Sprite> Colors = new Dictionary<Define.Color, Sprite>();

    void LoadColor()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprite/Dongle");

        foreach (Define.Color sprite in System.Enum.GetValues(typeof(Define.Color)))
        {
            Colors[sprite] = sprites[(int)sprite];
        }
    }

    public void Init()
    {
        LoadClip();
        LoadColor();
    }
}
