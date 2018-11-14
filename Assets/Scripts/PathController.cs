using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController
{
    public event EventHandler OnFinish;

    private Path path;
    private Transform transform;
    private int step;
    private Vector3 lastPosition;
    private Vector3 difference;

    public PathController(Path path, Transform transform)
    {
        this.path = path;
        this.transform = transform;
        this.transform.position = getFirstVector();
    }

    public void Play()
    {
        step = 0;
        transform.position = getFirstVector();
    }

    public void Reset()
    {
        step = 0;
        lastPosition = difference = Vector3.zero;
        transform.position = getFirstVector();
    }

    public void Move(float speed)
    {
        if (speed == 0)
        {
            return;
        }

        Vector3 direction = getTargetVector();

        if (Vector3.Distance(transform.position, direction) == 0)
        {
            step++;
        }

        transform.position = Vector3.MoveTowards(transform.position, direction, (speed * 5) * Time.deltaTime);

        if (step == path.Length && OnFinish != null)
        {
            lastPosition = direction;
            OnFinish(this, new EventArgs());
        }
    }

    private Vector3 getTargetVector()
    {
        return difference + new Vector3(path.X[step], path.Y[step], path.Z[step]);
    }

    private Vector3 getFirstVector()
    {
        Vector3 first = new Vector3(path.X[0], path.Y[0], path.Z[0]);

        if (lastPosition == Vector3.zero)
        {
            return first;
        }

        difference = lastPosition - first;

        return lastPosition;
    }
}
