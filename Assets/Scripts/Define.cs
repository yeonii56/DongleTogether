using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    #region constant
    // 상수 지정
    public const float DONGLE_SPAWN_POSITION = 0.36f;
    public const int DONGLE_SPAWN_NUM = 5;
    public const float MAXTIME = 5f;

    #endregion

    #region enum
    // enum 선언
    public enum Color
    {
        Dongle_0,
        Dongle_1,
        Dongle_2,
        Dongle_3,
    }

    public enum SFX
    {
        Button,
        Music,
        Pung,
        Shake,
        StartBgm,
        Touch
    }

    public enum Audio
    {
        MusicSource,
        SoundSource,
    }
    #endregion
}
