using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputScript : MonoBehaviour
{
    [SerializeField] public int Resolution;

    private void Awake()
    {
        Resolution = 2;
    }
}
