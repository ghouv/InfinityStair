using UnityEngine;

//위치 정보 
public class GameEntity : MonoBehaviour
{
    protected Vector3 startPosition;
    protected Vector3 currentPosition;

    protected virtual void Awake()
    {
        startPosition = transform.position;
        currentPosition = startPosition;
    }

    protected void ResetEntityPosition()
    {
        transform.position = startPosition;
        currentPosition = startPosition;
    }
}