using System;
using System.Collections;
using System.Collections.Generic;
using iTextSharp.text;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class ZombieSpawner : MonoBehaviour
{
    public int initialZombieperWave = 5;
    public int currentZombiePerWave;

    public float spawnDelay = 0.5f;
    public int waveCoolDown = 10;

    public int currentWave = 0;

    public bool inCoolDown;
    public float coolDownCounter = 0;

    public GameObject zombiePrefab;

    public List<Enemy> currenteZombiesAlive;

    public TextMeshProUGUI waverOverUI;
    public TextMeshProUGUI coolDownCounterUI;
    public TextMeshProUGUI currentWaveUI;


    private void Start()
    {
        currentZombiePerWave = initialZombieperWave;

        StartNextWave();
    }

    private void StartNextWave()
    {
        currenteZombiesAlive.Clear();

        currentWave++;
        currentWaveUI.text = "Wave: " + currentWave.ToString();
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentZombiePerWave; i++)
        {
            Vector3 spawnOffSet = new Vector3(Random.Range(-1, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffSet;

            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity); 

            Enemy enemyScript = zombie.GetComponent<Enemy>();

            currenteZombiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Update()
    {
        List<Enemy> zombiesToRemove = new List<Enemy>();
        foreach (Enemy zombie in currenteZombiesAlive)
        {
            if (zombie.isDead)
            {
                zombiesToRemove.Add(zombie);
            }
        }

        foreach (Enemy zombie in zombiesToRemove)
        {
            currenteZombiesAlive.Remove(zombie);
        }

        zombiesToRemove.Clear();


        if (currenteZombiesAlive.Count == 0 && inCoolDown == false)
        {
            StartCoroutine(WaveCoolDown());
        }
        if (inCoolDown)
        {
            coolDownCounter -= Time.deltaTime;
        }
        else
        {
            coolDownCounter = waveCoolDown;
        }

        coolDownCounterUI.text = coolDownCounter.ToString();
    }

    private IEnumerator WaveCoolDown()
    {
        inCoolDown = true;
        waverOverUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(waveCoolDown);

        inCoolDown = false;

        waverOverUI.gameObject.SetActive(false);
        currentZombiePerWave *= 2;
        StartNextWave();
    }
}
