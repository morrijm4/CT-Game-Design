using UnityEngine;

public class Move : MonoBehaviour
{
    public Vector3 speed = new Vector3(0, 0, 0);
    public Vector3 rotate = new Vector3(0, 0, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(this.speed);
        transform.Rotate(this.rotate);
    }
}
