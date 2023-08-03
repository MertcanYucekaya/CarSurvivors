using Lofelt.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CarEnvExplosion : MonoBehaviour
{
    [Header("Tree")]
    [SerializeField] private GameObject explodedTree;
    [SerializeField] private float treeExpForce;
    [Header("Block")]
    [SerializeField] private GameObject explodedBlock;
    [SerializeField] private float blockExpForce;
    private void Start()
    {
        DOTween.Init();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ExpTree"))
        {
            instantExploded(explodedTree, other, treeExpForce);
        }
        if (other.CompareTag("ExpBlock"))
        {
            instantExploded(explodedBlock, other, blockExpForce);
        }
    }

    void instantExploded(GameObject exploded, Collider other, float force) 
    {
        HapticPatterns.PlayEmphasis(0.5f, 0.05f);
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

    IEnumerator envDestroyAnimation(Transform[] envs)
    {
        yield return new WaitForSeconds(5);
        foreach(Transform t in envs)
        {
            t.DOLocalMoveY(-5, 4);
        }
        yield return new WaitForSeconds(4);
        foreach (Transform t in envs)
        {
            Destroy(t.gameObject);
        }
    }
}
