using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager
{
    // 점수 관련 매니저

    public int Score { get; private set; }
    public int PlusScore { get; private set; }

    public Vector2 TouchPos { get; private set; }

    public void GetTouchPosition(Vector2 position)
    {
        TouchPos = position;
    }

    public void SetScore(int num)
    {
        PlusScore = num;
        Score += PlusScore;
    }

    // 스코어 텍스트로 변환하기
    public void ScoreToText(Text score, Text maxScore)
    {
        // 기본 점수
        score.text = "나의 점수 : " + Manager.Score.Score.ToString();

        // 최고 점수
        PlayerPrefs.SetInt("MaxScore", Mathf.Max(Manager.Score.Score, PlayerPrefs.GetInt("MaxScore")));
        maxScore.text = "최고 점수 : " + PlayerPrefs.GetInt("MaxScore").ToString();
    }
}
