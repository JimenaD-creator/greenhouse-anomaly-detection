using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class RemoverScript : MonoBehaviour
{
    [Header("Robot Settings")]
    private string targetTag = "Infected";
    public float removeTime = 5f;

    private Queue<Vector3> removeQueue = new Queue<Vector3>();
    private NavMeshAgent agent;

    private bool isWorking = false;
    private bool returningToOrigin = false;

    private Vector3 originPoint;
    private GameObject currentTarget;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originPoint = transform.position;
    }

    
    public void AddRemoveRequest(Vector3 position)
    {
        removeQueue.Enqueue(position);

        if (returningToOrigin)
        {
            returningToOrigin = false;
            StopAllCoroutines();
            StartNextTask();
            return;
        }

        if (!isWorking)
        {
            StartNextTask();
        }
    }

    void StartNextTask()
    {
        if (removeQueue.Count == 0)
        {
            // Si no hay tareas → volver al origen
            StartCoroutine(ReturnToOrigin());
            return;
        }

        Vector3 nextPos = removeQueue.Dequeue();
        isWorking = true;

        agent.SetDestination(nextPos);
        StartCoroutine(WaitToReachTarget(nextPos));
    }

    IEnumerator WaitToReachTarget(Vector3 targetPos)
    {
        while (Vector3.Distance(transform.position, targetPos) > 1.0f)
        {
            yield return null;
        }

        currentTarget = FindClosestTarget(targetPos);

        if (currentTarget != null)
        {
            yield return StartCoroutine(RemoveObject());
        }

        isWorking = false;

        StartNextTask();
    }

    IEnumerator RemoveObject()
    {
        Debug.Log("Trabajando para remover: " + currentTarget.name);

        yield return new WaitForSeconds(removeTime);

        Destroy(currentTarget);
        Debug.Log("Objeto removido.");
    }

    GameObject FindClosestTarget(Vector3 around)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(targetTag);
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (var o in objs)
        {
            float d = Vector3.Distance(around, o.transform.position);

            if (d < minDist)
            {
                minDist = d;
                closest = o;
            }
        }

        return closest;
    }

    IEnumerator ReturnToOrigin()
    {
        returningToOrigin = true;
        agent.SetDestination(originPoint);

        while (Vector3.Distance(transform.position, originPoint) > agent.stoppingDistance + 0.2f)
        {
            yield return null;
        }

        returningToOrigin = false;
        StartNextTask();
}
}
