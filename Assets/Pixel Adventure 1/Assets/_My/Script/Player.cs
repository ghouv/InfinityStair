using UnityEngine;
//마우스 클릭과 버튼을 따로 해두고 싶어서 사용
using UnityEngine.EventSystems;

public class Player : GameEntity
{
    [SerializeField] private AudioSource sound;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private RectTransform timeBar; //시간
    
    private bool isTurn = false;
    private bool isDead = false;
    private int moveCount = 0;
    private int spawnIndex = 0;
    private int turnIndex = 0;
    private int score = 0;

    public int Score => score;

    private const float realMaxTime = 5f;   // 처음 초기 시간 5초 
    
    private float timeLeft = realMaxTime; 
    private float maxTime = realMaxTime;
    
    private void Start()
    {
        ResetPlayer();
    }

    // 오버플로우로 터져서 public 변경 후
    // GameManger 의 Restart() 수정
    public void ResetPlayer()
    {
        isTurn = false;
        isDead = false;
        moveCount = 0;
        spawnIndex = 0;
        turnIndex = 0;
        score = 0;

        animator.SetBool("Die", false);
        spriteRenderer.flipX = false;
        ResetEntityPosition();
        
        maxTime = realMaxTime;
        timeLeft = maxTime;
        if (timeBar != null)
        {
            timeBar.localScale = Vector3.one;
        }
    }

    public void Turn()
    {
        isTurn = !isTurn;
        spriteRenderer.flipX = isTurn;
    }

    public void Move()
    {
        if (isDead) return;

        sound.Play();
        moveCount++;

        currentPosition += isTurn ? new Vector3(-0.75f, 0.5f, 0) : new Vector3(0.75f, 0.5f, 0);
        transform.position = currentPosition;
        animator.SetTrigger("Move");

        if (GameManager.Instance.StairManager.IsWrongTurn(turnIndex, isTurn))
        {
            Die();
            return;
        }

        turnIndex = (turnIndex + 1) % GameManager.Instance.StairManager.TotalStairs;

        if (moveCount > 5)
        {
            GameManager.Instance.StairManager.SpawnStair(spawnIndex);
            spawnIndex = (spawnIndex + 1) % GameManager.Instance.StairManager.TotalStairs;
        }

        score++;
        GameManager.Instance.UpdateScore(score);

        maxTime -= 0.01f;   //한 칸씩 올라 갈 때마다 0.01초씩 시간 감축
        if (maxTime < 0.5f) maxTime = 0.5f; // 너무 힘들까봐 0.5초 보장
        timeLeft = maxTime;
        

    }

    private void Die()
    {
        isDead = true;
        animator.SetBool("Die", true);
        GameManager.Instance.GameOver();
    }

    // 시간 감소 + 사망 
    private void Update()
    {
        if (isDead) return;

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0)) Move();
            if (Input.GetMouseButtonDown(1)) Turn();
        }
        
        timeLeft -= Time.deltaTime;

        if (timeBar != null)
        {
            float ratio = Mathf.Clamp01(timeLeft / maxTime);
            timeBar.localScale = new Vector3(ratio, 1f, 1f);
        }

        if (timeLeft <= 0f)
        {
            Die();
        }
        
    }

}