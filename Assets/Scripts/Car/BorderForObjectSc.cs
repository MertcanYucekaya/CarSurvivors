using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BorderForObjectSc : MonoBehaviour
{
    [SerializeField] private Rigidbody motorSphereRigi;
    [SerializeField] private float recoilPower;
    [SerializeField] private CarController carController;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Border"))
        {
            carController.carControl = false;
            motorSphereRigi.velocity = Vector3.zero * recoilPower;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Border"))
        {
            Invoke("setForwardSpeed", .5f);
        }
    }
    void setForwardSpeed()
    {
        carController.carControl = true;
    }
}
