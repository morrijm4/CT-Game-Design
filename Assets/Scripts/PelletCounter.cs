using UnityEngine;
using UnityEngine.UI;
public class PelletCounter : MonoBehaviour
{ 
    public Text pelletCounter;
    private playerController playerController;

    private void Awake()
    {
        playerController = GetComponentInParent<playerController>();
        if (playerController == null)
        {
            Debug.LogError("PelletCounter: No playerController found in parent.");
        }
    }
    void Start()
    {

        pelletCounter.text = "x 0";
    }

    // Update is called once per frame
    void Update()
    {
        int count = playerController.capital;
        Debug.Log("PelletCounter: Updating pellet count to " + count);
        pelletCounter.text = "x" + count.ToString();
    }
}
