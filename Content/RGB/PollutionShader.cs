using Macrocosm.Common.Systems;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.RGB;

namespace Macrocosm.Content.RGB;
public class PollutionShader(Color[] skyList, float panSpeedX, float panSpeedY) : ChromaShader
{
    private readonly Color[] skyList = skyList;
    private readonly float timeScaleX = panSpeedX;
    private readonly float timeScaleY = panSpeedY;
    public override bool IsTransparentAt(EffectDetailLevel quality)
    {
        return true;
    }
    public override void Process(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
    {
        if (quality == EffectDetailLevel.Low) 
        {
            time *= 0.25f;
        }
        for (int i = 0; i < fragment.Count; i++) 
        {
            float scale = (TileCounts.Instance.PollutionLevel - TileCounts.PollutionLevelThreshold) / 100f;
            
            int colorIndex = (int)scale;
            if (scale <= 0)
            {
                return;
            }
            Color fogColor = scale >= skyList.Length -1 ? skyList[^1] : skyList[colorIndex];
            Color fogBackColor = skyList[colorIndex] * 0.40f;
            float staticNoise = NoiseHelper.GetStaticNoise(fragment.GetCanvasPositionOfIndex(i) * new Vector2(0.2f, 0.4f) + new Vector2(time * this.timeScaleX, time * this.timeScaleY));
            Vector4 color = Vector4.Lerp(fogBackColor.ToVector4(), fogColor.ToVector4(), staticNoise * staticNoise);
            color.W *= (colorIndex * 0.15f + 0.3f);
            fragment.SetColor(i, color);
        }
    }
}
