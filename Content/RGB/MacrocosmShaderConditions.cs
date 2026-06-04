using Macrocosm.Common.Events;
using Macrocosm.Common.Subworlds;
using Macrocosm.Common.Systems;
using Macrocosm.Content.Events;
using Macrocosm.Content.Subworlds;
using ReLogic.Peripherals.RGB;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Macrocosm.Content.RGB;
public class EarthOrbitShaderCondition : ChromaCondition
{
    public override bool IsActive()
    {
        return SubworldSystem.IsActive<EarthOrbitSubworld>();
    }
}
public class MoonOrbitShaderCondition : ChromaCondition
{
    public override bool IsActive()
    {
        return SubworldSystem.IsActive<MoonOrbitSubworld>();
    }
}
public class MoonShaderCondition : ChromaCondition
{
    public override bool IsActive()
    {
        return SubworldSystem.IsActive<Moon>() && (Main.LocalPlayer.ZoneOverworldHeight || Main.LocalPlayer.ZoneSkyHeight || Main.LocalPlayer.ZoneDirtLayerHeight);
    }
}
public class PollutionShaderCondition : ChromaCondition
{
    public override bool IsActive()
    {
        return !SubworldSystem.AnyActive<Macrocosm>() && TileCounts.Instance.EnoughPollution;
    }
}
public class MeteorShowerShaderCondition : ChromaCondition
{
    public override bool IsActive()
    {
        return MacrocosmEventSystem.IsActive<MeteorStormEvent>();
    }
}
public class MoonUndergroundCondition : ChromaCondition
{
    public override bool IsActive()
    {
        return SubworldSystem.IsActive<Moon>() && (Main.LocalPlayer.ZoneUnderworldHeight || Main.LocalPlayer.ZoneRockLayerHeight);
    }
}
