using System.Collections;
using UnityEngine;

public class Food : MonoBehaviour
{
    public enum ConsumableType { Food, Snack }  // Enum to distinguish food and snack
    public ConsumableType type;

    public BoxCollider2D gridArea;
    private SnakeController[] allSnakes;

    public float minWarpTime = 5f;  // Minimum time for random interval
    public float maxWarpTime = 15f;  // Maximum time for random interval
    private float timeUntilWarp;  // Timer for next warp
                                  // For Snack behavior only
    public float snackSpawnDelay = 10f;
    public float postEatDelay = 3f;

    private bool isFirstSpawn = true;

    private void Start()
    {
        allSnakes = FindObjectsOfType<SnakeController>();

        if (type == ConsumableType.Snack)
        {
            MoveOffScreen(); // Snack starts offscreen
            StartCoroutine(SpawnAfterDelay(snackSpawnDelay)); // Snack spawns after delay
        }
        else
        {
            RandomPosition();  // Food starts immediately on screen
        }
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
        while (IsPositionOccupied(newPosition));

        transform.position = newPosition;
    }

    // Check if the generated position is occupied by the snake, worm, or other consumables
    bool IsPositionOccupied(Vector3 position)
    {
        foreach (var snake in allSnakes)
        {
            foreach (Transform bodyPart in snake._body)  // Check both snake and worm bodies
            {
                if (bodyPart.position == position)
                {
                    return true;
                }
            }
        }
        return false;
    }


    // Handles triggering behavior when either snake or worm collides with food/snack
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "SnakeHead" || other.tag == "WormHead")
        {
            if (type == ConsumableType.Food)
            {
                RandomPosition(); // Food respawns immediately
            }
            else if (type == ConsumableType.Snack)
            {
                StartCoroutine(HandleSnackEaten()); // Snack disappears and respawns after delay
            }
        }
    }

    // Moves snack offscreen when it's eaten, and respawns after a delay
    private IEnumerator HandleSnackEaten()
    {
        MoveOffScreen();
        yield return new WaitForSeconds(postEatDelay);
        StartCoroutine(SpawnAfterDelay(snackSpawnDelay));
    }

    // Move the snack off-screen
    void MoveOffScreen()
    {
        transform.position = new Vector3(-1000f, -1000f, 0f);
    }

    // Spawns the snack after a delay
    private IEnumerator SpawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RandomPosition();  // Snack spawns at a random position
        isFirstSpawn = false;
    }
}
