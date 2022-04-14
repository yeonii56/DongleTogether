using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIData : MonoBehaviour
{
    [SerializeField]
    Slider musicSlider;
    [SerializeField]
    Slider soundSlider;

    [SerializeField]
    Text score;
    [SerializeField]
    Text maxScore;
    [SerializeField]
    Text plusScore;

    void Awake()
    {
        SetMusicVolum();
        SetSoundVolum();
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("MaxScore"))
        {
            PlayerPrefs.SetInt("MaxScore", 0);
        }
        Manager.Score.ScoreToText(score, maxScore);
    }

    private void Update()
    {
        // 터질때만 점수계산 하도록
        if (GameManager.Instance.isPung && Time.timeScale != 0)
        {
            Manager.Score.ScoreToText(score, maxScore);
            PlusScoreToText();
        }
    }

    // 방금 터트린 스코어 나타내기
    public void PlusScoreToText()
    {
        plusScore.transform.position = Manager.Score.TouchPos;
        StartCoroutine(PlusScoreRoutine());
    }

    // 방금 터트린 스코어 잠깐 나타났다 사라지는 효과
    IEnumerator PlusScoreRoutine()
    {
        plusScore.text = "+ " + Manager.Score.PlusScore.ToString();
        yield return Manager.Coroutine.WaitSeconds(0.5f);

        plusScore.text = "";
    }

    public void SetMusicVolum()
    {
        Manager.Sound.audioSources[(int)Define.Audio.MusicSource].volume = musicSlider.value;
    }

    public void SetSoundVolum()
    {
        Manager.Sound.audioSources[(int)Define.Audio.SoundSource].volume = soundSlider.value;
    }
}
