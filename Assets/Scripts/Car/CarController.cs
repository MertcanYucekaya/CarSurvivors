using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("ControllerValues")]
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float driftForTurnDistance;
    private float forwardSpeedC;
    [HideInInspector] public bool carControl;
    [Header("CarBodySlop")]
    [SerializeField] private float carBodySlopStart;
    [SerializeField] private float carBodySlopEnd;
    [SerializeField] private float carBodyMaxRotation;
    [SerializeField] private float carBodyFixTimeSpeed;
    [SerializeField] private Transform carBody;
    private bool isRight;
    [Header("CameraShake")]
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private float duration;
    [SerializeField] private float magnitude;
    [Header("TransprentEffect")]
    [SerializeField] private LayerMask transLayer;
    [SerializeField] private Transform unTransparentParent;
    [SerializeField] private Transform transparentParent;
    [SerializeField] private Material opaqMaterial;
    [SerializeField] private Material fadeMaterial;
    private Transform hitReturn;
    RaycastHit hit;
    [Header("References")]
    [SerializeField] private Transform camera;
    [SerializeField] private Joystick joystick;
    [SerializeField] private Transform lookAtObject;
    [SerializeField] private Rigidbody sphereRigi;
    [SerializeField] private TrailRenderer[] wheelTrails;
    void Start()
    {
        carControl = false;
        forwardSpeedC = forwardSpeed;
        forwardSpeed = 0;
    }
    public void carControlForStart()
    {
        sphereRigi.transform.parent = null;
        carControl = true;
        forwardSpeed = forwardSpeedC;
        
    }
    void Update()
    {
        transEffect();
        if (joystick.Direction != Vector2.zero && Time.timeScale>0 && carControl)
        {
            Vector3 lookPos= transform.position + (new Vector3(joystick.Direction.x, 0, joystick.Direction.y) * 8);
            lookPos.y = transform.position.y;
            lookAtObject.LookAt(lookPos);
            float lastEuler = transform.localEulerAngles.y;
            transform.rotation = Quaternion.Lerp(transform.rotation, lookAtObject.rotation, Time.deltaTime * turnSpeed);
            float currentEuler = transform.localEulerAngles.y;
            carSlopMethod(lastEuler, currentEuler);
            driftControl();
        }
        else
        {
            if (wheelTrails[0].emitting)
            {
                foreach (TrailRenderer t in wheelTrails)
                {
                    t.emitting = false;
                }
            }
            if (carBody.eulerAngles.z != 0)
            {
                carBody.localRotation = Quaternion.Lerp(carBody.localRotation, Quaternion.identity, Time.deltaTime * carBodyFixTimeSpeed);
            }
        }
        transform.position = sphereRigi.transform.position;
    }

    private void FixedUpdate()
    {
        sphereRigi.AddForce(transform.forward * forwardSpeed, ForceMode.Acceleration);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Border"))
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z); ;
        }
    }
    void driftControl()
    {
        float lookPosDistance = Quaternion.Angle(transform.rotation , lookAtObject.transform.rotation);
        if (wheelTrails[0].emitting == false)
        {
            if (Mathf.Abs(lookPosDistance) >= driftForTurnDistance)
            {
                foreach (TrailRenderer t in wheelTrails)
                {
                    t.emitting = true;
                }
            }
        }
        else
        {
            if (Mathf.Abs(lookPosDistance) < driftForTurnDistance)
            {
                foreach (TrailRenderer t in wheelTrails)
                {
                    t.emitting = false;
                }
            }
        }
    }

    void carSlopMethod(float lastEuler,float currentEuler)
    {
        if (lastEuler < currentEuler)
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }
        if (lastEuler - currentEuler > 300)
        {
            isRight = true;
        }
        if (currentEuler - lastEuler > 300)
        {
            isRight = false;
        }
        if (isRight)
        {
            carBody.localEulerAngles = new Vector3(0, 0
                , carBodyMaxRotation *
                Mathf.InverseLerp(carBodySlopStart, carBodySlopEnd
                , Quaternion.Angle(transform.rotation, lookAtObject.transform.rotation)));
        }
        else
        {
            carBody.localEulerAngles = new Vector3(0, 0
                , (carBodyMaxRotation * -1) *
                Mathf.InverseLerp(carBodySlopStart, carBodySlopEnd
                , Quaternion.Angle(transform.rotation, lookAtObject.transform.rotation)));
        }
    }

    public void cameraShakeMethod()
    {
        cameraShake.StartShake(duration,magnitude);
    }

    void transEffect()
    {
        if (Physics.Raycast(camera.position, camera.forward, out hit,50, transLayer))
        {
            hitReturn = hit.transform;
            if(hitReturn.transform.parent == unTransparentParent)
            {
                hitReturn.GetComponent<MeshRenderer>().material = fadeMaterial;
                hitReturn.SetParent(transparentParent);
            }
        }
        else if (transparentParent.childCount > 0)
        {
            transparentParent.GetChild(0).GetComponent<MeshRenderer>().material = opaqMaterial;
            transparentParent.GetChild(0).transform.SetParent(unTransparentParent);
        }
    }

    public void stopCar()
    {
        forwardSpeed = 0;
        carControl = false;
    }
}
