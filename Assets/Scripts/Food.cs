using System.Collections;
using UnityEngine;

public class Food : MonoBehaviour
{

    public BoxCollider2D gridArea;
    private SnakeController snakeController;
    private WormController wormController;
    private Snack snack;

    public float minWarpTime = 5f;  // Minimum time for random interval
    public float maxWarpTime = 15f;  // Maximum time for random interval
    private float timeUntilWarp;  // Timer for next warp
    private void Start()
    {
        snakeController = FindAnyObjectByType<SnakeController>();
        wormController = FindAnyObjectByType<WormController>();
        snack = FindAnyObjectByType<Snack>();
        RandomPosition();
        //ResetTimer();

    }

    void RandomPosition()
    {
        Bounds bounds = gridArea.bounds;
        Vector3 newPosition;

        do
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            newPosition = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);
        }
        // Loop until a position is found that is not occupied by the snake's body
        while (IsPositionOccupied(newPosition));

        transform.position = newPosition;
    }

    //private void Update()
    //{
    //    timeUntilWarp -= Time.deltaTime;

    //    if (timeUntilWarp <= 0)
    //    {
    //        RandomPosition();
    //        ResetTimer();

    //    }
    //}

    //void ResetTimer()
    //{
    //    timeUntilWarp = Random.Range(minWarpTime, maxWarpTime);
    //}

    bool IsPositionOccupied(Vector3 position)
    {
        foreach (Transform bodyPart in snakeController._body)
        {
            if (bodyPart.position == position)
            {
                return true;    
            }
        }

        foreach (Transform bodyPart in wormController._wormBody)
        {
            if (bodyPart.position == position)
            {
                return true;
            }
        }

        if (snack != null && snack.transform.position == position)
        {
            return true;
        }
        return false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "SnakeHead" || other.tag == "WormHead")
        {
            RandomPosition();
        }

    }
}
