using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float smoothing = 0.1f;
    public Ball ball;

    private Vector3 offset;
    private float distance;
    private float rotationLevel = 0.0f;
    private float screenHalfWidth = Screen.width / 2;

    void Start()
    {
        setDefaultOffset();
        distance = Vector3.Distance(transform.position, ball.transform.position);
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            offset = transform.position - ball.transform.position;
        }

        if (Input.GetMouseButton(1))
        {
            rotate();
            transform.LookAt(ball.transform.position);
        }
        else
        {
            moveToBall(ball.transform.position + offset);
            smoothLookAtBall();
        }
    }

    private void rotate()
    {
        var mouseDistance = (Input.mousePosition.x - screenHalfWidth) / screenHalfWidth;
        
        rotationLevel += Mathf.Clamp(mouseDistance, -1.0f, 1.0f) * Time.deltaTime * 5.0f;
    
        float x = ball.transform.position.x + Mathf.Cos(rotationLevel) * distance;
        float z = ball.transform.position.z + Mathf.Sin(rotationLevel) * distance;

        Vector3 vector = new Vector3(x, ball.transform.position.y + offset.y, z);

        moveToBall(vector);
    }

    private void moveToBall(Vector3 newPosition)
    {
        transform.position = Vector3.Lerp(transform.position, newPosition, smoothing * Time.deltaTime);
    }

    private void smoothLookAtBall()
    {
        Vector3 direction = ball.transform.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, smoothing * Time.deltaTime);
    }

    public void SetBall(Ball b)
    {
        ball = b;
        setDefaultOffset();
    }

    private void setDefaultOffset()
    {
        offset = new Vector3(-8, 2, -4);
    }
}
