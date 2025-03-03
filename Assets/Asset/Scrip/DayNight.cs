using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float dayDuration = 60f; // Một ngày kéo dài 60 giây
    public Light sunLight;
    public Gradient lightColor; // Dùng để thay đổi màu theo thời gian

    private float time; // Biến đếm thời gian trong ngày

    void Update()
    {
        time += Time.deltaTime / dayDuration;
        float rotationAngle = time * 360f; // Xoay đủ 360 độ trong 1 ngày
        transform.rotation = Quaternion.Euler(rotationAngle, 0, 0);

        // Thay đổi màu ánh sáng theo thời gian
        sunLight.color = lightColor.Evaluate(time % 1);
    }
}
