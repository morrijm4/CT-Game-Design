using UnityEngine;

public class Move : MonoBehaviour
{
    public Vector3 direction = new Vector3(0, 0, 0);
    public Vector3 rotate = new Vector3(0, 0, 0);

    void Start()
    {

    }

    void Update()
    {
        transform.Translate(this.direction);
        transform.Rotate(this.rotate);
    }
}
