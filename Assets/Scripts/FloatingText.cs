using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float fadeSpeed = 2f;
    public float moveSpeed = 2f;
    TextMeshPro tmpro;
    Color prevColor;

    bool isActive = false;

    void Start()
    {
        tmpro = GetComponent<TextMeshPro>();
        prevColor = tmpro.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            float alpha = Mathf.Lerp(tmpro.color.a, 0, fadeSpeed * Time.deltaTime);
            tmpro.color = new Color(prevColor.r, prevColor.g, prevColor.b, alpha);
            transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        }
        
    }

    public void Setup(int damage)
    {
        tmpro = GetComponent<TextMeshPro>();
        tmpro.text = "-"+damage.ToString()+" DMG";
        isActive = true;
    }
}
