using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor.Experimental.GraphView;


public class SnakeController : MonoBehaviour
{
    public bool isSnake; // True for snake, false for worm
    private Vector2 _direction;
    public List<Transform> _body = new List<Transform>();
    private LinkedList<Vector2> dirChanges = new LinkedList<Vector2>();

    public Transform bodyPrefab;
    public int initialSize = 4;
    float xBoundLeft = -23f;
    float xBoundRight = 23f;
    float yBoundUp = 11f;
    float yBoundDown = -11f;

    public float speed;  // Lower value means faster movement (time between moves)
    public float speedIncrement = 0.01f;  // Speed increase after each food
    public float maxSpeed = 0.05f;  // Maximum speed
    public int pendingGrowth;

    private GameManager gameManager; // get the game manager reference


   
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        _direction = isSnake ? Vector2.right : Vector2.left;
        ResetState();
        gameManager.UpdateSnakeScoreText();
        StartCoroutine(MoveSnake());
    }

    void Update()
    {
        // Handle inputs based on character type
        if (isSnake)
        {
            HandleSnakeInput();
        }
        else
        {
            HandleWormInput();
        }

        // Boundary and growth management
        HandleBounds();
        HandleGrowth();
    }

    private void HandleSnakeInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeDirection(Vector2.up);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeDirection(Vector2.down);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeDirection(Vector2.right);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeDirection(Vector2.left);
        }
    }

    private void HandleWormInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeDirection(Vector2.up);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeDirection(Vector2.down);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeDirection(Vector2.right);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeDirection(Vector2.left);
        }
    }

    private void HandleBounds()
    {
        if (transform.position.x < xBoundLeft)
        {
            transform.position = new Vector3(xBoundRight, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > xBoundRight)
        {
            transform.position = new Vector3(xBoundLeft, transform.position.y, transform.position.z);
        }

        if (transform.position.y < yBoundDown)
        {
            transform.position = new Vector3(transform.position.x, yBoundUp, transform.position.z);
        }
        else if (transform.position.y > yBoundUp)
        {
            transform.position = new Vector3(transform.position.x, yBoundDown, transform.position.z);
        }
    }

    private void HandleGrowth()
    {
        if (pendingGrowth > 0)
        {
            for (int i = 0; i < pendingGrowth; i++)
            {
                Grow();
                if (isSnake)
                {
                    gameManager.snakeScore++;
                }
                else
                {
                    gameManager.wormScore++;
                    gameManager.UpdateWormScoreText();
                }
                gameManager.UpdateSnakeScoreText();
                pendingGrowth--;
            }
        }
    }

    // helper variable
    private Vector2 GetLastDirection()
    {
        if (dirChanges.Count == 0) // keep current direction
        {
            return _direction;
        }
        return dirChanges.Last.Value; //returns the last or the most recently made direction change
    }

    private bool CanChangeDirection(Vector2 newDir)
    {
        if (dirChanges.Count == 2) // max buffer limit
        {
            return false;
        }

        Vector2 lastDir = GetLastDirection(); // last direction made
        return newDir != lastDir && newDir != GetOppositeDirection(lastDir); // return true if the new direction is not the same than last direction or opposite direction
    }


    // helper variable for the opposite direction above
    private Vector2 GetOppositeDirection(Vector2 direction)
    {
        return -direction;
    }

    public void ChangeDirection(Vector2 dir)
    {
        
        if (CanChangeDirection(dir)) //if move is legal
        {
            dirChanges.AddLast(dir); //add to the buffer
        }
    }
    private IEnumerator MoveSnake()
    {
        while (true)
        {
            // Move the body parts behind the head
            for (int i = _body.Count - 1; i > 0; i--)
            {
                _body[i].position = _body[i - 1].position;
            }


            // Only process one input per movement
            if (dirChanges.Count > 0)
            {
                Vector2 nextDirection = dirChanges.First.Value;
                dirChanges.RemoveFirst();

                // Ensure the snake doesn't collapse into itself
                if (nextDirection != _direction && nextDirection != -_direction)
                {
                    _direction = nextDirection;
                }
            }

            // Move the snake's head to the new position using Mathf.Round for grid alignment
            transform.position = new Vector3(
                Mathf.Round(transform.position.x + _direction.x),
                Mathf.Round(transform.position.y + _direction.y),
                0.0f
            );

            //_lastDirection = _direction;
            // Wait for the interval based on the speed before the next movement
            yield return new WaitForSeconds(speed);
        }


    }

    void Grow()
    {
        // instantiate new body part
        Transform body = Instantiate(bodyPrefab);
        body.position = _body[_body.Count - 1].position;
        _body.Add(body);

        // Increase the speed with every food, but cap it at maxSpeed
        if (speed > maxSpeed)
        {
            speed -= speedIncrement;
        }
    }

    public void ResetState()
    {
        // Destroy the existing body parts (except the head)
        for (int i = 1; i < _body.Count; i++)
        {
            Destroy(_body[i].gameObject);
        }
        _body.Clear();
        _body.Add(transform); // Add head back

        // Set different starting positions for snake and worm
        transform.position = new Vector3(isSnake ? -23 : 23, 0, 0);

        Vector3 bodyPartPosition = transform.position;
        for (int i = 1; i < initialSize; i++)
        {
            bodyPartPosition.x += isSnake ? -1 : 1; // Different directions for snake and worm
            Transform body = Instantiate(bodyPrefab, bodyPartPosition, Quaternion.identity);
            _body.Add(body);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            pendingGrowth += 1;
        }
        else if (other.CompareTag("Snack"))
        {
            pendingGrowth += 3;
        }
        else if (other.CompareTag("SnakeBody") || other.CompareTag("WormBody"))
        {
            if (isSnake)
            {
                gameManager.wormWonCollision = true;
            }
            else
            {
                gameManager.snakeWonCollision = true;
            }
            gameManager.GameOver();
        }
        else if (other.CompareTag("SnakeHead")  || other.CompareTag("WormHead"))
        {
            gameManager.headToHeadCollision = true;
            gameManager.GameOver();
        }
    }
}
