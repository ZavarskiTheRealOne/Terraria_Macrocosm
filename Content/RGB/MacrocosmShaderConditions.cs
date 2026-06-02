using Macrocosm.Common.Subworlds;
using Macrocosm.Common.Systems;
using Macrocosm.Content.Subworlds;
using ReLogic.Peripherals.RGB;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        return SubworldSystem.IsActive<Moon>();
    }
}
public class PollutionShaderCondition : ChromaCondition
{
    public override bool IsActive()
    {
        return !SubworldSystem.AnyActive<Macrocosm>() && TileCounts.Instance.EnoughPollution;
    }
}
