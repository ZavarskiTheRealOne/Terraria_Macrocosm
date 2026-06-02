using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.RGB;

namespace Macrocosm.Content.RGB;
public class MoonSubworldShader(Color main1, Color main2) : ChromaShader
{
    private Color MainColor1 = main1;
    private Color MainColor2 = main2;
    private float TimeMultiplier;
    private float starVisibility;
    public override void Update(float elapsedTime)
    {
        //normally, Main.colorOfTheSkies would be used here: however, it does have a problem where theres a small period where its more purple ish, which doesnt fit in a place like the moon, which has no atmosphere. so this acts like a 
        if (Main.dayTime)
        {
            float num = (float)(Main.time / 54000.0);
            if (num < 0.25f)
            {
                TimeMultiplier = num / 0.25f;
                starVisibility = 1f - num / 0.25f;
            }
            else if (num > 0.75f)
            {
                TimeMultiplier = (1 - num) / 0.25f;
                starVisibility = (num - 0.75f) / 0.25f;
            }
            else
            {
                TimeMultiplier = 1;
            }
        }
        else
        {
            TimeMultiplier = 0;
            starVisibility = 1f;
        }
    }
    public override bool IsTransparentAt(EffectDetailLevel quality)
    {
        return false;
    }
    [RgbProcessor(EffectDetailLevel.Low)]
    private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
    {
        for (int i = 0; i < fragment.Count; i++)
        {
            Vector4 color = Color.Lerp(MainColor1, MainColor2, (float)Math.Sin(time * 0.5f + fragment.GetCanvasPositionOfIndex(i).X) * 0.5f + 0.5f).ToVector4();
            fragment.SetColor(i, color);
        }
    }
    [RgbProcessor(EffectDetailLevel.High)]
    private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
    {
        Vector4 mainColor1 = MainColor1.ToVector4() * TimeMultiplier;
        Vector4 mainColor2 = MainColor2.ToVector4() * TimeMultiplier;
        mainColor1.W = 255;
        mainColor2.W = 255;
        for (int i = 0; i < fragment.Count; i++)
        {
            Vector2 canvasPosition = fragment.GetCanvasPositionOfIndex(i);
            Point gridPosition = fragment.GetGridPositionOfIndex(i);
            float amount = (float)Math.Sin(canvasPosition.X * 1.5f + canvasPosition.Y + time) * 0.5f + 0.5f;
            Vector4 value3 = Vector4.Lerp(mainColor1, mainColor2, amount);
            float dynamicNoise = NoiseHelper.GetDynamicNoise(gridPosition.X, gridPosition.Y, time / 60f);
            dynamicNoise = Math.Max(0f, 1f - dynamicNoise * 20f);
            dynamicNoise *= 1f - TimeMultiplier;
            value3 = Vector4.Max(value3, new Vector4(dynamicNoise * starVisibility));
            fragment.SetColor(i, value3);
        }
    }
}
