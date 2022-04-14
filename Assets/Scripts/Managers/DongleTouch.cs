using System.Collections.Generic;
using UnityEngine;

public class DongleTouch : MonoBehaviour
{
    // 1. 터치해서 잇기
    // -> 같은 색이면 이어지게, 리스트에 이어진 동글 다 넣기
    // => 넣어진 동글 터지면(isActive=fasle)하고 나서 리스트 비우기
    // 2. 터치 떼면 동글 터트리기(모델만 비활성화)

    List<Dongle> touchDongleList;
    LineRenderer line; // 한개로 사용하면 예각일 경우 굵기가 달라지는 현상이 생겨서 하나하나 넣어줌
    Animator anim;
    Dongle nowDong;
    Transform target;

    Vector2 touchPosition;

    void Awake()
    {
        Init();
    }

    void Update()
    {
        if (!GameManager.Instance.isPung)
        {
            OnTouch();
        }
        //OnAndroidMenu();
    }

    void Init()
    {
        touchDongleList = new List<Dongle>();
    }

    public void OnTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 pos = Camera.main.ScreenToWorldPoint(touch.position);       // 터치 포지션

            Ray ray = new Ray(pos, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);    // 터치된 물체        

            switch (touch.phase)
            {
                // 처음 터치됐을 때
                case TouchPhase.Began:
                    if (hit.collider == null)
                        return;

                    // 터치된 것이 동글이면 라인 이어지는 효과
                    if(hit.collider.CompareTag("Dongle"))
                    {
                        NextTarget(hit.transform);
                        Manager.Sound.Audioplay(Define.Audio.SoundSource, Define.SFX.Touch);
                    }
                    break;
                
                // 터치된 채로 이동했을 때
                case TouchPhase.Moved:
                    if (target == null)
                        return;

                    // 처음 닿은 물체가 동글이면 다음 물체가 같은 색의 동글일 경우 이어지게
                    if (target.CompareTag("Dongle"))
                    {
                        line.SetPosition(1, pos);
                        if (hit.transform != null && hit.transform.CompareTag("Dongle"))
                        {
                            Dongle hitDongle = hit.transform.GetComponent<Dongle>();

                            // 색이 같아야하고, 거리가 가깝고, 이미 이어지지 않아야만 연결 가능
                            if (hitDongle.dongleColor == nowDong.dongleColor && Vector2.Distance(target.position, pos) < 0.6f && !hitDongle.isLine)
                            {
                                Manager.Sound.Audioplay(Define.Audio.SoundSource, Define.SFX.Touch);

                                line.SetPosition(1, hit.transform.position);
                                NextTarget(hit.transform);
                            }
                        }
                    }

                    // 이전 동글로 되돌아 갔을경우 라인 취소되게
                    if(touchDongleList.Count > 1 && hit.transform == touchDongleList[touchDongleList.Count - 2].transform)
                    {
                        TouchDongleCancle();
                        nowDong = touchDongleList[touchDongleList.Count - 1];
                        target = hit.transform;
                        SetLine(nowDong.GetComponentInChildren<LineRenderer>());
                    }

                    break;

                // 터치 뗐을 경우 
                case TouchPhase.Ended:
                    if (target != null)
                    {
                        touchPosition = Camera.main.ScreenToWorldPoint(touch.position);       // 손 뗐을 때 터치 포지션 넘겨주기 위해서 넣기

                        // 이어진 동글이 2개 이상일 경우에만 터짐
                        if (touchDongleList.Count > 1)
                        {
                            Manager.Score.SetScore((int)Mathf.Pow(2, touchDongleList.Count));
                            Manager.Score.GetTouchPosition(touchPosition);

                            Manager.Sound.Audioplay(Define.Audio.SoundSource, Define.SFX.Pung);

                            foreach (Dongle dongle in touchDongleList)
                            {
                                dongle.isActive = false;
                            }
                        }
                        else
                        {
                            // 1개 이하의 동글만 터치했을 경우에 라인 취소되는 효과
                            TouchDongleCancle();
                        }
                        touchDongleList.Clear();
                    }
                    break;

                // 터치 대기 상태(마지막 프레임에서 변화 x)
                case TouchPhase.Stationary:
                    break;

                // 터치 강제로 취소된 상태(ex.5개 이상 터치 있으면 강제취소됨)
                case TouchPhase.Canceled:
                    break;
            }
        }
    }

    // 동글 잡힌 거 취소됨
    void TouchDongleCancle()
    {
        anim = nowDong.GetComponent<Animator>();
        anim.SetBool("IsTouch", false);
        nowDong.isLine = false;
        line.enabled = false;
        if (touchDongleList.Count > 1)
            touchDongleList.Remove(nowDong);
    }
    
    // 현재 동글 설정하고 라인 생성
    void NextTarget(Transform hit)
    {
        target = hit.transform;

        nowDong = target.GetComponent<Dongle>();
        nowDong.isLine = true;
        touchDongleList.Add(nowDong);

        anim = nowDong.GetComponent<Animator>();
        anim.SetBool("IsTouch", true);

        SetLine(target.GetComponentInChildren<LineRenderer>());
    }

    // 라인 생성
    void SetLine(LineRenderer nowLine)
    {
        line = nowLine;
        line.SetPosition(0, target.position);
        line.SetPosition(1, target.position);
        line.enabled = true;
    }

    // 갤럭시 버튼 효과
    void OnAndroidMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("esc");
            Handheld.Vibrate();
        }

        if (Input.GetKeyDown(KeyCode.Home))
        {
            // 홈버튼 누르면
            Debug.Log("home");
            Handheld.Vibrate();
        }

        if (Input.GetKeyDown(KeyCode.Menu))
        {
            Debug.Log("Menu");
        }
    }
}
