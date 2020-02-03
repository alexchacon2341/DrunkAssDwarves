using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;

    public static InputManager Instance { get { return instance; } }

    [HideInInspector]
    public float Horizontal, Vertical;
    [HideInInspector]
    public bool Fire, JStart, KStart, KFire;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        Fire = Input.GetButtonDown("JoystickFire1");
        JStart = Input.GetButtonDown("JoystickStart");
        //Press E to start
        KStart = Input.GetButtonDown("Start");
        KFire = Input.GetButtonDown("Fire1");
    }
}
