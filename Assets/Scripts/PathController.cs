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

    //Позиция после остановки
    private Vector3 lastPosition;

    //Данный параметр нужен для возможности продолжения движения из lastPosition
    private Vector3 difference;

    public PathController(string jsonText, Transform transform)
    {
        this.path = new Path(jsonText); ;
        this.transform = transform;
        this.transform.position = getFirstVector();
    }

    public void Play()
    {
        if (step != 0)
            return;

        lastPosition = transform.position;
        transform.position = getFirstVector();
    }

    public void Reset()
    {
        //Reset 'обнуляет' difference и lastPosition, возвращает шар в начальную позицию
        step = 0;
        lastPosition = difference = Vector3.zero;
        transform.position = getFirstVector();
    }

    public void Move(float speed)
    {
        Vector3 direction = getTargetVector();
        transform.position = Vector3.MoveTowards(transform.position, direction, (speed * 10) * Time.deltaTime);

        if (Vector3.Distance(transform.position, direction) < 0.1)
        {
            step++;
            checkForFinish();
        }
    }

    private void checkForFinish()
    {
        if (step == path.Length && OnFinish != null)
        {
            step = 0;
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
