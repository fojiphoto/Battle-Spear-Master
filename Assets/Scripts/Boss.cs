using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Boss : MonoBehaviour
{
    public GameObject vfxOnDeath;

    public float speed;

    public TextMeshProUGUI text;

    public int power;

    Vector3 startPos;

    private void Awake()
    {
        text.SetText("Power : {0}", power);

        startPos = transform.position;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Player.Instance.transform.position, speed * Time.deltaTime);

    }

    public void ResetPos()
    {
        transform.position = startPos;
    }

    public void Destroy()
    {
        GameObject vfx = Instantiate(vfxOnDeath, transform.position, Quaternion.identity);
        Destroy(vfx, 1f);
        GameManager.Instance.WonTheGame();
        Destroy(gameObject);
    }
}
