using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Vector3 startPosition;
    private Vector3 oldPosition;
    public bool isTurn = false;

    private int moveCount = 0;
    private int turnCount = 0;
    private int spawnCount = 0;     // 계단 증가 변수

    private bool isDie = false;     // 캐릭터의 생사 유무 판단 
    
    private AudioSource sound;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sound = GetComponent<AudioSource>();
        
        //재시작시 처음 위치 귀환
        startPosition = transform.position;
        
        Init();
        

    }

    private void Init()
    {
        anim.SetBool("Die", false);
        transform.position = startPosition;
        oldPosition = startPosition;
        moveCount = 0;
        spawnCount = 0;
        turnCount = 0;
        isTurn = false;
        spriteRenderer.flipX = isTurn;
        isDie = false;
    }
    
    
    
    public void CharTurn()
    {
        isTurn = isTurn == true ? false : true;
        
        spriteRenderer.flipX = isTurn;
    }

    public void CharMove()
    {
        //캐릭터 사망시 추가 행동 금지
        if(isDie)
            return;
        
        //캐릭터 이동 할때 사운드
        sound.Play();
        
        moveCount++;
        
        MoveDirection();
        
        // 잘못된 방향으로 가면 사망
        if (isFailTurn())
        {
            CharDie();
            return;
        }

        if (moveCount > 5)
        {
            // 계단 스폰
            RespawnStair();
        }
        
        GameManager.Instance.AddScore();
    }

    private void MoveDirection()
    {
        if (isTurn) //left
        {
            oldPosition += new Vector3(-0.75f, 0.5f, 0);
        }
        else
        {
            oldPosition += new Vector3(0.75f, 0.5f, 0);
        }

        transform.position = oldPosition;
        anim.SetTrigger("Move");
    }

    private bool isFailTurn()
    {
        bool result = false;
        if (GameManager.Instance.isTurn[turnCount] != isTurn)
        {
            result = true;
        } 

        turnCount++;

        if (turnCount > GameManager.Instance.Stairs.Length - 1) //계단 20개
        {
            turnCount = 0;
        }
        
        return result; 
    }

    private void RespawnStair()
    {
        GameManager.Instance.SpawnStair(spawnCount);
        
        spawnCount++;

        if (spawnCount > GameManager.Instance.Stairs.Length - 1)
        {
            spawnCount = 0;
        }
    }

    // 캐릭터 죽는 모션
    private void CharDie()
    {
        //캐릭터 사망시 창 띄우기
        GameManager.Instance.GameOver();
        
        anim.SetBool("Die", true);
        isDie = true;
    }

    public void ButtonRestart()
    {
        Init();
        GameManager.Instance.Init();
        GameManager.Instance.InitStairs();
    }
}
