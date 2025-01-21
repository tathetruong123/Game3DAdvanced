using System.Collections;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public ParticleSystem rainEffect;      // Hiệu ứng mưa
    public ParticleSystem snowEffect;      // Hiệu ứng tuyết
    public Light directionalLight;        // Ánh sáng mặt trời
    public Gradient dayNightLightColor;   // Màu ánh sáng cho ngày/đêm 
    public AudioSource rainAudioSource;   // Âm thanh mưa
    public AudioSource snowAudioSource;   // Âm thanh tuyết

    [Range(0, 24)] public float currentHour = 12f; // Thời gian hiện tại trong ngày 
    public float timeSpeed = 0.1f;

    private bool isRaining = false;
    private bool isSnowing = false;

    void Start()
    {
        StartCoroutine(TimeCycle());
    }

    IEnumerator TimeCycle()
    {
        while (true)
        {
            UpdateWeather();
            yield return new WaitForSeconds(1f); // Cập nhật mỗi giây
        }
    }

    void UpdateWeather()
    {
        // Giả lập thời gian trôi
        currentHour += Time.deltaTime * timeSpeed;
        if (currentHour >= 24f) currentHour = 0f;

        // Điều chỉnh ánh sáng theo thời gian
        float normalizedTime = currentHour / 24f; 
        directionalLight.color = dayNightLightColor.Evaluate(normalizedTime); // Thay đổi màu ánh sáng

        // Chuyển đổi giữa mưa và tuyết
        if (currentHour >= 6f && currentHour < 18f) 
        {
            StartRain();
        }
        else 
        {
            StartSnow();
        }
    }

    void StartRain()
    {
        if (!isRaining)
        {
            StopSnow();

            rainEffect.Play();
            rainAudioSource.Play();
            Debug.Log("Rain started!");
            isRaining = true;
        }
    }

    void StopRain()
    {
        if (isRaining)
        {
            rainEffect.Stop();
            rainAudioSource.Stop();
            Debug.Log("Rain stopped!");
            isRaining = false;
        }
    }

    void StartSnow()
    {
        if (!isSnowing)
        {
            StopRain();

            snowEffect.Play();
            snowAudioSource.Play();
            Debug.Log("Snow started!");
            isSnowing = true;
        }
    }

    void StopSnow()
    {
        if (isSnowing)
        {
            snowEffect.Stop();
            snowAudioSource.Stop();
            Debug.Log("Snow stopped!");
            isSnowing = false;
        }
    }
}
