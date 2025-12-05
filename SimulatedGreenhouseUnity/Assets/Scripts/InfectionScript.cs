using UnityEngine;
using System.Collections;

public class PropagationRaycaster : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float minRange = 0.0f;
    public float maxRange = 2.0f;
    public LayerMask targetLayer;

    [Header("Propagation Settings")]
    public GameObject prefabToSpawn;

    private float minDelay = 0.0f;
    private float maxDelay = 0.5f;
    private float waitTime;

    private bool hasPropagated = false;
    private bool canPropagate = false;

    private void Start()
    {
        StartCoroutine(EnablePropagation());
    }

    IEnumerator EnablePropagation()
    {
        yield return new WaitForSeconds(0.1f);
        canPropagate = true;
    }

    private void Update()
    {
        if (canPropagate && !hasPropagated)
        {
            StartCoroutine(PropagationRoutine());
        }
    }

    IEnumerator PropagationRoutine()
    {
        hasPropagated = true;

        waitTime = Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(waitTime*60);

        Debug.Log("Start Propagation...");

        TryPropagate(transform.forward);
        TryPropagate(-transform.forward);
    }

    void TryPropagate(Vector3 direction)
{
    float distance = Random.Range(minRange, maxRange);
    RaycastHit hit;

    if (Physics.Raycast(transform.position, direction, out hit, distance, targetLayer))
    {
        Debug.DrawRay(transform.position, direction * distance, Color.red, 1f);

        GameObject target = hit.collider.gameObject;

        if (target == this.gameObject) return;
        if (target.CompareTag("Infected")) return;
        if (target.CompareTag("Maduro")) return;

        Vector3 spawnPos = target.transform.position;

        Destroy(target);

        GameObject prefab = prefabToSpawn != null ? prefabToSpawn : this.gameObject;

        Instantiate(prefab, spawnPos, Quaternion.Euler(0, 90, 0));
        Debug.Log("Infectado...");
    }
    else
    {
        Debug.DrawRay(transform.position, direction * distance, Color.green, 1f);
        Debug.Log("No Infectado...");
    }
}

}
