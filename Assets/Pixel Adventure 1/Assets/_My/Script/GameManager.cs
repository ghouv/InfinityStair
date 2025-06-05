using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Player player;
    [SerializeField] private StairManager stairManager;
    [SerializeField] private AudioManager audioManager;

    [SerializeField] private TextMeshProUGUI scoreText;     // 실시간 점수
    [SerializeField] private GameObject gameOverUI;        
    [SerializeField] private TextMeshProUGUI maxScoreText;  // 최고 점수
    [SerializeField] private TextMeshProUGUI nowScoreText;  // 현재 점수

    private int maxScore = 0;

    public StairManager StairManager => stairManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        stairManager.InitStairs();
        audioManager.PlayBGM();
        UpdateScore(0);
        gameOverUI.SetActive(false); // Restart 시 Game Over 창 닫기
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    public void GameOver()
    {
        audioManager.PlayDieSound();

        int currentScore = player.Score;
        if (currentScore > maxScore)
        {
            maxScore = currentScore;
        }

        if (nowScoreText != null)
            nowScoreText.text = currentScore.ToString();

        if (maxScoreText != null)
            maxScoreText.text = maxScore.ToString();

        gameOverUI.SetActive(true);
    }

    public void RestartGame()
    {
        player.ResetPlayer();
        stairManager.InitStairs();
        audioManager.PlayBGM();
        UpdateScore(0);
        gameOverUI.SetActive(false);

    }
}