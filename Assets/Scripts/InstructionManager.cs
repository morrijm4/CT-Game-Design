using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionSequencer : MonoBehaviour
{
    [Header("Instruction Panels (in order)")]
    [SerializeField] private GameObject[] instructionPanels;
    [SerializeField] private float secondsPerPanel = 3f;

    void Start()
    {
        foreach (var panel in instructionPanels)
            panel.SetActive(false);

        StartCoroutine(RunSequence());
    }

    private IEnumerator RunSequence()
    {
        foreach (var panel in instructionPanels)
        {
            panel.SetActive(true);
            yield return new WaitForSeconds(secondsPerPanel);
            panel.SetActive(false);
        }

        SceneManager.LoadScene("Arena");
    }
}