using UnityEngine;

public class ArtifactBehavior : MonoBehaviour
{
    private SpriteRenderer sr;
    private bool isVisible = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // Make the artifact invisible at the start
        if (sr != null)
        {
            sr.color = new Color(1f, 1f, 1f, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Flashlight reveals the artifact
        if (other.CompareTag("Flashlight") && !isVisible)
        {
            Debug.Log("Artifact revealed: " + gameObject.name);
            sr.color = new Color(1f, 1f, 1f, 1f); // Fully visible
            isVisible = true;
        }

        // Player can collect it only if it's visible
        if (other.CompareTag("Player") && isVisible)
        {
            Debug.Log("Artifact collected: " + gameObject.name);
            Destroy(gameObject);
        }
    }
}

