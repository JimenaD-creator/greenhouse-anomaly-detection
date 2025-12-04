using UnityEngine;

public class TomatoScript : MonoBehaviour
{
    [Header("Gorwth Settings")]
    public GameObject nextPhase;     // Preset Tomate Maduro
    private float minDelay = 2f;          // Tiempo mínimo de espera en Minutos
    private float maxDelay = 4f;          // Tiempo máximo de espera en Minutos

    private float waitTime;
    private Transform originalParent;

    void Start()
    {
        originalParent = transform.parent;
        waitTime = Random.Range(minDelay, maxDelay);

        StartCoroutine(DestroyAndSpawn());
    }

    private System.Collections.IEnumerator DestroyAndSpawn()
    {
        yield return new WaitForSeconds(waitTime*60f);

        if (nextPhase != null && gameObject != null)
        {
            GameObject newObj = Instantiate(nextPhase, transform.position, transform.rotation);
            newObj.transform.SetParent(originalParent);
        }
        Destroy(gameObject);
    }
}
