﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScrollingScript : MonoBehaviour
{
    public Vector2 speed = new Vector2(10, 10);
    public Vector2 direction = new Vector2(-1, 0);

    public bool isLinkedToCamera = false;
    public bool isLooping = true;
    public float timer;

    private float defaultTimer = 30f;     //Määrittele aika jonka jälkeen ensimmäinen sprite siirtyy viimeiseksi

    private List<SpriteRenderer> backgroundPart;
    
    void Start()
    {
        ScrollStart();
    }

    void Update()
    {
        ScrollUpdate();
    }

    void ScrollStart()
    {
        if (isLooping)
        {
            backgroundPart = new List<SpriteRenderer>();

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                SpriteRenderer r = child.GetComponent<SpriteRenderer>();

                if (r != null)
                {
                    backgroundPart.Add(r);
                }
            }

            backgroundPart = backgroundPart.OrderBy(t => t.transform.position.x).ToList();
        }
    }

    void ScrollUpdate()
    {
        Vector3 movement = new Vector3(speed.x * direction.x, speed.y * direction.y, 0);

        movement *= Time.deltaTime;
        transform.Translate(movement);

        if (isLinkedToCamera)
        {
            Camera.main.transform.Translate(movement);
        }

        if (isLooping)
        {
            SpriteRenderer firstChild = backgroundPart.FirstOrDefault();

            if (firstChild != null)
            {
                if (firstChild.transform.position.x < Camera.main.transform.position.x)
                {
                    if (firstChild.IsVisibleFrom(Camera.main) == true)
                    {
                        timer = defaultTimer;
                    }

                    if (firstChild.IsVisibleFrom(Camera.main) == false)
                    {
                        timer = timer - Time.deltaTime;

                        if (timer < 0)
                        {
                            SpriteRenderer lastChild = backgroundPart.LastOrDefault();

                            Vector3 lastPosition = lastChild.transform.position;
                            Vector3 lastSize = (lastChild.bounds.max - lastChild.bounds.min);

                            firstChild.transform.position = new Vector3(lastPosition.x + lastSize.x, firstChild.transform.position.y, firstChild.transform.position.z);

                            backgroundPart.Remove(firstChild);
                            backgroundPart.Add(firstChild);
                        }
                    }
                }
            }
        }
    }
}