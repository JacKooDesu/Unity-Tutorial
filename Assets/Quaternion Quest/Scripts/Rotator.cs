﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = transform.rotation * Quaternion.AngleAxis(5f, transform.up);
    }
}
