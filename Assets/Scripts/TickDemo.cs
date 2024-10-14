using UnityEngine;

public class TickDemo : MonoBehaviour
{
    public float tickTime = 2f;
    float timer = 0f;

    private string tickString = "TICK";

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= tickTime)
        {
            print(tickString);
            //timer = 0;
            timer %= tickTime;
        }
    }
}

