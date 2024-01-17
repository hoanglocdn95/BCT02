using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] public GameObject[] points;

    [SerializeField] private float speed = 2f;
    [SerializeField] private int currentPointIndex = 0;

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

        this.Moving();
    }

    protected virtual void Moving()
    {
        transform.position = Vector2.MoveTowards(transform.position, points[this.currentPointIndex].transform.position, Time.deltaTime * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.parent = this.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }
}
