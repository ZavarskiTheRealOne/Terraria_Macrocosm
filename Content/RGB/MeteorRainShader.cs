using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using ReLogic.Peripherals.RGB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.RGB;

namespace Macrocosm.Content.RGB;
public class MeteorRainShader(Color meteorColour) : ChromaShader
{
    private Color meteorColor = meteorColour;
    public override bool IsTransparentAt(EffectDetailLevel quality)
    {
        return true;
    }
    public override void Process(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
    {
        Vector4 colour = meteorColor.ToVector4();
        Vector4 fadeVector = new Vector4(0, 0, 0, 0.25f);
        for (int i = 0; i < fragment.Count; i++)
        {
            Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
            Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
            float num = (NoiseHelper.GetStaticNoise(gridPositionOfIndex.X) * 10f + time) % 10f - canvasPositionOfIndex.Y;
            Vector4 vector2 = fadeVector;
            if (num > 0f)
            {
                float amount = Math.Max(0f, 1.2f - num);
                if (num < 0.2f)
                {
                    amount = num * 5f;
                }
                vector2 = Vector4.Lerp(vector2, colour, amount);
            }
            fragment.SetColor(i, vector2);
        }
    }
}
