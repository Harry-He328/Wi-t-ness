using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public Cinemachine.CinemachineVirtualCamera cine1, cine2;
    public bool activePlayer1 = true;
    public bool activePlayer2 = false;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(SwitchCameraAndUpdatePlayer());
        }
    }

    private IEnumerator SwitchCameraAndUpdatePlayer()
    {
        if (cine1.Priority > cine2.Priority) // 切换到player2
        {
            cine1.Priority -= 1;
            cine2.Priority += 1;
            activePlayer1 = false;
            yield return new WaitForSeconds(1.0f); // 等待一秒
            activePlayer2 = true;
        }
        else // 切换到player1
        {
            cine1.Priority += 1;
            cine2.Priority -= 1;
            activePlayer2 = false;
            yield return new WaitForSeconds(1.0f); // 等待一秒
            activePlayer1 = true;
        }
    }
}