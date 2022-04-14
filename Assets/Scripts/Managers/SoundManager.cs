using UnityEngine;

public class SoundManager
{
    // 사운드 매니저

    public AudioSource[] audioSources = new AudioSource[System.Enum.GetValues(typeof(Define.Audio)).Length];

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if(root == null)
        {
            root = new GameObject { name = "@Sound" };            
        }
        string[] names = System.Enum.GetNames(typeof(Define.Audio));

        for (int i = 0; i < audioSources.Length; i++)
        {
            GameObject go = new GameObject { name = names[i] };
            go.transform.parent = root.transform;           
            audioSources[i] = go.AddComponent<AudioSource>();
        }

        Audioplay(Define.Audio.MusicSource, Define.SFX.Music);
    }

    public void Audioplay(Define.Audio source, Define.SFX clip)
    {
        ClipChange(audioSources[(int)source], Manager.Resource.Clips[clip]);
    }

    void ClipChange(AudioSource audiosource, AudioClip clip)
    {
        if (audiosource.clip != clip)
        {
            audiosource.clip = clip;
        }
        audiosource.Play();
    }
}
