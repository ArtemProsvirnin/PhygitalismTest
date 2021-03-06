﻿using System;
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
        checkForExit();
    }

    private void checkForChangeBall()
    {
        checkForChangeBall("left", nextBall);
        checkForChangeBall("right", previousBall);
    }
    
    private void checkForChangeBall(string key, Action action)
    {
        if (Input.GetKeyDown(key))
        {
            ActiveBall.Pause();
            action();
            updateBall();
        }
    }

    private void nextBall()
    {   
        if (activeBallIndex >= ballCount - 1)
        {
            activeBallIndex = 0;
        }
        else
        {
            activeBallIndex++;
        }        
    }

    private void previousBall()
    {
        if (activeBallIndex <= 0)
        {
            activeBallIndex = ballCount - 1;
        }
        else
        {
            activeBallIndex--;
        }
    }

    private void updateBall()
    {
        ActiveBall.Resume();
        slider.Value = ActiveBall.speed;
        cameraManager.SetBall(ActiveBall);
    }

    private void checkForClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            Ray ray = cameraManager.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask)) // Raycast по маске 'Clickable', Prefab для шара привязан к Layer 'Clickable'
            {
                Ball ball = hit.collider.gameObject.GetComponent<Ball>();

                if (ball != null && ball == ActiveBall) //Если щелчок прошел не по активному шару, то игнорируем его
                {
                    if (Time.time < doubleClickTime + 0.5) //Ждем пол секунды с момента последнего клика
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

    private void checkForExit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
