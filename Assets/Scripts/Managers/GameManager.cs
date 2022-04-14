using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // UI 버튼 매니저
    // 시간도 관리

    static GameManager instance = null; 
    public static GameManager Instance { get { return instance; } }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this.gameObject); }
    }

    [SerializeField]
    SpawnDongle spawnDongle;

    [SerializeField]
    GameObject mainPanel;
    [SerializeField]
    GameObject pausedPanel;
    [SerializeField]
    GameObject gameOverPanel;
    [SerializeField]
    Transform spawner;
    [SerializeField]
    Slider timeSlider;
    [SerializeField]
    Image fillArea;
    [SerializeField]
    Text score;
    [SerializeField]
    Text maxScore;

    Dongle dong;

    public int[] colors = new int[4];

    // 일시정지 
    private  bool paused;
    public  bool Paused
    {
        get { return paused; }
        private set
        {
            paused = value;
            Time.timeScale = value ? 0 : 1;
            pausedPanel.SetActive(value);
        }
    }

    float timer;
    float a = 0f;
    float maxTime;
    bool timePlus;
    bool isColorTime;

    public bool isPung;

    private void Start()
    {       
        Application.targetFrameRate = 30; // 프레임 30으로 고정
        Time.timeScale = 0f;
        paused = false;
        isPung = true;
        isColorTime = false;
        timePlus = false;
        maxTime = Define.MAXTIME;
        timer = 0f;
    }

    private void Update()
    {
        Timeer();
    }

    // 타임슬라이더 시간에 따라 줄어드는 기능
    void Timeer()
    {
        // 줄어드는 속도는 점점 빨라짐, 최대치는 a가  maxTime * 0.1f일 경우
        if (a <= maxTime * 0.1f)
        {
            a += Time.deltaTime * 0.1f;
        }

        timer += Time.deltaTime * a;

        timeSlider.value = (maxTime - timer) / maxTime;

        // 타이머가 0보다 작아지면 게임 끝남
        if (timer >= maxTime)
        {
            Time.timeScale = 0f;
            Manager.Score.ScoreToText(score, maxScore);

            gameOverPanel.SetActive(true);

            instance.isPung = true;
        }

        // 타이머가 maxtime의 0.3f 이하만큼 남으면 슬라이더 색상 깜빡거리게함
        if(timeSlider.value <= 0.3f && !isColorTime)
        {
            StartCoroutine(ImageColorChange(fillArea, Color.red));
        }

        // 동글 터트리면 추가로 시간을 줌
        if (instance.isPung && !timePlus && timer > 1f)
        {
            timePlus = true;

            float plusTime;

            // 각 점수마다 더해지는 시간 다르게
            if(Manager.Score.PlusScore < 500)
            {
                plusTime = maxTime * 0.05f;
            }
            else if(Manager.Score.PlusScore < 1000)
            {
                plusTime = maxTime * 0.1f;
            }
            else
            {
                plusTime = maxTime * 0.2f;
            }

            // maxTime 이상으로 넘어가지 않게 하기
            if(timer - plusTime <= 0)
            {
                timer = 0;
            }
            else
            {
                timer -= plusTime;
            }
        }
        else if(timePlus && !instance.isPung)
        {
            timePlus = false;
        }
    }

    // 이미지 색상을 0.3f 동안 바뀌며 깜빡 거리게 함
    IEnumerator ImageColorChange(Image img, Color thisColor)
    {
        isColorTime = true;
        Color color = img.color;
        img.color = thisColor;
        yield return Manager.Coroutine.WaitSeconds(0.3f);

        img.color = color;
        yield return Manager.Coroutine.WaitSeconds(0.3f);

        isColorTime = false;
    }

    // 일시정지 버튼 클릭
    public void PauseClick()
    {
        Manager.Sound.Audioplay(Define.Audio.SoundSource, Define.SFX.Button);


        if (Paused == false)
        {
            Manager.Sound.audioSources[(int)Define.Audio.MusicSource].Pause();
        }
        else
        {
            Manager.Sound.audioSources[(int)Define.Audio.MusicSource].UnPause();
        }

        Paused = !Paused;
    }

    // 시작 버튼 클릭
    public void StartClick()
    {
        Time.timeScale = 1f;
        instance.isPung = false;
        mainPanel.SetActive(false);
        Manager.Sound.Audioplay(Define.Audio.MusicSource, Define.SFX.StartBgm);

        Manager.Sound.Audioplay(Define.Audio.SoundSource, Define.SFX.Button);

    }

    public void ButtonClick()
    {
        Manager.Sound.Audioplay(Define.Audio.SoundSource, Define.SFX.Button);
    }

    // 게임 종료
    public void ExitClick()
    {
        StartCoroutine("QuitRoutine");
    }

    // 나가기 전에 버튼 소리 나고 나가게
    IEnumerator QuitRoutine()
    {
        Manager.Sound.Audioplay(Define.Audio.SoundSource, Define.SFX.Button);


        yield return new WaitForSecondsRealtime(0.3f);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();        
    }

    // 다시 시작(씬 다시 로드)
    public void ReStartClick()
    {
        SceneManager.LoadScene(0);
    }

    // 동글 섞임
    public void ShakeDongle()
    {
        if (!instance.isPung)
        {
            Manager.Sound.Audioplay(Define.Audio.SoundSource, Define.SFX.Shake);


            spawnDongle.InitHint();

            for (int i = 0; i < spawner.childCount; i++)
            {
                dong = spawner.GetChild(i).GetComponent<Dongle>();

                dong.Init();
            }
        }           
    }

    // 힌트
    public void HintClick()
    {
        if (!instance.isPung)
        {
            Manager.Sound.Audioplay(Define.Audio.SoundSource, Define.SFX.Button);

            spawnDongle.Hint();
        }
        else return;
    }
}
