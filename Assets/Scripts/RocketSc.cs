using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSc : MonoBehaviour
{
    [SerializeField] private Rigidbody rigi;
    [SerializeField] private GameObject explosionParticle;
    [SerializeField] private float explosionParticleDestroyTime;
    private float damage;
    private float speed;
    private Transform particleContainer;
    private float expRadius;
    private LayerMask enemyLayer;

    private void Start()
    {
        Destroy(gameObject, 5);
    }
    public void setValues(float damage,float speed, float expRadius, LayerMask enemyLayer, Transform particleContainer)
    {
        this.damage = damage;
        this.speed = speed;
        this.expRadius = expRadius;
        this.enemyLayer = enemyLayer;
        this.particleContainer = particleContainer;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, expRadius, enemyLayer);
            foreach(Collider c in cols)
            {
                c.GetComponent<EnemyCar>().getDamage(damage);
            }
            GameObject g = Instantiate(explosionParticle, transform.position, Quaternion.identity);
            g.transform.SetParent(particleContainer);
            Destroy(g, explosionParticleDestroyTime);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        rigi.velocity = transform.forward * speed;
    }

}
