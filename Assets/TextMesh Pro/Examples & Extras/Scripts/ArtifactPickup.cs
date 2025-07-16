using UnityEngine;

public class ArtifactPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Artifact collected: " + gameObject.name);
            Destroy(gameObject);  // Removing the artifact
        }
    }
}

