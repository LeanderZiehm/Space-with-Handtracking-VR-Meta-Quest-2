using TMPro;
using UnityEngine;

public class MyDisplay : MonoBehaviour
{
    private TextMeshProUGUI textField;

    private TextMeshProUGUI GetTextField()
    {
        if (textField == null)
        {
            textField = GetComponent<TextMeshProUGUI>();
            
            if (textField == null)
            {
                textField = GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        return textField;
    }

    public void Display(string str)
    {
        GetTextField().text = str;
    }

    public void Display(int intValue)
    {
        GetTextField().text = intValue.ToString();
    }

    public void Display(float floatValue)
    {
        GetTextField().text = floatValue.ToString();
    }
    public void AddToDisplay(string str)
    {
        GetTextField().text += str;
    }

    public void AddToDisplayFromTopInNewLine(string str,string color = null)
    {

        if (color == null)
        {
            GetTextField().text = str + "\n" + GetTextField().text;
        }
        else
        {
            GetTextField().text =  $"<color={color}>"+ str + "</color>" + "\n" + GetTextField().text;
        }
      
    }
    public void Display2DecimalPlaces(float floatValue)
    {
        Display(floatValue, 2);
    }

    public void Display(float floatValue, int decimalPlaces)
    {
        float dec = 10 * decimalPlaces;
        floatValue = ((int)(floatValue * dec)) / dec;
        GetTextField().text = floatValue.ToString();
    }

    public void SetColor(Color newColor)
    {
        GetTextField().color = newColor;
    }
}