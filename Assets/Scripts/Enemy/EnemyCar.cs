using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Lofelt.NiceVibrations;
using System;
using static ToonyColorsPro.ShaderGenerator.Enums;

public class EnemyCar : MonoBehaviour
{
    private float hitPoint;
    private int damage;
    private float speed;
    private float turnSpeed;
    private Transform target;
    private Transform container;
    private EnemySpawner enemySpawner;
    private int goldAmount;
    private GameObject damageInfoCanvasObject;
    private Camera mainCamera;
    private GameObject explosionParticle;
    [HideInInspector] public bool carDamageCheck;
    [HideInInspector] public bool isMove;

    [Header("ExplodedEnv")]
    private float expEnvDistanceZPositive;
    private float expEnvDistanceZNegative;
    private float expEnvDistanceX;
    [Header("Tree")]
    private GameObject explodedTree;
    private float treeExpForce;
    [Header("Block")]
    private GameObject explodedBlock;
    private float blockExpForce;
    [Header("References")]
    [SerializeField] private Transform lookAtObject;
    [SerializeField] private Rigidbody sphereRigi;
    [SerializeField] private float explosionParticleDestroyTime;
    [SerializeField] private GameObject goldObject;
    [Header("ChangeMaterialItems")]
    [SerializeField] private MeshRenderer[] changeMaterialItems;
    [SerializeField] private Material[] materials;
    private void Start()
    {
        DOTween.Init();
        carDamageCheck = true;
        isMove = true;
    }
    public void setValues(float hitPoint,int damage, float speed,float turnSpeed,int goldAmount, Transform target, Transform container
        , EnemySpawner enemySpawner, GameObject damageInfoCanvasObject, Camera mainCamera
        , float expEnvDistanceZPositive, float expEnvDistanceZNegative, float expEnvDistanceX, GameObject explodedTree, float treeExpForce , GameObject explodedBlock , float blockExpForce
        , GameObject explosionParticle)
    {
        this.hitPoint = hitPoint;
        this.damage = damage;
        this.speed = speed;
        this.turnSpeed = turnSpeed;
        this.goldAmount = goldAmount;
        this.container = container;
        this.target = target;
        this.enemySpawner = enemySpawner;
        this.damageInfoCanvasObject = damageInfoCanvasObject;
        this.mainCamera = mainCamera;

        this.expEnvDistanceZPositive = expEnvDistanceZPositive;
        this.expEnvDistanceZNegative = expEnvDistanceZNegative;
        this.expEnvDistanceX = expEnvDistanceX;
        this.explodedTree = explodedTree;
        this.treeExpForce = treeExpForce;
        this.explodedBlock = explodedBlock;
        this.blockExpForce = blockExpForce;

        this.explosionParticle = explosionParticle;
    }
    private void Update()
    {
        if (target != null && isMove)
        {
            Vector3 lookPos = target.position;
            lookPos.y = 0;
            lookAtObject.LookAt(lookPos);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookAtObject.rotation, Time.deltaTime * turnSpeed);
        }
    }
    private void FixedUpdate()
    {
        if (target != null)
        {
            sphereRigi.velocity = transform.forward * speed;
        }    
    }
    private object _lock = new object();
    public void getDamage(float damage)
    {
        
        lock (_lock)
        {
            hitPoint -= damage;

            if (hitPoint <= 0)
            {
                enemyCountCheck();
                GameObject g = Instantiate(explosionParticle, transform.position, Quaternion.identity);
                g.transform.SetParent(container);
                Destroy(g, explosionParticleDestroyTime);
                instantGold();
                HapticPatterns.PlayEmphasis(0.5f, 0.05f);
                Destroy(gameObject);
            }
            else
            {
                damageInfo(damage);
            }
        } 
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            other.GetComponent<CarCombat>().getDamage(damage);
            getDamage(hitPoint);
        }
        if (other.CompareTag("ExpTree"))
        {
            instantExploded(explodedTree, other, treeExpForce);
        }
        if (other.CompareTag("ExpBlock"))
        {
            instantExploded(explodedBlock, other, blockExpForce);
        }
    }

    void instantGold()
    {
        for (int i = 0; i < goldAmount; i++)
        {
            Vector3 v = transform.position;
            GameObject g = Instantiate(goldObject, new Vector3(UnityEngine.Random.Range(v.x + 3, v.x - 3), v.y + 1, UnityEngine.Random.Range(v.z + 3, v.z - 3)), Quaternion.Euler(90, 0, 0));
            g.transform.SetParent(container);
        }
    }

    void damageInfo(float damage)
    {
        GameObject g = Instantiate(damageInfoCanvasObject, new Vector3(transform.position.x, 2, transform.position.z)
                       , Quaternion.Euler(mainCamera.transform.eulerAngles.x, 0, 0));
        g.transform.SetParent(container);
        g.GetComponent<Canvas>().worldCamera = mainCamera;
        g.GetComponentInChildren<TextMeshProUGUI>().text = damage.ToString();
        g.transform.DOScale(g.transform.localScale, .5f).OnComplete(() =>
        {
            g.transform.DOScale(Vector3.zero, .5f).OnComplete(() =>
            {
                Destroy(g);

            });
        });
    }
    void enemyCountCheck()
    {
        if (carDamageCheck)
        {
            enemySpawner.enemyCountCheck(GetComponent<EnemyCar>());
        }
    }

    #region ENV EXP
    void instantExploded(GameObject exploded, Collider other, float force)
    {
        if (envExpCheck() && target != null)
        {
            GameObject g = Instantiate(exploded, other.transform.position, other.transform.rotation);
            Vector3 v = new Vector3(other.transform.position.x - transform.position.x, 1
                   , other.transform.position.z - transform.position.z);
            Destroy(other.gameObject);
            v = v.normalized;
            v.y = .05f;
            foreach (Rigidbody r in g.GetComponentsInChildren<Rigidbody>())
            {
                r.AddForce(v * force);
            }
            StartCoroutine(envDestroyAnimation(g.GetComponentsInChildren<Transform>()));
            Destroy(other.gameObject);
        }
    }

    IEnumerator envDestroyAnimation(Transform[] envs)
    {
        yield return new WaitForSeconds(5);
        foreach (Transform t in envs)
        {
            t.DOLocalMoveY(-5, 4);
        }
        yield return new WaitForSeconds(4);
        foreach (Transform t in envs)
        {
            Destroy(t.gameObject);
        }
    }

    bool envExpCheck()
    {
        float expPositive = Math.Abs(transform.position.z) - Math.Abs(target.position.z);
        float expNegative = Math.Abs(target.position.z) - Math.Abs(transform.position.z);
        if (Math.Abs(transform.position.x) - Math.Abs(target.position.x) <= expEnvDistanceX)
        {
            return true;

        }else if (expPositive > 0 && expPositive < expEnvDistanceZPositive)
        {
            return true;
        }
        else if(expNegative > 0 && expNegative < expEnvDistanceZNegative)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
#endregion
    public void changeRandomMaterial()
    {
        int rnd = UnityEngine.Random.Range(0, materials.Length);
        foreach(MeshRenderer m in changeMaterialItems)
        {
            m.material = materials[rnd];
        }
    }

    public void gameOverForEnemey()
    {
        sphereRigi.isKinematic = false;
        isMove = false;
    }
}
