using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public float money = 100f;

    public void Pay(float amount)
    {
        money -= amount;
        Debug.Log("Te queda " + money + " de dinero");
    }
}
