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
    
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;
        oldPosition = transform.localPosition;
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CharTurn();
        }    
        else if (Input.GetMouseButtonDown(0))
        {
            CharMove();
        }
    }

    
    private void CharTurn()
    {
        isTurn = isTurn == true ? false : true;
        
        spriteRenderer.flipX = isTurn;
    }

    private void CharMove()
    {
        moveCount++;
        
        MoveDirection();
        
        // 잘못된 방향으로 가면 사망
        if (isFailTurn())
        {
            anim.SetBool("Die", true);
            return;
        }

        if (moveCount > 5)
        {
            // 계단 스폰
            RespawnStair();
        }
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
}
