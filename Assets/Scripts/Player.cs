using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public TextMeshProUGUI text;

    private int power;

    public DrawWithMouse drawControl;

    public float speed = 5f;

    private Vector3 startPos;

    bool startMovement = false;

    Vector3[] positions;

    int moveIndex = 0;

    private float min_X = -2.15f;

    private float max_X = 2.15f;

    private float min_Y = -4.85f;

    private float max_Y = 4.85f;

    public GameObject vfxCollected;

    public GameObject vfxOnDeath;

    public bool moveAble = true;

    private int ID;

    private void Awake()
    {
        Instance = this;

        startPos = transform.position;

        ID = GetInstanceID();

        power = 0;

        text.SetText("Power : {0}", power);
    }

    private void OnMouseDown()
    {
        if (!moveAble) return;
        drawControl.StartLine(transform.position);
    }

    private void OnMouseDrag()
    {
        if (!moveAble) return;
        drawControl.Updateline();
    }

    private void OnMouseUp()
    {
        if (!moveAble) return;
        positions = new Vector3[drawControl.line.positionCount];
        drawControl.line.GetPositions(positions);
        drawControl.ResetLine();
        startMovement = true;
        moveIndex = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Coin"))
        {
            GameObject vfx = Instantiate(vfxCollected, transform.position, Quaternion.identity);
            Destroy(vfx, 1f);

            power += 1;
            text.SetText("Power : {0}", power);

            Destroy(collision.gameObject);

            GameManager.Instance.SpawnCoin();
        }

        if (collision != null && collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<Enemy>().power > this.power)
            {
                GameManager.Instance.playerLife -= 1;
                GameManager.Instance.CheckLose();
                ReSetPos();
            }
            else
            {
                power += collision.gameObject.GetComponent<Enemy>().power;
                text.SetText("Power : {0}", power);

                collision.gameObject.GetComponent<Enemy>().Destroy();
            }
        }

        if (collision != null && collision.gameObject.CompareTag("Boss"))
        {
            if (collision.gameObject.GetComponent<Boss>().power > this.power)
            {
                GameManager.Instance.playerLife -= 1;
                GameManager.Instance.CheckLose();
                ReSetPos();
                collision.gameObject.GetComponent<Boss>().ResetPos();
            }
            else
            {
                power += collision.gameObject.GetComponent<Boss>().power;
                text.SetText("Power : {0}", power);

                collision.gameObject.GetComponent<Boss>().Destroy();
            }
        }
    }

    public void Success()
    {
        //GameObject vfx = Instantiate(vfxKill, transform.position, Quaternion.identity);
        //Destroy(vfx, 1f);
        startMovement = false;
    }

    public void ReSetPos()
    {
        GameObject vfx = Instantiate(vfxOnDeath, transform.position, Quaternion.identity);
        Destroy(vfx, 1f);
        transform.position = startPos;
        startMovement = false;
        drawControl.ResetLine();
    }

    private void Update()
    {
        if (startMovement)
        {
            CheckPos();

            Vector2 currentPos = positions[moveIndex];
            transform.position = Vector2.MoveTowards(transform.position, currentPos, speed * Time.deltaTime);

            float distance = Vector2.Distance(currentPos, transform.position);
            if (distance <= 0.05f)
            {
                moveIndex++;
            }

            if (moveIndex > positions.Length - 1)
            {
                startMovement = false;
            }
        }
    }

    private void CheckPos()
    {
        if (transform.position.x < min_X)
        {
            Vector3 moveDirX = new Vector3(min_X, transform.position.y, 0f);
            transform.position = moveDirX;
        }
        else if (transform.position.x > max_X)
        {
            Vector3 moveDirX = new Vector3(max_X, transform.position.y, 0f);
            transform.position = moveDirX;
        }
        else if (transform.position.y < min_Y)
        {
            Vector3 moveDirX = new Vector3(transform.position.x, min_Y, 0f);
            transform.position = moveDirX;
        }
        else if (transform.position.y > max_Y)
        {
            Vector3 moveDirX = new Vector3(transform.position.x, max_Y, 0f);
            transform.position = moveDirX;
        }
        else if (transform.position.x < min_X && transform.position.y < min_Y)
        {
            Vector3 moveDirX = new Vector3(min_X, min_Y, 0f);
            transform.position = moveDirX;
        }
        else if (transform.position.x < min_X && transform.position.y > max_Y)
        {
            Vector3 moveDirX = new Vector3(min_X, max_Y, 0f);
            transform.position = moveDirX;
        }
        else if (transform.position.x > max_X && transform.position.y > max_Y)
        {
            Vector3 moveDirX = new Vector3(max_X, max_Y, 0f);
            transform.position = moveDirX;
        }
        else if (transform.position.x > max_X && transform.position.y < min_Y)
        {
            Vector3 moveDirX = new Vector3(max_X, min_Y, 0f);
            transform.position = moveDirX;
        }
    }
}