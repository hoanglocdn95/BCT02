using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawMovement : MonoBehaviour
{
    [SerializeField] public GameObject[] points;
    protected SpriteRenderer spriteRenderer;

    [SerializeField] private float speed = 5f;
    [SerializeField] private int currentPointIndex = 0;

    private void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, points[currentPointIndex].transform.position) < 0.1f)
        {
            this.currentPointIndex++;

            if (this.currentPointIndex >= points.Length)
            {
                this.currentPointIndex = 0;
            }
        }

        if(this.points[this.currentPointIndex].transform.position.x > transform.position.x)
        {
            this.spriteRenderer.flipX = true;
        }

        if (this.points[this.currentPointIndex].transform.position.y > transform.position.y)
        {
            this.spriteRenderer.flipY = true;
        }

        this.Moving();
    }

    protected virtual void Moving()
    {
        transform.position = Vector2.MoveTowards(transform.position, points[this.currentPointIndex].transform.position, Time.deltaTime * speed);
    }
}
