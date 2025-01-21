using System.Collections;
using UnityEngine;

public class RainAndLightningController : MonoBehaviour
{
    public GameObject[] lightningPrefabs;   // Mảng sấm sét 
    public ParticleSystem rainEffect;      // Hiệu ứng mưa 
    public AudioSource rainAudioSource;    // Âm thanh mưa
    public AudioClip[] lightningSounds;    // Mảng âm thanh sấm sét

    private int currentLightningIndex = 0; 

    void Start()
    {
        StartCoroutine(RainCycle());
        StartCoroutine(LightningCycle());
    }

    IEnumerator RainCycle()
    {
        while (true)
        {
            // Bắt đầu mưa
            rainEffect.Play();
            rainAudioSource.Play();
            Debug.Log("Rain started!");
            yield return new WaitForSeconds(120f); 

            // Dừng mưa
            rainEffect.Stop();
            rainAudioSource.Stop();
            Debug.Log("Rain stopped!");
            yield return new WaitForSeconds(60f); 
        }
    }

    IEnumerator LightningCycle()
    {
        while (true)
        {
            // Hiển thị loại sấm sét hiện tại
            SpawnLightningWithSound();
            yield return new WaitForSeconds(15f); 
        }
    }

    void SpawnLightningWithSound()
    {
        if (lightningPrefabs.Length == 0 || lightningSounds.Length == 0) return;

        // Tạo sấm sét từ Prefab hiện tại
        GameObject lightning = Instantiate(lightningPrefabs[currentLightningIndex], transform.position, Quaternion.identity);
        Destroy(lightning, 5f); 

        // Phát âm thanh sấm sét
        AudioSource.PlayClipAtPoint(lightningSounds[currentLightningIndex], transform.position);

        // Đổi sang loại sấm sét tiếp theo
        currentLightningIndex = (currentLightningIndex + 1) % lightningPrefabs.Length;

        Debug.Log($"Lightning {currentLightningIndex + 1} appeared with sound!");
    }
}
