using UnityEngine;
using UnityEngine.UI;

public class SliderTimer : MonoBehaviour
{
    public Slider countdownSlider;
    public float totalTime = 180f;
    private float currentTime;

    void Start()
    {
        currentTime = totalTime;
        countdownSlider.maxValue = totalTime;
        countdownSlider.value = totalTime;
    }

    void Update()
    {
        currentTime -= Time.deltaTime;
        currentTime = Mathf.Max(0f, currentTime);
        countdownSlider.value = currentTime;
    }
}
