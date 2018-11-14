using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 0.5f;
    public TextAsset jsonFile;

    private bool movable;
    private bool started;
    private LineRenderer line;
    private PathController pathController;
    
    void Start()
    {
        Path path = new Path(jsonFile.text);
        pathController = new PathController(path, transform);
        pathController.OnFinish += onFinish;

        line = gameObject.GetComponent<LineRenderer>();
    }
	
	void Update()
    {
        if (!started)
        {
            return;
        }

        move();
        drawLine();
    }

    public void StartMoving()
    {
        if (started)
        {
            return;
        }
        
        pathController.Play();
        line.positionCount = 0;

        WakeUp();

        started = true;
    }

    public void Reset()
    {
        pathController.Reset();
        line.positionCount = 0;
        started = false;
    }

    public void WakeUp()
    {
        movable = true;
    }

    public void Freeze()
    {
        movable = false;
    }
    
    private void move()
    {
        if (!movable)
        {
            return;
        }

        pathController.Move(speed);
    }

    private void drawLine()
    {
        if (speed == 0)
        {
            return;
        }

        line.positionCount++;
        line.SetPosition(line.positionCount - 1, transform.position);
    }

    private void onFinish(object sender, EventArgs e)
    {
        started = false;
    }
}
