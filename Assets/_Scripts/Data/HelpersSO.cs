using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HelpersSO", menuName = "HelperSO")]
public class HelpersSO : ScriptableObject
{
    public void DestoryObj(GameObject target)
    {
        Destroy(target);
    }
}
