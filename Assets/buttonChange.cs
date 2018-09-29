using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonChange : MonoBehaviour 
{
    public Sprite Button_1;
    SpriteRenderer renderer;
    
	void Start () 
	{
        renderer = GetComponent<SpriteRenderer>();
	}

    public void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag.Equals("DroppedPickUp"))
        {
            renderer.sprite = Button_1;
        }
    }
}
