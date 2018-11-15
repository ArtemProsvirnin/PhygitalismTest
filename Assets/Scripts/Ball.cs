using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 0.5f;
    public TextAsset jsonFile;

    private LineRenderer line;
    private PathController pathController;

    private BallState state;
    private BallState stateBeforePause;

    void Start()
    {
        pathController = new PathController(jsonFile.text, transform);
        pathController.OnFinish += onFinish;

        line = gameObject.GetComponent<LineRenderer>();

        Reset();
    }
	
	void Update()
    {
        if (speed == 0)
        {
            return;
        }

        state.Move(speed);
        state.DrawLine(transform);
    }

    public void Reset()
    {
        state = new StartState(pathController, line);
    }

    public void StartMoving()
    {
        state = new MoveState(pathController, line);
    }

    public void Pause()
    {
        stateBeforePause = state;
        goIdle();
    }

    public void Resume()
    {
        if (stateBeforePause != null)
        {
            state = stateBeforePause;
            stateBeforePause = null;
        }
    }

    private void onFinish(object sender, EventArgs e)
    {
        goIdle();
    }

    private void goIdle()
    {
        state = new IdleState(pathController, line);
    }

    //States of the ball

    private abstract class BallState
    {
        protected LineRenderer line;
        protected PathController pathController;

        public BallState(PathController pc, LineRenderer l)
        {
            pathController = pc;
            line = l;
        }

        public abstract void Move(float speed);
        public abstract void DrawLine(Transform transform);
    }

    private class IdleState : BallState
    {
        public IdleState(PathController pc, LineRenderer l) : base(pc, l)
        {
        }

        public override void Move(float speed)
        {
        }

        public override void DrawLine(Transform transform)
        {
        }
    }

    private class StartState: IdleState
    {
        public StartState(PathController pc, LineRenderer l) : base(pc, l)
        {
            pathController.Reset();
            line.positionCount = 0;
        }
    }

    private class MoveState: BallState
    {
        public MoveState(PathController pc, LineRenderer l) : base(pc, l)
        {
            pathController.Play();
        }

        public override void Move(float speed)
        {
            pathController.Move(speed);
        }

        public override void DrawLine(Transform transform)
        {
            line.positionCount++;
            line.SetPosition(line.positionCount - 1, transform.position);
        }
    }
}
