using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("계단")]
    [Space(10)] 
    public GameObject[] Stairs;
    public bool[] isTurn;
    
    private enum State {Start, Left, Right};
    private State state;
    private Vector3 oldPosition;
    
    [Header("UI")]
    [Space(10)]
    public GameObject UI_GameOver;
    public TextMeshProUGUI textMaxScore;
    public TextMeshProUGUI textNowScore;
    public TextMeshProUGUI textShowScore;
    private int maxScore = 0;
    private int nowScore = 0;
    
    //사운드
    [Header("Audio")] 
    [Space(10)] 
    private AudioSource sound;
    public AudioClip bgmSound;
    public AudioClip dieSound;
    
    
    void Start()
    {
        Instance = this;
        
        sound = GetComponent<AudioSource>();
        
        Init();
        InitStairs();
    }

    public void Init()
    {
        state = State.Start;
        oldPosition = Vector3.zero;
        
        isTurn = new bool[Stairs.Length];

        for (int i = 0; i < Stairs.Length; i++)
        {
            Stairs[i].transform.position = Vector3.zero;
            isTurn[i] = false;
        }
        nowScore = 0;
        
        textShowScore.text = nowScore.ToString();
        
        //사망시 비활성화
        UI_GameOver.SetActive(false);
        
        //시작 시 배경음 활성화
        sound.clip = bgmSound;
        sound.Play();
        sound.loop = true;  //반복 재생
        
        //사운드 볼륨 조절
        sound.volume = 0.4f;
    }
    
    
    public void InitStairs()
    {
        for (int i = 0; i < Stairs.Length; i++)
        {
            switch (state)
            {
                case State.Start:
                    Stairs[i].transform.position = new Vector3(0.75f, -0.1f, 0);
                    state = State.Right;
                    break;
                case State.Left:
                    Stairs[i].transform.position = oldPosition + new Vector3(-0.75f, 0.5f, 0);
                    isTurn[i] = true;
                    break;
                case State.Right:
                    Stairs[i].transform.position = oldPosition + new Vector3(0.75f, 0.5f, 0);
                    isTurn[i] = false;
                    break;
            }
            oldPosition = Stairs[i].transform.position;

            if (i != 0)
            {
                int ran = Random.Range(0, 5);

                if (ran < 2 && i < Stairs.Length - 1)
                {
                    state = state == State.Left ? State.Right : State.Left;
                }
            }
        }
    }

    public void SpawnStair(int count)
    {
        int ran = Random.Range(0, 5);
        
        if (ran < 2)
        {
            state = state == State.Left ? State.Right : State.Left;
        }
        
        switch (state)
        {
            case State.Left:
                Stairs[count].transform.position = oldPosition + new Vector3(-0.75f, 0.5f, 0);
                isTurn[count] = true;
                break;
            case State.Right:
                Stairs[count].transform.position = oldPosition + new Vector3(0.75f, 0.5f, 0);
                isTurn[count] = false;
                break;
        }
        oldPosition = Stairs[count].transform.position;

    }

    public void GameOver()
    {
        //사망 시 배경음
        sound.loop = false; //반복 재생 해제
        sound.Stop();
        sound.clip = dieSound;
        sound.Play();
        
        //사망 시 볼륨
        sound.volume = 1;
        
        //애니메이션이 끝나는 타이밍
        StartCoroutine(ShowGameOver());
    }
    
    //Coroutine
    IEnumerator ShowGameOver()
    {
        //1초 뒤에 아래 작성한 코드로 이동하라
        yield return new WaitForSeconds(1f);
        
        UI_GameOver.SetActive(true);

        if (nowScore > maxScore)
        {
            maxScore = nowScore;
        }
        textMaxScore.text = maxScore.ToString();
        textNowScore.text = nowScore.ToString();
    }
    
    //계단 오를때 점수 추가
    public void AddScore()
    {
        nowScore++;
        textShowScore.text = nowScore.ToString();
    }
}
