using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ReturnHome : MonoBehaviour
{
    public int players = 4;

    void Update()
    {
        for (int i = 0; i < players; i++)
        {
            if (Gamepad.all.Count > i && Gamepad.all[i].buttonSouth.wasPressedThisFrame)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
