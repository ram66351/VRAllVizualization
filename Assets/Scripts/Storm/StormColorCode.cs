using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StormColorCode : MonoBehaviour
{
    public Material[] Mat;
    public Sprite[] sprites;
    public static StormColorCode Instance;

    void Awake()
    {
        Instance = this;
    }

    public StormColorCodeCategory GetColorCodeAndDescription(float wind)
    {
        StormColorCodeCategory stromColCat = new StormColorCodeCategory(wind);
        int index = (int)stromColCat.Category;
        stromColCat.Mat = Mat[index];
        stromColCat.sprite = sprites[index];
        return stromColCat;
    }

    public Color GetColorCode(float windSpeed)
    {
        StormColorCodeCategory.StromCategory Category;
        if (windSpeed <= 1)
        {
            Category = StormColorCodeCategory.StromCategory.Calm;
        }
        else if (windSpeed > 1 && windSpeed <= 5)
        {
            Category = StormColorCodeCategory.StromCategory.Light_air;
        }
        else if (windSpeed > 5 && windSpeed <= 11)
        {
            Category = StormColorCodeCategory.StromCategory.Light_breeze;
        }
        else if (windSpeed > 11 && windSpeed <= 19)
        {
            Category = StormColorCodeCategory.StromCategory.Gentle_breeze;
        }
        else if (windSpeed > 19 && windSpeed <= 28)
        {
            Category = StormColorCodeCategory.StromCategory.Moderate_breeze;
        }
        else if (windSpeed > 28 && windSpeed <= 38)
        {
            Category = StormColorCodeCategory.StromCategory.Fresh_breeze;
        }
        else if (windSpeed > 38 && windSpeed <= 49)
        {
            Category = StormColorCodeCategory.StromCategory.Strong_breeze;
        }
        else if (windSpeed > 49 && windSpeed <= 61)
        {
            Category = StormColorCodeCategory.StromCategory.Near_gale;
        }
        else if (windSpeed > 61 && windSpeed <= 74)
        {
            Category = StormColorCodeCategory.StromCategory.Gale;
        }
        else if (windSpeed > 74 && windSpeed <= 88)
        {
            Category = StormColorCodeCategory.StromCategory.Strong_gale;
        }
        else if (windSpeed > 88 && windSpeed <= 102)
        {
            Category = StormColorCodeCategory.StromCategory.Storm;
        }
        else if (windSpeed > 102 && windSpeed <= 117)
        {
            Category = StormColorCodeCategory.StromCategory.Violent_storm;
        }
        else
        {
            Category = StormColorCodeCategory.StromCategory.Hurricane; 
        }

        return Mat[(int)Category].GetColor("_TintColor"); 
    }
}

public class StormColorCodeCategory
{
    public enum StromCategory
    {
        Calm,
        Light_air,
        Light_breeze,
        Gentle_breeze,
        Moderate_breeze,
        Fresh_breeze,
        Strong_breeze,
        Near_gale,
        Gale,
        Strong_gale,
        Storm,
        Violent_storm,
        Hurricane
    }

    public StromCategory Category;
    public Material Mat;
    public Sprite sprite;
    public string Description;

    public StormColorCodeCategory(float windSpeed)
    {
        if (windSpeed <= 1)
        {
            Category = StromCategory.Calm;
            Description = "Calm; smoke rises vertically";
        }
        else if (windSpeed > 1 && windSpeed <= 5)
        {
            Category = StromCategory.Light_air;
            Description = "Direction of wind shown by \n smokedrift but not by wind vanes";
        }
        else if (windSpeed > 5 && windSpeed <= 11)
        {
            Category = StromCategory.Light_breeze;
            Description = " Wind felt on face; leaves rustle;\n ordinary vanes moved by wind";
        }
        else if (windSpeed > 11 && windSpeed <= 19)
        {
            Category = StromCategory.Gentle_breeze;
            Description = "Leaves and small twigs in constantmotion; \n wind extends light flag";
        }
        else if (windSpeed > 19 && windSpeed <= 28)
        {
            Category = StromCategory.Moderate_breeze;
            Description = "Raises dust and loose paper; \n small branches are moved";
        }
        else if (windSpeed > 28 && windSpeed <= 38)
        {
            Category = StromCategory.Fresh_breeze;
            Description = "Small trees in leaf begin to sway, \n crested wavelets form on inland waters";
        }
        else if (windSpeed > 38 && windSpeed <= 49)
        {
            Category = StromCategory.Strong_breeze;
            Description = "Large branches in motion; \n whistling heard in telegraph wires; \n umbrellas used with difficulty";
        }
        else if (windSpeed > 49 && windSpeed <= 61)
        {
            Category = StromCategory.Near_gale;
            Description = "Whole trees in motion; \n inconvenience felt when walking \n against the wind";
        }
        else if (windSpeed > 61 && windSpeed <= 74)
        {
            Category = StromCategory.Gale;
            Description = "Breaks twigs off trees; \n generally impedes progress";
        }
        else if (windSpeed > 74 && windSpeed <= 88)
        {
            Category = StromCategory.Strong_gale;
            Description = "Slight structural damage occurs \n (chimney-pots and slates removed)";
        }
        else if (windSpeed > 88 && windSpeed <= 102)
        {
            Category = StromCategory.Storm;
            Description = "Seldom experienced inland; \n trees uprooted; \n considerable structural damage occurs";
        }
        else if (windSpeed > 102 && windSpeed <= 117)
        {
            Category = StromCategory.Violent_storm;
            Description = "Very rarely experienced; \n accompanied by widespread damage";
        }
        else
        {
            Category = StromCategory.Hurricane;
            Description = "Very rarely experienced; \n accompanied by widespread damage";
        }


    }

}
