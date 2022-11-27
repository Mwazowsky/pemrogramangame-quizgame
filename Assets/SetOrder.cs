using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetOrder : MonoBehaviour
{
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas = this.gameObject.GetComponent<Canvas>();
    }

    public void ChangeOrdertoOne()
    {
        canvas.sortingOrder = 1;
    }

    public void ChangeOrdertoZero()
    {
        canvas.sortingOrder = 0;
    }
}
