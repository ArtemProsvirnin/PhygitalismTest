using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {
    public Ball[] balls;
    public SpeedSlider slider;
    public CameraManager cameraManager;

    private int activeBallIndex = 0;
    private int ballCount;   
    private LayerMask mask;
    private float doubleClickTime = 0.0f;

    private Ball ActiveBall
    {
        get { return balls[activeBallIndex]; }
    }

    void Start()
    {
        ballCount = balls.Length;
        mask = LayerMask.GetMask("Clickable");
                
        slider.OnChange += updateSpeed;
    }

    void Update()
    {
        checkForChangeBall();
        checkForClick();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void checkForChangeBall()
    {
        if (Input.GetKeyDown("left"))
        {
            nextBall();
        }

        if (Input.GetKeyDown("right"))
        {
            previousBall();
        }
    }

    private void nextBall()
    {
        ActiveBall.Freeze();

        if (activeBallIndex >= ballCount - 1)
        {
            activeBallIndex = 0;
        }
        else
        {
            activeBallIndex++;
        }

        updateBall();
    }

    private void previousBall()
    {
        ActiveBall.Freeze();

        if (activeBallIndex <= 0)
        {
            activeBallIndex = ballCount - 1;
        }
        else
        {
            activeBallIndex--;
        }

        updateBall();
    }

    private void updateBall()
    {
        ActiveBall.WakeUp();
        slider.Value = ActiveBall.speed;
        cameraManager.SetBall(ActiveBall);
    }

    private void checkForClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            Ray ray = cameraManager.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                Ball ball = hit.collider.gameObject.GetComponent<Ball>();

                if (ball != null && ball == ActiveBall)
                {
                    if (Time.time < doubleClickTime + 0.5)
                    {
                        ball.Reset();
                        doubleClickTime = 0.0f;
                    }
                    else
                    {
                        ball.StartMoving();
                        doubleClickTime = Time.time;
                    }
                }
            }
        }
    }

    private void updateSpeed(object sender, EventArgs e)
    {
        ActiveBall.speed = slider.Value;
    }
}
