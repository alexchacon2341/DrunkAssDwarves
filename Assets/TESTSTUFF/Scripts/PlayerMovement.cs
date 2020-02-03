﻿/*
 * Copyright (c) 2018 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private AudioSource audioSrc;

    public float speed = 3; 

    [SerializeField]
    private float speedMultiplier;

    void Awake()
    {
        //rb2d = GetComponent<Rigidbody2D>();
        audioSrc = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        //rb2d.velocity = new Vector2(Mathf.Lerp(0, Input.GetAxis("Horizontal") * speedMultiplier, 0.8f),
        //                            Mathf.Lerp(0, Input.GetAxis("Vertical") * speedMultiplier, 0.8f));
        Vector2 moveDirection = Vector2.zero;
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");

        if (horizontal > 0)
        {
            moveDirection.x = 1;
            //animator.SetInteger("Direction", 2);
        }
        else if (horizontal < 0)
        {
            moveDirection.x = -1;
            //animator.SetInteger("Direction", 0);
        }
        else if (vertical > 0)
        {
            moveDirection.y = 1;
            //animator.SetInteger("Direction", 1);
        }
        else if (vertical < 0)
        {
            moveDirection.y = -1;
          //  animator.SetInteger("Direction", 3);
        }
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (!audioSrc.isPlaying)
            {
                audioSrc.Play();
            }
        }
    }
}