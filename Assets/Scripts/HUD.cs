using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum Infotype { Exp, Level, Kill, Time, Health}
    public Infotype type;

    Text myText;
    Slider mySlider;

    private void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case Infotype.Exp:
                float curExp = GameManager.instance.exp;
                float mawExp = GameManager.instance.nextExp[GameManager.instance.level];
                mySlider.value = curExp / mawExp;
                break;

            case Infotype.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                break;

            case Infotype.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;

            case Infotype.Time:

                break;

            case Infotype.Health:

                break;
        }
    }
}
