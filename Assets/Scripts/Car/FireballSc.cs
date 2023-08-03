using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSc : MonoBehaviour
{
    [SerializeField] private GameObject expParticle;
    private Transform particleContainer;
    private float damage;
    [SerializeField] private GameObject fireballDamageParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameObject gparticle = Instantiate(fireballDamageParticle, transform.position, Quaternion.identity);
            Destroy(gparticle, 1);
            Instantiate(expParticle, transform.position, Quaternion.identity).transform.SetParent(particleContainer);
            other.GetComponent<EnemyCar>().getDamage(damage);
        }
    }

    public void setValues(float damage,Transform particleContainer)
    {
        this.damage = damage;
        this.particleContainer = particleContainer;
    }
}
