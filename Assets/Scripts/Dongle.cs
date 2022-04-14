using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dongle : MonoBehaviour
{
    // 1. 터치해서 잇기
    // 2. 터치 떼면 동글 터트리기(모델만 비활성화)
    // 3. 터진 동글만큼 점수 얻기
    // 4. 큐에서 빼서 위에 새 동글 생성
    // 5. 동글 박스 큐에 넣고 비활성화

    SpawnDongle spawner;

    public List<Dongle> dongList;
    public List<Dongle> list;
    public List<Dongle> bestlist;
    public Collider2D[] hit;
    public Transform child;
    public int dongleColor;
    public int nearCount;
    public bool inTheList;
    public bool isLine;  // 이미 터치 된 동글인지 판별
    public bool isActive; // 터치 떼졌을때(터질때) false 됨

    Dongle dong;
    Animator anim;
    Rigidbody2D rigid;
    SpriteRenderer spriteR;

    void Awake()
    {
        spriteR = child.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        Init();

        dongList = new List<Dongle>();
        list = new List<Dongle>();
        bestlist = new List<Dongle>();
        spawner = GetComponentInParent<SpawnDongle>();
    }

    void Update()
    {
        // 동글이 터지면
        if (!isActive)
        {
            LineRenderer line = transform.GetComponentInChildren<LineRenderer>();
            line.enabled = false;

            spawner.InitHint();

            StartCoroutine("DestroyDongle");
        }
    }

    // 모델 비활성화하고 새로운 동글 큐에서 꺼내서 위에 놓고 터진 동글 큐에 넣고 비활성화
    IEnumerator DestroyDongle()
    {
        isActive = true;
        GameManager.Instance.isPung = true;

        child.gameObject.SetActive(false);
        spawner.CreateNewDongle(this);
        yield return Manager.Coroutine.WaitSeconds(0.5f);

        gameObject.SetActive(false);
        spawner.EnQueue(this);
    }

    // 초기화(리셋시 사용)
    public void Init()
    {
        inTheList = false;
        isLine = false;
        isActive = true;
        rigid.simulated = true;
        child.gameObject.SetActive(true);
        nearCount = 0;
        CreateDongle();
    }

    // 동글 생성될 때 효과
    void CreateDongle()
    {
        // 동글 색 지정
        dongleColor = Random.Range(0, System.Enum.GetValues(typeof(Define.Color)).Length);
        spriteR.sprite = Manager.Resource.Colors[(Define.Color)dongleColor];

        // 크기 커지게
        anim.SetTrigger("IsCreate");
    }

    // 주변 동글 탐색해서 리스트에 넣기
    public void DongleInTheList(List<Dongle> list)
    {
        // 근처에 있는 같은 색의 동글 리스트에 넣기
        hit = Physics2D.OverlapBoxAll(transform.position, Vector2.one * Define.DONGLE_SPAWN_POSITION * 2, 0, LayerMask.NameToLayer("default"));

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].CompareTag("Dongle"))
            {
                dong = hit[i].GetComponent<Dongle>();
                // 동글 색이 같으면 주변동글 리스트에 넣기
                if (dong.dongleColor == dongleColor)
                {
                    if (dong != this && !dongList.Contains(dong))
                    {
                        nearCount++;
                        dongList.Add(dong);
                    }

                    // 동글이 리스트에 들어있지 않으면 넘겨줄 list에 넣어주기 
                    if (!dong.inTheList)
                    {
                        dong.inTheList = true;
                        list.Add(dong);
                        // 리스트에 들어간 동글 이어서 탐색하기
                        dong.DongleInTheList(list);
                    }
                }
                else continue;
            }
        }
    }
    public void ConnectLine(List<Dongle> parentList)
    {
        // 이어지는 dong이 하나면 리스트에 넣고 그것으로 이어준다.
        if (dongList.Count == 1 && !dongList[0].inTheList)
        {
            dongList[0].inTheList = true;
            parentList.Add(dongList[0]);
            dongList[0].ConnectLine(parentList);
        }

        // 이어지는 dong이 두개 이상이면 dong이 긴 것을 판별한다.
        else if (dongList.Count > 1)
        {
            for (int i = 0; i < dongList.Count; i++)
            {
                if (!dongList[i].inTheList)
                {
                    list.Clear();
                    dongList[i].inTheList = true;
                    list.Add(dongList[i]);
                    dongList[i].ConnectLine(list);

                    // 가장 긴 리스트 선택
                    SelectBestList();
                }
                else { continue; }
            }

            if (bestlist.Count != 0)
            {
                foreach (Dongle item in bestlist)
                {
                    item.inTheList = true;
                }

                for (int i = 0; i < bestlist.Count; i++)
                {
                    if (!parentList.Contains(bestlist[i]))
                    {
                        parentList.AddRange(bestlist);
                        bestlist.Clear();
                    }
                }
            }
        }
    }

    void SelectBestList()
    {
        // 가장 긴 리스트 선택
        if (bestlist.Count == 0)
        {
            bestlist.AddRange(list);
        }
        else if (bestlist.Count < list.Count)
        {
            bestlist.Clear();
            bestlist.AddRange(list);
        }
        // 그 리스트 안의 inthelist = false; 해주기
        for (int j = 0; j < list.Count; j++)
        {
            list[j].inTheList = false;
        }
    }
}
