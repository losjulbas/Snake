using System.Collections;
using UnityEngine;

public class Snack : MonoBehaviour
{
    public BoxCollider2D gridArea;
    private SnakeController snakeController;
    private WormController wormController;
    private Food food;

    public float minWarpTime = 5f;  // Minimum time for random interval
    public float maxWarpTime = 15f;  // Maximum time for random interval
    private float timeUntilWarp;  // Timer for next warp

    public float snackSpawnDelay = 10f;  // Time until snack first spawns
    public float postEatDelay = 3f;  // Delay after being eaten before spawning again

    private bool isFirstSpawn = true;  // To handle the initial spawn logic

    private void Start()
    {
        snakeController = FindAnyObjectByType<SnakeController>();
        wormController = FindAnyObjectByType<WormController>();
        food = FindAnyObjectByType<Food>();

        MoveSnackOffScreen();

        // Start the initial spawn delay
        StartCoroutine(SpawnSnackAfterDelay(snackSpawnDelay));
    }

    // Coroutine to handle delayed spawn
    private IEnumerator SpawnSnackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Spawn the snack for the first time or after it's been eaten
        RandomPosition();
        ResetWarpTimer();  // Start the warp timer for snack warping
        isFirstSpawn = false;  // First spawn done
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

    private void Update()
    {
        if (isFirstSpawn) return;  // Skip warp logic before the first spawn

        timeUntilWarp -= Time.deltaTime;

        if (timeUntilWarp <= 0)
        {
            // Warp to a new position if the snack hasn't been eaten
            RandomPosition();
            ResetWarpTimer();
        }
    }

    void ResetWarpTimer()
    {
        timeUntilWarp = Random.Range(minWarpTime, maxWarpTime);
    }

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

        if (food != null && food.transform.position == position)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "SnakeHead" || other.tag == "WormHead")
        {
            // Start the post-eat delay coroutine
            StartCoroutine(HandleSnackEaten());
        }
    }

    // Coroutine to handle snack being eaten and respawning after a delay
    private IEnumerator HandleSnackEaten()
    {
        MoveSnackOffScreen();  // Move snack off-screen when eaten
        yield return new WaitForSeconds(postEatDelay);  // Wait for the post-eat delay
        StartCoroutine(SpawnSnackAfterDelay(snackSpawnDelay));  // Respawn after the delay
    }

    // Move the snack off-screen instead of disabling it
    private void MoveSnackOffScreen()
    {
        transform.position = new Vector3(-1000f, -1000f, 0f);  // Move off-screen
    }
}
