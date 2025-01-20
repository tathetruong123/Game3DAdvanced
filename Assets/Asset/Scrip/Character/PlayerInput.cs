using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector] public float horizontalInput;
    [HideInInspector] public float verticalInput;

    public bool attackInput;   // Tấn công
    public bool jumpInput;     // Nhảy
    public bool crouchToggle;  // Cúi (bật/tắt)
    public bool laughInput;    // Cười
    public bool sprintInput;   // Chạy nhanh

    private bool isCrouching = false; // Trạng thái hiện tại của cúi

    private void Update()
    {
        // Lấy giá trị đầu vào di chuyển
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Kiểm tra phím chạy nhanh
        sprintInput = Input.GetKey(KeyCode.LeftShift);

        // Kiểm tra phím tấn công K
        if (!attackInput && Time.timeScale != 0)
        {
            attackInput = Input.GetKeyDown(KeyCode.K);
        }

        // Kiểm tra phím nhảy
        if (!jumpInput && Time.timeScale != 0)
        {
            jumpInput = Input.GetKeyDown(KeyCode.J); // Nhấn phím J
        }

        // Kiểm tra phím cười
        if (!laughInput && Time.timeScale != 0)
        {
            laughInput = Input.GetKeyDown(KeyCode.X); // Nhấn phím X
        }
    }

    private void OnDisable()
    {
        // Đặt lại tất cả các trạng thái đầu vào
        horizontalInput = 0;
        verticalInput = 0;
        attackInput = false;
        jumpInput = false;
        crouchToggle = false;
        laughInput = false;
        sprintInput = false;

        isCrouching = false;
    }
}
