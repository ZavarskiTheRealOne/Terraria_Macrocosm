using System;
using Macrocosm.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Macrocosm.Content.Biomes;

public class PollutionVisualSystem : ModSystem
{
    private float visualPollutionLevel;

    private static readonly Color[] SkyPalette =
    [
        new(198, 239, 126),
        new(211, 211, 82),
        new(207, 166, 49),
        new(194, 45, 3),
        new(24, 0, 63)
    ];

    public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
    {
        if (Main.gameMenu || Main.LocalPlayer is null || !Main.LocalPlayer.active)
            return;

        bool pollutionBiomeActive = Main.LocalPlayer.InModBiome<PollutionBiome>();
        float targetPollutionLevel = pollutionBiomeActive ? TileCounts.Instance.PollutionLevel : 0f;
        visualPollutionLevel = MathHelper.Lerp(visualPollutionLevel, targetPollutionLevel, pollutionBiomeActive ? 0.025f : 0.04f);

        if (visualPollutionLevel < 0.01f)
            visualPollutionLevel = 0f;

        if (visualPollutionLevel <= 0f)
            return;

        float intensity = MathHelper.Clamp(visualPollutionLevel / TileCounts.PollutionLevelMax, 0f, 1f);
        Color skyTint = SamplePollutionPalette(SkyPalette, visualPollutionLevel);

        float vanillaBrightness = Math.Max(Math.Max(backgroundColor.R, backgroundColor.G), backgroundColor.B) / 255f;
        Color scaledTint = new Color(
            (byte)(skyTint.R * vanillaBrightness),
            (byte)(skyTint.G * vanillaBrightness),
            (byte)(skyTint.B * vanillaBrightness)
        );
        backgroundColor = Color.Lerp(backgroundColor, scaledTint, intensity);
        tileColor = Color.Lerp(tileColor, Color.Lerp(skyTint, Color.White, 0.35f), intensity * 0.35f);
    }

    public override void OnWorldUnload()
    {
        visualPollutionLevel = 0f;
    }

    private static Color SamplePollutionPalette(Color[] palette, float pollutionLevel)
    {
        float scaled = (pollutionLevel - TileCounts.PollutionLevelThreshold) / 100f;
        if (scaled <= 0f)
            return palette[0];

        if (scaled >= palette.Length - 1)
            return palette[^1];

        int index = (int)scaled;

        return Color.Lerp(palette[index], palette[index + 1], scaled - index);
    }
}
