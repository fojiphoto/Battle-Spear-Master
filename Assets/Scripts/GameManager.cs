using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform[] spawnPoints;

    public GameObject coinPrefab;

    public List<GameObject> enemies;

    public GameObject boss;

    public GameObject vfxSuccess;

    public GameObject vfxLose;

    public TextMeshProUGUI text;

    public int playerLife = 5;

    private void Awake()
    {
        Instance = this;

        text.SetText("Player Life Amount : {0}", playerLife);
    }

    private void Start()
    {
        SpawnCoin();
    }

    private void Update()
    {
        text.SetText("Player Life Amount : {0}", playerLife);
    }
    public void CheckLose()
    {
        if (playerLife == 0)
        {
            LoseTheGame();
        }
    }

    public void SpawnCoin()
    {
        Transform randomTrans = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];

        GameObject coin = Instantiate(coinPrefab, randomTrans.position, Quaternion.identity);
    }

    public void CheckEnemy()
    {
        if (enemies.Count == 0)
        {
            GameObject bossPrefab = Instantiate(boss, transform.position, Quaternion.identity);
        }
    }

    public void WonTheGame()
    {
        GameObject vfx = Instantiate(vfxSuccess, transform.position, Quaternion.identity) as GameObject;
        Destroy(vfx, 2f);

        StartCoroutine(Win());
    }

    public void LoseTheGame()
    {
        GameObject vfx = Instantiate(vfxLose, transform.position, Quaternion.identity) as GameObject;
        Destroy(vfx, 2f);

        StartCoroutine(Lose());
    }

    IEnumerator Win()
    {
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator Lose()
    {

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}