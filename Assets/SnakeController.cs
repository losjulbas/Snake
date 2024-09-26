using Unity.VisualScripting;
using UnityEngine;

public class SnakeController : MonoBehaviour
{

    private Vector2 _direction = Vector2.right;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(Mathf.Round(transform.position.x + _direction.x), Mathf.Round(transform.position.y + _direction.y), 0.0f);
    }
}
