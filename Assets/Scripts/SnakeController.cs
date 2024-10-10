using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;


public class SnakeController : MonoBehaviour
{

    private Vector2 _direction = Vector2.right;
    public List<Transform> _body = new List<Transform>();
    public Transform bodyPrefab;
    public int initialSize = 4;
    float xBoundLeft = -23f;
    float xBoundRight = 23f;
    float yBoundUp = 11f;
    float yBoundDown = -11f;
    //public float speed = 1.0f;
    public float speed;  // Lower value means faster movement (time between moves)
    public float startingSpeed = 2.0f;  // The snake's starting speed
    public float speedIncrement = 0.2f;  // Speed increase after each food
    public float maxSpeed = 5.0f;  // Maximum speed
    public int pendingGrowth;

    //public int score = 0;
    //public TMP_Text scoreText;

    private GameManager gameManager; // get the game manager reference

    //Rigidbody rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        speed = startingSpeed;
        //rb = GetComponent<Rigidbody>();
        ResetState();
        //StartCoroutine(MoveSnake());
        gameManager.UpdateSnakeScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && _direction != Vector2.down)
        {
            _direction = Vector2.up;         
        }
        if (Input.GetKeyDown(KeyCode.S) && _direction != Vector2.up) 
        { _direction = Vector2.down; }
        if (Input.GetKeyDown(KeyCode.D) && _direction != Vector2.left) 
        { _direction = Vector2.right; }
        if (Input.GetKeyDown(KeyCode.A) && _direction != Vector2.right) 
        { _direction = Vector2.left; }

        if (transform.position.x < xBoundLeft)
        {
            // Move players to the right wall
            transform.position = new Vector3(xBoundRight, transform.position.y, transform.position.z);

        }
        else if (transform.position.x > xBoundRight)
        {
            // Move player to the left wall
            transform.position = new Vector3(xBoundLeft, transform.position.y, transform.position.z);
        }

        if (transform.position.y < yBoundDown)
        {
            // Move players to the up wall
            transform.position = new Vector3(transform.position.x,yBoundUp, transform.position.z);

        }
        else if (transform.position.y > yBoundUp)
        {
            // Move player to the down wall
            transform.position = new Vector3(transform.position.x, yBoundDown, transform.position.z);
        }

    }

    //private IEnumerator MoveSnake()
    //{
    //    while (true)
    //    {
    //        // Move the body parts behind the head
    //        for (int i = _body.Count - 1; i > 0; i--)
    //        {
    //            _body[i].position = _body[i - 1].position;
    //        }

    //        // Move the snake's head to the new position using Mathf.Round for grid alignment
    //        transform.position = new Vector3(
    //            Mathf.Round(transform.position.x + _direction.x),
    //            Mathf.Round(transform.position.y + _direction.y),
    //            0.0f
    //        );

    //        // Wait for the interval based on the speed before the next movement
    //        yield return new WaitForSeconds(speed);
    //    }
    //}
    private void FixedUpdate()
    {
        // position body parts behind head
        for (int i = _body.Count - 1; i > 0; i--)
        {
            _body[i].position = _body[i - 1].position;
        }

        if (pendingGrowth > 0)
        {
            for (int i = 0; i < pendingGrowth; i++)
            {
                Grow();
                gameManager.snakeScore++;
                gameManager.UpdateSnakeScoreText();
                pendingGrowth--;
            }
        }

        // update snakes position
        transform.position = new Vector3(
            Mathf.Round(transform.position.x + _direction.x * speed * Time.fixedDeltaTime),
            Mathf.Round(transform.position.y + _direction.y * speed * Time.fixedDeltaTime),
            0.0f);
        //transform.position += new Vector3(Mathf.Round(_direction.x), _direction.y, 0.0f) * speed * Time.fixedDeltaTime;
    }


    void Grow()
    {
        // instantiate new body part
        Transform body = Instantiate(bodyPrefab);
        body.position = _body[_body.Count - 1].position;
        _body.Add(body);

        //rb.linearVelocity += new Vector3(Mathf.Round((transform.position.x + _direction.x) * speed), Mathf.Round((transform.position.y + _direction.y) * speed), 0.0f);
        //transform.Translate((transform.position.x + _direction.x), Mathf.Round((transform.position.y + _direction.y)), 0.0f * speed * Time.deltaTime);
        //speed = Mathf.Max(speed - 0.05f, maxSpeed);

        // Increase the speed with every food, but cap it at maxSpeed
        if (speed < maxSpeed)
        {
            speed += speedIncrement;
        }
    }

    public void ResetState()
    {
        for (int i = 1; i < _body.Count; i++)
        {
            Destroy(_body[i].gameObject);
        }
        _body.Clear();
        _body.Add(transform);

        // Move the snake's head to the starting position
        transform.position = new Vector3(-23, 0, 0);

        // For each body part, instantiate it at a position relative to the head
        Vector3 bodyPartPosition = transform.position;

        for (int i = 1; i < initialSize; i++)
        {
            // Offset each body part's position to the left of the previous part
            bodyPartPosition.x -= 1;  // This moves the body to the left of the head
            Transform body = Instantiate(bodyPrefab, bodyPartPosition, Quaternion.identity);
            _body.Add(body);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Food")
        {
            pendingGrowth += 1;
        }
        else if (other.tag == "Snack")
        {
            pendingGrowth += 3;
        }
        else if (other.tag == "SnakeBody")
        {
            gameManager.wormWonCollision = true;
            gameManager.GameOver();
            //ResetState();
        }
        else if (other.tag == "WormBody")
        {
            Debug.Log("Snake hit worm body: Worm wins!");
            gameManager.wormWonCollision = true;
            gameManager.GameOver();
        }
        else if (other.tag == "WormHead")
        {
            Debug.Log("SnakeController: Head to head collision detected");
            gameManager.headToHeadCollision = true;
            gameManager.GameOver();
        }
    }


    //public void UpdateScoreText()
    //{
    //    scoreText.text = "Score: " + score.ToString();
    //}

}
