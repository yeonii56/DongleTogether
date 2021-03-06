# ❤모여라 동글❤
### 👉 Casual Puzle Game
> 동글들을 선으로 이어서 가장 높은 점수를 얻는 안드로이드 모바일 게임입니다.

![image](https://user-images.githubusercontent.com/90385816/164004216-3fcb0950-0139-404c-a613-dbcf6536fb5a.png)

---

## 👉 프로젝트 소개
- 게임 장르 : 물리기반 2D 퍼즐
- 플랫폼 : 안드로이드 모바일
- 제작 인원 : 1 인 (개인 프로젝트, 디자인과 개발 모두 직접 제작)
- 제작 기간 : 10 일 (3/2 ~ 3/16)
- 사용 엔진과 언어 : Unity, C#
<br></br>

## 👉 플레이 영상(유튜브)
https://youtu.be/eKXq6QrJJ98
<br></br>

## 👉 포트폴리오
- 제가 이 프로젝트로 배우고 경험해보고 싶었던 부분은 다음과 같습니다.  
  1️⃣ GC 를 생각하며 작성하기  
  2️⃣ 최적화에 신경쓰기.  
  3️⃣ 디자인 패턴을 활용하여 코드의 설계성을 높이기.
 <br></br>      
 
> ### ❣ 게임 로직 설명
  [DongleTogether_게임로직.pdf](https://github.com/yeonii56/DongleTogether/files/8505497/DongleTogether_.pdf)

 - 동글을 터치로 이어서 터트림
 - 타이머가 다 닳으면 게임 종료
 - 동글을 터트리면 점수와 시간을 획득
 - 많이 이어서 터트릴 수록 높은 점수 획득
 - 섞기와 힌트 버튼을 활용하여 더 높은 점수를 획득할 수 있음  
 <br></br>
 
> ### ❣ 코드의 성능  

`💙 가독성`  
  - C#의 코드 작성 규칙을 지켜서 작성하였습니다. (변수명은 소문자로 시작, 함수는 대문자 등)
  - 싱글턴패턴으로 자주 사용하는  instance들은 `_`언더바를 사용하여 가독성을 높였습니다.
  - 내용이 너무 길어지지 않도록 함수들은 기능별로 세분화하였습니다.     

`💙 무결성`  
  - bool 값을 지정하여 동글이 터지는 동안은 터치되지 않으며 게임이 진행될 수 없게 했습니다.
  - 동글이 터지고 추가되기 때문에 배열이 아닌 리스트로 작성했습니다.   

`💙 유연성` `💙 재사용성`

  - 하드코딩을 지양하고 모든 상수를 `Define.cs` 파일 한 군데에만 다 모아놓았기 때문에, 게임에 필요한 상수값을 변경할 땐 이 파일만 수정하면 되도록 작성했습니다.
  - 여러 게임 타입들을 Enum 으로 `Define.cs`에 정리를 해두어 유용하게 사용하였는데, Enum 을 foreach 문을 사용하여 로드하도록 하였기 때문에 마찬가지로 이 `Define.cs` 안의 Enum 만 변경해주면 됩니다.   
 <br></br>      

> ### ❣ 최적화  

`💚 오브젝트 풀링` 
  - 동글이 파괴되고 생성되는 것이 반복되기 때문에 이를 instantiate나 destroy로 파괴하거나 새로 생성하는 것이 아닌, 풀링을 이용하여 재사용하였습니다.
  - 데이터의 검색이 필요하지 않고, 메모리의 크기가 동적이고, 데이터의 흐름을 잘 파악할 수 있기 때문에 큐를 사용하였습니다.    

`💚 드로우 콜 줄이기`
  - 리소스를 직접 디자인하여 가져올 때 부터 하나의 png파일에 여러 리소스를 넣어두고 multiple sprite로 사용할 수 있게 했습니다.
  - 같은 머테리얼을 사용하지만 다른 텍스쳐 파일을 갖고있는 리소스들을 sprite atlas 기능을 이용하여 하나의 텍스쳐로 묶어주었습니다.   

`💚 Update()는 지양`
  - 이 게임은 시간을 계속 업데이트 해야 하며 시간이 흐르는 동안 진행되는 게임입니다. 따라서 시간의 제어와 관련된 구문을 제외하고는 Update 함수를 사용하는 대신 코루틴을 사용했습니다.
  - 이벤트가 일어나는 방식도 Update 함수를 사용하지 않고, 다른 이벤트가 일어났을 시에 일어나도록 연결했습니다. ex) 동글이 터질때만 점수가 변하도록 하였습니다.   

`💚 가비지를 줄이기 위한 캐싱`
  - 코루틴을 많이 사용하기 때문에 매번 생성되는 new WaitSeconds 객체 가비지를 막고자, 대기 시간 별 YieldInstruction 객체를 미리 캐싱해두고 사용했습니다. (코루틴 매니저를 싱클톤으로 활용하여 같은 대기 시간은 같은 객체를 사용하도록 하였습니다.)
  - Transform이나 GameObject 같은 경우에도 미리 캐싱해서 사용했습니다.   
<br></br>

> ### ❣ Scene 관리 

`💛 하나의 Scene`
  - 씬이 넘어가는 과정에서 데이터를 새로 가져오며 렉이 발생할 수 있으므로, 게임 시작 시에 한번만 데이터를 가져오며 플레이되도록 하나의 씬만 사용했습니다.
  - 하나의 씬이지만 각 화면은 캔버스의 패널로 나뉘게 했습니다.   

<br></br>

> ### ❣ 구현 내용
`💜 Manager`
  - 프로젝트 내에서 전체적으로 자주 사용 될 여러 매니저 클래스들을 작성한 후, 이들의 인스턴스를 `Managers` 클래스의 프로퍼티로 한 데 모아 싱글톤으로 관리헸습니다.

`💜 시작 화면`
  - 사운드 버튼
    - SoundManager로 배경음과 효과음의 SoundSource로 관리하여 슬리아더로 볼륨을 조절할 수 있도록 했습니다.
    - 여러 효과음은 하나의 SoundSource로 관리했습니다.  
    
  - 시작 버튼
    - 게임시작. 게임화면으로 넘어감(하나의 씬이므로 SceneManager대신 SetActive()이용)
    - 시간이 흐르기 시작합니다.  
    
  - 설명 버튼
     - 게임 설명 패널 활성화  
     
  - 나가기 버튼
    - 게임 종료
 
`💜 게임 화면`
  - 동글 배치
    - 5x5로 리스트를 이용해 동글을 배치했습니다.(생성과 파괴시에 동적으로 동글의 개수가 동적으로 변하기떄문에 리스트 이용함)  
    
  - 동글 생성과 파괴
    - 오브젝트 풀링을 이용하여 동글이 파괴될 시에 큐에 넣고 새로 생성될 때 큐에서 꺼내서 재사용하도록 했습니다.
    - Unity의 물리 기능을 이용하여 파괴된 동글의 위에 있던 동글이 아래로 자연스럽게 떨어지도록 했습니다.
   
  - 동글 잇기
    - Touch.Phase 이용
    - 터치로 이어진 동글이 2개 이상일 경우에만 동글이 터지도록 했습니다.
    - 터치된 동글이 1개이거나, 여러개여도 다시 전의 동글로 돌아갈 경우 라인이 취소되도록 했습니다.
    - 플레이어가 터치된 동글을 알 수 있도록 크기가 커지며 효과음이 나도록 했습니다.
   
  - 점수 표시
    - PlayerPrefs를 이용해서 최고 점수를 기록했습니다. 현재 점수와 최고 점수를 비교하여 현재 점수가 최고 점수일 경우 현재 점수와 최고 점수가 같이 오르도록 했습니다.
    - 터치르 뗀 부분의 포지션을 가져와서 그 위치에 방금 터진 동글의 점수를 표시해 시각적 효과를 더했습니다.
   
  - 점수 계산
    - 점수는 터진 동글의 개수만큼 2의 제곱수로 오르도록 했습니다.
    - 또한 각 점수마다 일정 시간만큼이 채워지도록하여 재미 요소를 더했습니다.

  - 섞기 버튼
    - 각 동글이 초기화되어 색이 다시 지정되도록 했습니다.

  - 힌트 버튼
    - 리스트 이용
    - 25번 가장 길게 잇는 것을 판별하는 것은 비효율적이기 때문에 가장 많이 같은 색의 동글이 붙어 있는 리스트를 판별한 뒤 그 리스트 안에 동글들 중에서만 가장 길게 이을 수 있는 라인을 판별했습니다.
    - 힌트 리스트에 들어간 동글은 코루틴을 이용하여 일시적으로 커지게하여서 나타냈고, 라인을 이어주지 않아서 플레이어가 어떻게 라인을 이을지는 직접 생각하도록 했습니다.
    - 동글이 섞이거나 파괴될 시에 리스트를 초기화해주어서 오류가 일어나지 않도록 했습니다.

  - 일시정지 버튼
    - 일시정지 버튼을 누를 시에 시간이 멈추고 그 동안은 동글들을 볼 수 없도록 화면이 가려지게 했습니다.
    - 일시정지 패널을 버튼의 자식으로 두어 아무 화면이나 누르면 게임이 이어지도록 했습니다.

`💜 종료 화면`
  - 최고 점수와 나의 점수를 나타내도록 했습니다.
  - 게임을 다시 시작하거나 종료할 수 있게 구성했습니다.
    





