using UnityEngine;
using UnityEngine.UI;

public enum Channel { red, green, blue, alpha, all }

public class GarageColorChange : MonoBehaviour
{
    public const string COLOR = "_Color";

    [SerializeField] private Renderer mRenderer;
    [SerializeField] private Slider mSlider;
    [SerializeField] private Channel mChannel;
    [SerializeField] private Color mStartingColor = Color.gray;
    private float mColorValue;


    private void Start()
    {
        if (PlayerPrefs.HasKey(gameObject.name))
        {
            mColorValue = PlayerPrefs.GetFloat(gameObject.name);
            mSlider.value = mColorValue;

        }
        else
        {
            mColorValue = mSlider.maxValue * 0.5f;
            mSlider.value = mColorValue;
            mRenderer.material.SetColor(COLOR, mStartingColor);
        }


    }

    public void ChangeSwordPartColour()
    {
        mColorValue = mSlider.value;

        switch (mChannel)
        {
            case Channel.all:
                mRenderer.material.SetColor(COLOR, new Color(mStartingColor.r * mColorValue,
                    mStartingColor.g * mColorValue, mStartingColor.b * mColorValue, 1));
                break;

            case Channel.red:
                mRenderer.material.SetColor(COLOR, new Color(mColorValue, mStartingColor.g * mStartingColor.b, mStartingColor.a));
                break;

            case Channel.green:
                mRenderer.material.SetColor(COLOR, new Color(mStartingColor.r, mColorValue, mStartingColor.b, mStartingColor.a));

                break;

            case Channel.blue:
                mRenderer.material.SetColor(COLOR, new Color(mStartingColor.r, mStartingColor.g, mColorValue, mStartingColor.a));

                break;

            case Channel.alpha:
                mRenderer.material.SetColor(COLOR, new Color(mStartingColor.r, mStartingColor.g, mStartingColor.b, mColorValue));
                break;


        }
    }

    public void SaveButton()
    {
        PlayerPrefs.SetFloat(gameObject.name, mColorValue);

    }


}