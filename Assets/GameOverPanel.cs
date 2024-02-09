using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    public PlayerInput playerInput;
    private void OnEnable()
    {
        playerInput.currentActionMap.Disable();
        Time.timeScale = Mathf.Epsilon;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnDisable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;

        playerInput.currentActionMap.Enable();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
