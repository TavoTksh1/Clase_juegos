using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public SpriteRenderer _renderer;
    public GameObject triangle;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        ChangeColor_();
        TP();

    }
    public void ChangeColor_()
    {
        _renderer.color = Color.red;
    }

    public void TP()
    {
        transform.position = triangle.transform.position;
    }
}
