using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    static Manager s_instance;
    static Manager Instance { get { Init(); return s_instance; } }

    #region managers
    CoroutineManager _coroutineManager = new CoroutineManager();
    public static CoroutineManager Coroutine { get { return Instance._coroutineManager; } }
    SoundManager _soundManager = new SoundManager();
    public static SoundManager Sound { get { return Instance._soundManager; } }
    ResourceManager _resourceManager = new ResourceManager();
    public static ResourceManager Resource { get { return Instance._resourceManager; } }
    ScoreManager _scoreManager = new ScoreManager();
    public static ScoreManager Score { get { return Instance._scoreManager; } }
    #endregion

    void Awake()
    {
        Screen.SetResolution(Screen.width, (Screen.width * 16) / 9, true);
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Manager");

            if (go == null)
            {
                go = new GameObject { name = "@Manager" };
                go.AddComponent<Manager>();
            }

            s_instance = go.GetComponent<Manager>();
            s_instance._resourceManager.Init();
            s_instance._soundManager.Init();
        }
    }
}
