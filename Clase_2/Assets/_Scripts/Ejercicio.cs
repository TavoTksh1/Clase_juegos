using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Ejercicio : MonoBehaviour
{
    [Header ("Numbers")]
    public int a;
    public int b;
    private int _result;

    // Start is called before the first frame update
    void Start()
    {
        AddNumber();
    }

    public void AddNumber()
    {
        _result = a+b;
        Debug.Log("El resultado es:" + _result);
        Debug.Log(string.Format("El resultado es: {0}", _result));
    }
}
