using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCombat : MonoBehaviour
{
    
    [Header("Rocket")]
    [SerializeField] private GameObject rocketObject;
    [SerializeField] private float rocketDamage;
    [SerializeField] private float rocketMoveSpeed;
    [SerializeField] private float rocketRadius;
    [SerializeField] private float rocketCooldown;
    [SerializeField] private float rocketDistance;
    [SerializeField] private bool isRocket;
    private int isRocketAmount;
    [Header("RocketsParent")]
    [SerializeField] GameObject[] rocketParents;
    [SerializeField] GameObject rocketAmountParticle;
    [SerializeField] GameObject rocketUpgradeParticle;
    [SerializeField] float RocketParticleDestroyTime;
    [SerializeField] Transform RocketAmountAndUpgradeParticlePos;
    [Header("ONERocketReference")]
    [SerializeField] private Transform oneRocketBarrel;
    [SerializeField] private Transform oneRocketSpawnPoint;
    [SerializeField] private Transform oneRocketGunFireSpawnPos;
    [Header("TWORocketReference")]
    [SerializeField] private Transform twoRocketBarrel;
    [SerializeField] private Transform[] twoRocketSpawnPoint;
    [SerializeField] private Transform[] twoRocketGunFireSpawnPos;
    [Header("THREERocketReference")]
    [SerializeField] private Transform thRocketBarrel;
    [SerializeField] private Transform[] thRocketSpawnPoint;
    [SerializeField] private Transform[] thRocketGunFireSpawnPos;
    [Header("Fireball")]
    [SerializeField] private float fireballDamage;
    [SerializeField] private Transform fireballParent;
    [SerializeField] private float fireballTurnSpeed;
    [SerializeField] private Transform[] fireballs;
    private int fireballCount;
    [Header("Shield")]
    [SerializeField] private GameObject shieldObject;
    [Header("-----------Private----------")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform particleContainer;
    [SerializeField] private CarController carController;
    [SerializeField] private SphereCollider coinMagnetCol;
    [SerializeField] private GameObject gunFireParticle;
    [Header("HP")]
    [SerializeField] private GameObject[] hearths;
    //[SerializeField] private GameObject[] hearthsBackground;
    [Range(1,10)][SerializeField] private int hitPoint;
    [SerializeField] private Transform healParticleParent;
    [SerializeField] private Transform healParticlePos;
    [SerializeField] private GameObject healParticle;
    [SerializeField] private float healParticleDestroyTime;


    private void Start()
    {
        fireballCount = 0;
        isRocketAmount = 1;
        StartCoroutine(rocketMethod());
        for(int i = 0; i < hitPoint; i++)
        {
            hearths[i].SetActive(true);
        }
    }
    private void Update()
    {
        fireballParent.position = new Vector3(transform.position.x, fireballParent.position.y, transform.position.z);
        fireballParent.Rotate(Vector3.down * fireballTurnSpeed * Time.deltaTime);
        if (nearEnemeyMethod() != null)
        {
            Transform t = nearEnemeyMethod().transform;
            if (isRocketAmount == 1)
            {
                Vector3 lookPos = t.position;
                lookPos.y = oneRocketBarrel.position.y;
                oneRocketBarrel.LookAt(lookPos);
            }
            else if (isRocketAmount == 2)
            {
                Vector3 lookPos = t.position;
                lookPos.y = twoRocketBarrel.position.y;
                twoRocketBarrel.LookAt(lookPos);
            }
            else if (isRocketAmount == 3)
            {
                Vector3 lookPos = t.position;
                lookPos.y = thRocketBarrel.position.y;
                thRocketBarrel.LookAt(lookPos);
            }
        }
    }
    #region Rocket
    IEnumerator rocketMethod()
    {
        yield return new WaitForSeconds(rocketCooldown);
        if (nearEnemeyMethod() != null)
        {
            rocketCheck();
        }
        if (isRocket)
        {
            StartCoroutine(rocketMethod());
        }
        
    }
    Collider nearEnemeyMethod()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, rocketDistance, enemyLayer);
        Collider nearEnemy = null;
        if (cols.Length > 0)
        {
            if (cols.Length == 1)
            {
                nearEnemy = cols[0];
            }
            else
            {
                nearEnemy = cols[0];
                foreach (Collider c in cols)
                {
                    if (Vector3.Distance(transform.position, c.transform.position)
                         < Vector3.Distance(transform.position, nearEnemy.transform.position))
                    {
                        nearEnemy = c;
                    }
                }
            }
            
        }
        return nearEnemy;
    }
    void rocketCheck()
    {
        if (isRocketAmount == 1)
        {
            GameObject p = Instantiate(gunFireParticle, oneRocketGunFireSpawnPos.position, Quaternion.identity);
            p.transform.SetParent(oneRocketBarrel);
            Destroy(p, 1);
            GameObject g = Instantiate(rocketObject, oneRocketSpawnPoint.position, Quaternion.identity);
            g.transform.rotation = oneRocketSpawnPoint.rotation;
            g.GetComponent<RocketSc>().setValues(rocketDamage, rocketMoveSpeed, rocketRadius, enemyLayer, particleContainer);
        }
        else if (isRocketAmount == 2)
        {
            for (int i = 0; i < isRocketAmount; i++)
            {
                GameObject p = Instantiate(gunFireParticle, twoRocketGunFireSpawnPos[i].position, Quaternion.identity);
                p.transform.SetParent(twoRocketBarrel);
                Destroy(p, 1);
                GameObject g = Instantiate(rocketObject, twoRocketSpawnPoint[i].position, Quaternion.identity); 
                g.transform.rotation = twoRocketSpawnPoint[i].rotation;
                g.GetComponent<RocketSc>().setValues(rocketDamage, rocketMoveSpeed, rocketRadius, enemyLayer, particleContainer);
            }
        }
        else
        {
            for (int i = 0; i < isRocketAmount; i++)
            {
                GameObject p = Instantiate(gunFireParticle, thRocketGunFireSpawnPos[i].position, Quaternion.identity);
                p.transform.SetParent(thRocketBarrel);
                Destroy(p, 1);
                GameObject g = Instantiate(rocketObject, thRocketSpawnPoint[i].position, Quaternion.identity);
                g.transform.rotation = thRocketSpawnPoint[i].rotation;
                g.GetComponent<RocketSc>().setValues(rocketDamage, rocketMoveSpeed, rocketRadius, enemyLayer, particleContainer);
            }
        }
    }
    #endregion
    private object _lock = new object();
    public void getDamage(int damage)
    {
        lock (_lock) 
        {
            carController.cameraShakeMethod();
            if (shieldObject.activeSelf)
            {
                shieldObject.SetActive(false);
                return;
            }
            hitPoint -= damage;
            if (hitPoint <= 0)
            {
                gameManager.gameOverMethod();
                foreach (GameObject g in hearths)
                {
                    g.SetActive(false);
                }
            }
            else
            {
                for (int i = hearths.Length - 1; i >= hitPoint; i--)
                {
                    hearths[i].SetActive(false);
                }
            }
        }
    }
    public int getHitPoint()
    {
        return hitPoint;
    }
    

    private object _lockC = new object();
    public void setGameManagerCoin()
    {
        lock (_lockC)
        {
            gameManager.setCoin();
        }
    }

    

    /// ///////////////////////////////////////////////////////////////////////////////////////////////////
    public void setFireballCount()
    {
        fireballCount++;
        for (int i = 0; i < fireballCount; i++)
        {
            fireballs[i].gameObject.SetActive(true);
            fireballs[i].GetComponent<FireballSc>().setValues(fireballDamage, particleContainer);
        }
    }
    void rocketUpgradeParticleMethod()
    {
        GameObject g = Instantiate(rocketUpgradeParticle, RocketAmountAndUpgradeParticlePos.position, Quaternion.identity);
        g.transform.SetParent(RocketAmountAndUpgradeParticlePos);
        Destroy(g, RocketParticleDestroyTime);
    }
    public void setRocketDamage(float rocketDamage)
    {
        rocketUpgradeParticleMethod();
        this.rocketDamage += rocketDamage;
    }
    public void setRocketRadius(float rocketRadius)
    {
        rocketUpgradeParticleMethod();
        this.rocketRadius += rocketRadius;
    }
    public void setRocketDistance(float rocketDistance)
    {
        rocketUpgradeParticleMethod();
        this.rocketDistance += rocketDistance;
    }
    public void setRocketCooldown(float rocketCooldown)
    {
        rocketUpgradeParticleMethod();
        this.rocketCooldown -= rocketCooldown;
    }
    public void setRocketCount()
    {
        isRocketAmount++;
        if (isRocketAmount == 2)
        {
            GameObject particle = Instantiate(rocketAmountParticle, RocketAmountAndUpgradeParticlePos.position, Quaternion.identity);
            particle.transform.SetParent(RocketAmountAndUpgradeParticlePos);
            Destroy(particle, RocketParticleDestroyTime);
            foreach(GameObject g in rocketParents)
            {
                g.SetActive(false);
            }
            rocketParents[1].SetActive(true);
        }else if(isRocketAmount == 3)
        {
            foreach (GameObject g in rocketParents)
            {
                g.SetActive(false);
            }
            rocketParents[2].SetActive(true);
        }
    }
    #region Shield
    public void getShield()
    {
        shieldObject.SetActive(true);
    }

    public bool checkShield()
    {
        if (shieldObject.activeSelf)
        {
            return false;
        }
        return true;
    }

    public void setMagnetRadius(float multiRadius)
    {
        coinMagnetCol.radius *= multiRadius;
    }
    #endregion
    #region Hearth

    public void getHeal()
    {
        if(hitPoint > 0 && hitPoint < 10)
        {
            GameObject g = Instantiate(healParticle, healParticlePos.position, Quaternion.identity);
            g.transform.SetParent(healParticleParent);
            Destroy(g, healParticleDestroyTime);
            hitPoint += 1;
            for (int i = 0; i < hitPoint; i++)
            {
                hearths[i].SetActive(true);
            }
        }
    }

    public bool checkHeal()
    {
        int hearhCount = 0;
        foreach(GameObject g in hearths)
        {
            if (g.activeSelf)
            {
                hearhCount++;
            }
        }
        if (hearhCount >= 10)
        {
            return false;
        }
        return true;
    }
    #endregion
}
