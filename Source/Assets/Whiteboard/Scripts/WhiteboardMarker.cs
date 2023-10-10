using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class WhiteboardMarker : MonoBehaviour
{
    [field: SerializeField] private Transform PencilTip { get; set; }
    [field: SerializeField] private int penSize {get ;set;}
    [field: SerializeField] private Color penColor { get; set; }

    //
    private Whiteboard board { get; set; }

    private Color[] color { get; set; }
    private float height { get; set; }

    private bool lastFrame { get; set; }
    private RaycastHit touch;
    private Vector2 touchPos { get; set; }
    private Vector2 lastTouchPos { get; set; }
    private Quaternion _lastTouchRot{ get; set; }
    
    void Start()
    {
        Application.targetFrameRate = 122;
        setColor(penColor);
        height = PencilTip.localScale.y;
    }


    float duration = 0;

    void Update()
    {
        duration += Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (duration >= 0.1f)
        {
            duration = 0;
            Draw();
        }
    }

    public void setRed()
    {
        setColor(Color.red);
    }
    public void setBlue()
    {
        setColor(Color.blue);
    }
    public void setGreen()
    {
        setColor(Color.green);
    }

    private void setColor(Color color)
    {
        this.color = Enumerable.Repeat(color, penSize * penSize).ToArray();
    }

    private void Draw()
    {
        if (Physics.Raycast(PencilTip.position, transform.up, out touch, height))
        {
            if (touch.transform.CompareTag("Whiteboard"))
            {
                if (board == null) board = touch.transform.GetComponent<Whiteboard>();

                touchPos = new Vector2(touch.textureCoord.x, touch.textureCoord.y);
                var x = (int)(touchPos.x * board.textureSize.x - (penSize * 0.5f));
                var y = (int)(touchPos.y * board.textureSize.y - (penSize * 0.5f));

                if (y < 0 || y > board.textureSize.y || x < 0 || x > board.textureSize.x) return;

                if (lastFrame)
                {
                    board.texture.SetPixels(x, y, penSize, penSize, color);

                    for (float f = 0.01f; f < 1.00f; f += 0.01f)
                    {
                        var lerpX = (int)Mathf.Lerp(lastTouchPos.x, x, f);
                        var lerpY = (int)Mathf.Lerp(lastTouchPos.y, y, f);

                        board.texture.SetPixels(lerpX, lerpY, penSize, penSize, color);
                    }

                    transform.rotation = _lastTouchRot;
                    
                    board.texture.Apply();
                }

                lastTouchPos = new Vector2(x, y);
                _lastTouchRot = transform.rotation;
                lastFrame = true;

                return;
            }
        }

        board = null;
        lastFrame = false;
    }
}
