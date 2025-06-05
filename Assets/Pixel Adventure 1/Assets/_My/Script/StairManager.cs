using UnityEngine;

public class StairManager : MonoBehaviour
{
    [SerializeField] private GameObject[] stairs;
    private bool[] isTurn;
    private Vector3 lastPosition;

    private enum Direction { Start, Left, Right }
    private Direction direction;

    public int TotalStairs => stairs.Length;

    private void Awake()
    {
        isTurn = new bool[stairs.Length];
    }

    public void InitStairs()
    {
        direction = Direction.Start;
        lastPosition = Vector3.zero;

        for (int i = 0; i < stairs.Length; i++)
        {
            Vector3 pos = direction switch
            {
                Direction.Start => new Vector3(0.75f, -0.1f, 0),
                Direction.Left => lastPosition + new Vector3(-0.75f, 0.5f, 0),
                Direction.Right => lastPosition + new Vector3(0.75f, 0.5f, 0),
                _ => Vector3.zero
            };

            stairs[i].transform.position = pos;
            isTurn[i] = direction == Direction.Left;
            lastPosition = pos;

            if (i > 0 && Random.Range(0, 5) < 2 && i < stairs.Length - 1)
            {
                direction = direction == Direction.Left ? Direction.Right : Direction.Left;
            }
            else if (direction == Direction.Start)
            {
                direction = Direction.Right;
            }
        }
    }

    public void SpawnStair(int index)
    {
        if (Random.Range(0, 5) < 2)
        {
            direction = direction == Direction.Left ? Direction.Right : Direction.Left;
        }

        Vector3 pos = direction == Direction.Left
            ? lastPosition + new Vector3(-0.75f, 0.5f, 0)
            : lastPosition + new Vector3(0.75f, 0.5f, 0);

        stairs[index].transform.position = pos;
        isTurn[index] = direction == Direction.Left;
        lastPosition = pos;
    }

    public bool IsWrongTurn(int index, bool playerTurn)
    {
        return isTurn[index] != playerTurn;
    }
}