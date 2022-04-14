using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDongle : MonoBehaviour
{
    // 동글 스폰하는 기능, 탐색 용이하게 리스트에 넣어둠
    // 없앤 동글 큐에 넣어서 재사용

    [SerializeField]
    GameObject dongle;

    public List<Dongle> dongleList;
    public List<Dongle> bestList;    
    public List<Dongle> newList;    
    public List<Dongle> lineList;    
    public Dongle dong;
    Queue<Dongle> queue;

    public int test;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        // 큐 생성
        queue = new Queue<Dongle>();
        dongleList = new List<Dongle>();
        bestList = new List<Dongle>();
        newList = new List<Dongle>();
        lineList = new List<Dongle>();


        // 5*5 동글 생성
        CreateDongle();
    }

    // 5*5 동글 생성
    void CreateDongle()
    {
        int count = 0;

        for (int i = 0; i < Define.DONGLE_SPAWN_NUM; i++)
        {
            for (int j = 0; j < Define.DONGLE_SPAWN_NUM; j++)
            {                
                count++;
                CreateOneDongle().transform.localPosition = new Vector2(j * Define.DONGLE_SPAWN_POSITION, i * Define.DONGLE_SPAWN_POSITION);
                dong.name = "Dong" + count.ToString();
            }
        }
    }

    // 동글 한개 생성, 리스트에 넣기
    Dongle CreateOneDongle()
    {
        dong = Instantiate(dongle, transform).GetComponent<Dongle>();

        dongleList.Add(dong);

        return dong;
    }

    // 큐에 비활성화된 동글 넣고, 리스트에서는 제거
    public void EnQueue(Dongle dong)
    {
        queue.Enqueue(dong);
        dongleList.Remove(dong);
    }

    // 동글 큐에서 꺼내고, 리스트에 넣기
    Dongle DeQueue()
    {
        if (queue.Count > 0)
        {
            dong = queue.Dequeue();
            dongleList.Add(dong);
        }
        else
        {
            CreateOneDongle();
        }
        return dong;
    }

    // 큐에서 새로운동글 올리고 터진 동글 큐에 넣기
    public void CreateNewDongle(Dongle item)
    {
        StartCoroutine(ResetRoutine(item));
    }


    // 게임판 위에다가 큐에 있는 동글 활성화해서 놓기
    IEnumerator ResetRoutine(Dongle item)
    {        
        Vector2 newPosition = new Vector2(item.transform.position.x, item.transform.position.y + 3f);
        Dongle newDong = DeQueue();
        newDong.transform.position = newPosition;

        yield return Manager.Coroutine.WaitSeconds(0.2f);

        newDong.gameObject.SetActive(true);

        yield return Manager.Coroutine.WaitSeconds(0.5f);

        GameManager.Instance.isPung = false;
    }

    // 가장 길게 이어지는 동글리스트 판별
   void CreateHintList()
    {
        if(bestList.Count == 0)
        {
            // 25번 탐색
            for (int i = 0; i < dongleList.Count; i++)
            {
                dong = dongleList[i];

                // 가장 긴 리스트를 bestList로 설정
                if (!dong.inTheList)
                {
                    dong.DongleInTheList(newList);

                    if (bestList.Count == 0 || newList.Count > bestList.Count)
                    {
                        bestList.Clear();
                        bestList.AddRange(newList);
                    }
                    newList.Clear();
                }               
            }
        }
    }


    // 처음 시작할 동글 판별
    void SelectStartDongle()
    {
        CreateHintList();

        dong = null;

        for (int i = 0; i < bestList.Count; i++)
        {
            foreach (Dongle item in bestList)
            {
                item.inTheList = false;
            }

            dong = bestList[i];

            dong.inTheList = true;
            newList.Add(dong);
            dong.ConnectLine(newList);

            if(lineList.Count == 0)
            {
                lineList.AddRange(newList);
            }
            else if(lineList.Count < newList.Count)
            {
                lineList.Clear();
                lineList.AddRange(newList);                
            }
            newList.Clear();
        }
        bestList.Clear();
    }

    // 힌트
    public void Hint()
    {     
        if(lineList.Count == 0)
            SelectStartDongle();

        foreach (Dongle item in lineList)
        {
            StartCoroutine(HintRoutine(item));
        }
    }

    // 힌트 줄 때 커졌다 작아지게 하기
    IEnumerator HintRoutine(Dongle dong)
    {
        Animator anim = dong.GetComponent<Animator>();

        anim.SetBool("IsTouch", true);
        yield return Manager.Coroutine.WaitSeconds(0.5f);

       anim.SetBool("IsTouch", false);
    }

    // 섞기 버튼 누를때 힌트 초기화하기
    public void InitHint()
    {
        foreach (Dongle item in dongleList)
        {
            item.inTheList = false;
            item.nearCount = 0;
            item.bestlist.Clear();
            item.list.Clear();
            item.dongList.Clear();
        }
        lineList.Clear();
    }
}
