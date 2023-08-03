using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class EnemySpawner : MonoBehaviour
{
   
    [System.Serializable]
    public class EnemiesWillSpawn
    {
        public float instantCooldown;
        public int[] enemyIndex;
        public int[] enemyCount;
    }
    public List<EnemiesWillSpawn> level = new();
    [System.Serializable]
    public class EnemiesElements
    {
        public GameObject enemyObject;
        public float hitPoint;
        public int damage;
        public float speed;
        public float turnSpeed;
        public int goldAmount;
    }
    [Header("-----------Important----------")]
    public List<EnemiesElements> enemiesElements = new();
    [Header("-----------Private----------")]
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform enemyTarget;
    [SerializeField] private GameObject explosionParticle;
    [SerializeField] private Transform enemiesContainer;
    [SerializeField] private Transform container;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject damageInfoCanvasObject;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CarCombat carCombat;
    private int waveInEnemyCount;
    private int currentWave;
    [Header("ExplodedEnv")]
    [SerializeField] private float expEnvDistanceZPositive;
    [SerializeField] private float expEnvDistanceZNegative;
    [SerializeField] private float expEnvDistanceX;
    [Header("Tree")]
    [SerializeField] private GameObject explodedTree;
    [SerializeField] private float treeExpForce;
    [Header("Block")]
    [SerializeField] private GameObject explodedBlock;
    [SerializeField] private float blockExpForce;
    

    private void Start()
    { 
    }
    public void enemySpawnerForStart()
    {
        DOTween.Init();
        currentWave = 0;
        currentWaveInEnemyCount();
        StartCoroutine(instantEnemy());
    }
    private object _lock = new object();
    public void enemyCountCheck(EnemyCar enemyCar)
    {
        lock (_lock)
        {
            enemyCar.carDamageCheck = false;
            waveInEnemyCount--;
            if (waveInEnemyCount <= 0)
            {
                currentWave++;
                gameManager.setCurrentVave();
                if (currentWave >= level.Count)
                {
                    gameManager.winMethod();
                }
                else
                {
                    currentWaveInEnemyCount();
                    StartCoroutine(instantEnemy());
                    
                }
            }
        }
    }
    void currentWaveInEnemyCount()
    {
        waveInEnemyCount = 0;
        for (int i = 0; i < level[currentWave].enemyCount.Length; i++)
        {
            waveInEnemyCount += level[currentWave].enemyCount[i];
        }
    }
    IEnumerator instantEnemy()
    {
        int spawnQue = 0;
        for(int i = 0; i < level[currentWave].enemyCount.Length; i++)
        {
            for(int x = 0; x < level[currentWave].enemyCount[i]; x++)
            {
                GameObject g = Instantiate(enemiesElements[level[currentWave].enemyIndex[i]].enemyObject
                    , new Vector3(spawnPoints[spawnQue].position.x, enemiesElements[level[currentWave].enemyIndex[i]].enemyObject.transform.position.y, spawnPoints[spawnQue].position.z), Quaternion.identity);

                EnemiesElements e = enemiesElements[level[currentWave].enemyIndex[i]];

                g.GetComponent<EnemyCar>().setValues(e.hitPoint, e.damage, e.speed, e.turnSpeed, e.goldAmount, enemyTarget, container, enemySpawner
                , damageInfoCanvasObject, mainCamera
                , expEnvDistanceZPositive, expEnvDistanceZNegative, expEnvDistanceX, explodedTree, treeExpForce, explodedBlock, blockExpForce, explosionParticle);
                g.GetComponent<EnemyCar>().changeRandomMaterial();

                spawnQue++;
                if(spawnQue >= spawnPoints.Length)
                {
                    spawnQue = 0;
                }
                yield return new WaitForSeconds(level[currentWave].instantCooldown);
            }
        }
    }
}
