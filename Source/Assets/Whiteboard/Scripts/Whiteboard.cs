using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whiteboard : MonoBehaviour
{
    public Texture2D[] canvas = new Texture2D[3];
    public Texture2D texture;
    public Vector2 textureSize = new Vector2(2048, 2048);

    private void Awake()
    {
        for(int i = 0; i< canvas.Length; i++)
        {
            canvas[i] = new Texture2D((int)textureSize.x, (int)textureSize.y);
        }
    }

    void Start()
    {
        Erase();
    }

    public void setTexture(int i)
    {
        var r = GetComponent<Renderer>();
        texture = canvas[i];
        r.material.mainTexture = texture;
    }

    public void Erase()
    {
        var r = GetComponent<Renderer>();
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        r.material.mainTexture = texture;
    }
}
