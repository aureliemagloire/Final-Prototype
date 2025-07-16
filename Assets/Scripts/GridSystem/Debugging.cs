using UnityEngine;

public class PuzzleTestTrigger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            var manager = Object.FindFirstObjectByType<PuzzleManager>();
            if (manager != null)
            {
                Debug.Log("Found PuzzleManager! Starting puzzle...");
                manager.StartPuzzle();
            }
            else
            {
                Debug.LogError("PuzzleManager not found in scene!");
            }
        }
    }
}
