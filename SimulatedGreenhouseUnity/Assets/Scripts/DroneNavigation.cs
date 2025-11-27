using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class DroneNavigation : MonoBehaviour
{
    public List<Vector3> waypoints;
    public float errorDist = 0.1f; // Por si
    public float timeStop = 1.0f; // CUanto tiempo se detiene

    private NavMeshAgent agent;
    private int currIndex = 0;
    private bool isChecking = true;
    private bool isPaused = false;
    private RemoverScript remover;
    public GreenhouseInitializer greenhouse;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        remover = FindObjectOfType<RemoverScript>();

        waypoints = greenhouse.GetCoordinates();

        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("Lista Vacia");
            isChecking = false;
            return;
        }

        goNextPlant();
    }

    void Update()
    {
        if (!isChecking || isPaused) return;
        if (!agent.pathPending && agent.remainingDistance < errorDist)
        {
            StartCoroutine(pausaChequeo());
        }
    }

    IEnumerator pausaChequeo()
    {
        isPaused = true;
        agent.isStopped = true; // Detiene

        yield return new WaitForSeconds(timeStop);
        agent.isStopped = false;

        currIndex++;
        if (currIndex >= waypoints.Count)
        {
            currIndex = 0;
        }

        goNextPlant();
        isPaused = false;
    }

    void goNextPlant()
    {
        if (agent.isActiveAndEnabled)
        {
            agent.SetDestination(waypoints[currIndex]);
        }
    }

    // Deteccion y ese Rollo

    private string tagBien = "Bien"; 
    private string tagInfected = "Infected"; 

    
    private void OnTriggerEnter(Collider other)
    {
   
        if (other.CompareTag(tagBien))
        {
            Debug.Log("Tomate Bien");
        }

        if (other.CompareTag(tagInfected))
        {
            Debug.Log("Tomate Infectado");
            Vector3 pos = other.transform.position;
            remover.AddRemoveRequest(pos);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagBien))
        {
            // Debug.Log("Adios Tomate Bien");
        }
    }
}
