using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSc : MonoBehaviour
{
	[SerializeField] private Rigidbody rb;
    Vector3 direction;

    private void Update()
    {
        if (rb.isKinematic==false)
        {
            rb.velocity = direction * 2000 * Time.deltaTime;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CoinMagnet"))
        {
            direction = -(transform.position - other.transform.position).normalized; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            other.GetComponent<CarCombat>().setGameManagerCoin();
            Destroy(gameObject);
        }
        if (other.CompareTag("CoinMagnet"))
        {
            direction = -(transform.position - other.transform.position).normalized;
            rb.isKinematic = false;
        }
    }
}
