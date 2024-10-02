using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Collections;


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

    public int score = 0;
    public TMP_Text scoreText;

    //Rigidbody rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        speed = startingSpeed;
        //rb = GetComponent<Rigidbody>();
        ResetState();
        //StartCoroutine(MoveSnake());
        UpdateScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _direction = Vector2.up;         
        }
        if (Input.GetKeyDown(KeyCode.S)) 
        { _direction = Vector2.down; }
        if (Input.GetKeyDown(KeyCode.D)) 
        { _direction = Vector2.right; }
        if (Input.GetKeyDown(KeyCode.A)) 
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

        // update snakes position
        transform.position = new Vector3(
            Mathf.Round(transform.position.x + _direction.x * speed * Time.fixedDeltaTime),
            Mathf.Round(transform.position.y + _direction.y * speed * Time.fixedDeltaTime),
            0.0f);
        //transform.position += new Vector3(Mathf.Round(_direction.x), _direction.y, 0.0f) * speed * Time.fixedDeltaTime;
    }


    void Grow()
    {
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

    void ResetState()
    {
        for (int i = 1; i < _body.Count; i++)
        {
            Destroy(_body[i].gameObject);
        }
        _body.Clear();
        _body.Add(transform);

        for (int i = 1; i < initialSize; i++)
        {
            _body.Add(Instantiate(bodyPrefab));
        }

        // back to starting position
        transform.position = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Food")
        {
            Grow();
            score++;
            UpdateScoreText();
        } else if (other.tag == "Body")
        {
            ResetState();
        }

    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

}
