using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    public Text display;
    private int count;

    public int GetCount()
    {
        return count;
    }

    public int SetCount(int x)
    {
        count = x;
        if (display) display.text = "x " + count.ToString();
        return count;
    }

    public void Increment()
    {
        SetCount(GetCount() + 1);
    }

    public void Decrement()
    {
        SetCount(GetCount() - 1);
    }
}
