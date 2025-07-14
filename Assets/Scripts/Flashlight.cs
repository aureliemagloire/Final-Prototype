using UnityEngine;

public class Flashlight : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = mousePos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Echo"))
        {
            other.GetComponent<EchoAI>().Freeze();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Echo"))
        {
            other.GetComponent<EchoAI>().Unfreeze();
        }
    }
    
}

