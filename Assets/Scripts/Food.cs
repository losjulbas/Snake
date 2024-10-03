using UnityEngine;

public class Food : MonoBehaviour
{

    public BoxCollider2D gridArea;
    private SnakeController snakeController;
    private void Start()
    {
        snakeController = FindAnyObjectByType<SnakeController>();
        RandomPosition();
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
        while (CheckSnakePosition(newPosition));

        transform.position = newPosition;
    }


    bool CheckSnakePosition(Vector3 position)
    {
        foreach (Transform bodyPart in snakeController._body)
        {
            if (bodyPart.position == position)
            {
                return true;    
            }
        }
        return false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            RandomPosition();
        }

    }
}
