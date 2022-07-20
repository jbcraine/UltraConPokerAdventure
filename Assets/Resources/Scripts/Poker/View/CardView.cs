using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    public CardFaceSprites faces;
    private Sprite currentFace;
    private Image cardRenderer;
    private bool _FaceUp;

    // Start is called before the first frame update
    void Awake()
    {
        cardRenderer = GetComponent<Image>();
    }

    private void Start()
    {
        currentFace = faces.cardBack;
        cardRenderer.sprite = currentFace;
    }

    public void ToggleFaceUp()
    {
        if (_FaceUp)
        {
            cardRenderer.sprite = faces.cardBack;
            _FaceUp = false;
        }
        else
        {
            cardRenderer.sprite = currentFace;
            _FaceUp = true;
        }
    }

    public void SetFaceUp(bool set)
    {
        _FaceUp = set;
        if (_FaceUp)
        {
            cardRenderer.sprite = currentFace;
        }
        else
        {
            cardRenderer.sprite = faces.cardBack;
        }
    }

    public void SetFace(CardFace face, CardSuit suit)
    {
        currentFace = faces.GetCardSprite(face, suit);
        transform.localScale = new Vector3(1, 1, 0);
        cardRenderer.sprite = currentFace;
        Debug.Log("Rendered");
    }
}
