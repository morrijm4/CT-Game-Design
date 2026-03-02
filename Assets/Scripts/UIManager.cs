using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public string prefix = "x ";
    public Text pelletCount;
    public Text bombCount;
    private PelletShooter pelletShooter;
    private BombShooter bombShooter;

    void Awake()
    {
        pelletShooter = GetComponent<PelletShooter>();
        bombShooter = GetComponent<BombShooter>();

        if (pelletShooter == null)
            Debug.LogError("PelletShooter is null in UIManager");
        if (bombShooter == null)
            Debug.LogError("BombShooter is null in UIManager");
    }

    void Start()
    {
        UpdateBombCount();
        UpdatePelletCount();
    }

    public void UpdateBombCount()
    {
        if (bombCount == null) return;
        pelletCount.text = prefix + pelletShooter.GetCount().ToString();
    }

    public void UpdatePelletCount()
    {
        if (pelletCount == null) return;
        pelletCount.text = prefix + pelletShooter.GetCount().ToString();
    }
}
