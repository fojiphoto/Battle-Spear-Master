using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    public float speed = 2;

    int index = 0;

    private Vector3 startPos;

    public bool isLoop = true;

    public GameObject vfxOnDeath;

    private bool canMove = true;

    public Transform[] waypoints;

    public TextMeshProUGUI text;

    public int power;

    private void Awake()
    {
        startPos = transform.position;

        text.SetText("Power : {0}", power);
    }

    private void OnMouseDown()
    {
        canMove = false;
        //Destroy();
    }

    private void Update()
    {
        if (!canMove) return;

        Vector3 destination = waypoints[index].position;
        Vector3 newPos = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        transform.position = newPos;

        float distance = Vector3.Distance(transform.position, destination);
        if (distance <= 0.05)
        {
            if (index < waypoints.Length - 1)
            {
                index++;
            }
            else
            {
                if (isLoop)
                {
                    index = 0;
                    transform.position = startPos;
                }
            }
        }
    }

    public void Destroy()
    {
        GameManager.Instance.enemies.Remove(gameObject);
        GameObject vfx = Instantiate(vfxOnDeath, transform.position, Quaternion.identity);
        Destroy(vfx, 1f);
        GameManager.Instance.CheckEnemy();
        Destroy(gameObject);
    }
}