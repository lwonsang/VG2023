using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EvolveTemp : CharacterBase
{
    public Sprite turnLeft;
    public Sprite turnRight;
    public Sprite turnUp;
    public Sprite turnDown;

    public Sprite turnLeftEvolved;
    public Sprite turnRightEvolved;
    public Sprite turnUpEvolved;
    public Sprite turnDownEvolved;

    public Boolean Evolved = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Evolved==false){
            if (Input.GetKey(KeyCode.Space)) {
                transform.localScale += new Vector3(1, 1, 1);
                Evolved = true;
            }
            //Slowed down movement from 0.2f
            //Move Up
            if (Input.GetKey(KeyCode.UpArrow)) {
                transform.position += new Vector3(0, 0.05f, 0);
                _spriterenderer.sprite = turnUp;
            }

            //Move Down
            if (Input.GetKey(KeyCode.DownArrow)) {
                _spriterenderer.sprite = turnDown;
                transform.position += new Vector3(0, -0.05f, 0);
            }

            //Move Left
            if (Input.GetKey(KeyCode.LeftArrow)) {
                transform.position += new Vector3(-0.05f, 0, 0);
                _spriterenderer.sprite = turnLeft;
            }

            //Move Right
            if (Input.GetKey(KeyCode.RightArrow)) {
                transform.position += new Vector3(0.05f, 0, 0);
                _spriterenderer.sprite = turnRight;
            }
        }

        if (Evolved==true){
            //Slowed down movement from 0.2f
            //Move Up
            if (Input.GetKey(KeyCode.UpArrow)) {
                transform.position += new Vector3(0, 0.01f, 0);
                _spriterenderer.sprite = turnUpEvolved;
            }

            //Move Down
            if (Input.GetKey(KeyCode.DownArrow)) {
                _spriterenderer.sprite = turnDownEvolved;
                transform.position += new Vector3(0, -0.01f, 0);
            }

            //Move Left
            if (Input.GetKey(KeyCode.LeftArrow)) {
                transform.position += new Vector3(-0.01f, 0, 0);
                _spriterenderer.sprite = turnLeftEvolved;
            }

            //Move Right
            if (Input.GetKey(KeyCode.RightArrow)) {
                transform.position += new Vector3(0.01f, 0, 0);
                _spriterenderer.sprite = turnRightEvolved;
            }
        }
    }
}
