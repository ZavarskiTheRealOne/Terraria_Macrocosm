using Macrocosm.Common.Netcode;
using Macrocosm.Common.Systems;
using Macrocosm.Content.RGB;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Peripherals.RGB;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.RGB;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace Macrocosm;

public class Macrocosm : Mod
{
    public static Mod Instance => ModContent.GetInstance<Macrocosm>();

    public const string TexturesPath = "Macrocosm/Assets/Textures/";
    public const string FancyTexturesPath = TexturesPath + "LowRes/";
    public const string FancyHighResTexturesPath = TexturesPath + "HighRes/";
    public const string EmptyTexPath = TexturesPath + "Empty";
    public const string UITexturesPath = TexturesPath + "UI/";
    public const string UISymbolsPath = UITexturesPath + "Symbols/";
    public const string UIButtonsPath = UITexturesPath + "Buttons/";

    public const string ShadersPath = "Macrocosm/Assets/Effects/";
    public const string MusicPath = "Macrocosm/Assets/Music/";
    public const string SFXPath = "Macrocosm/Assets/Sounds/SFX/";

    public const int ItemShoot_UsesAmmo = 10;

    public static Asset<Texture2D> EmptyTex { get; set; }
    public static Type[] GetTypes() => AssemblyManager.GetLoadableTypes(Instance.Code);
    public static Effect GetShader(string name) => GameShaders.Misc[$"{Instance.Name}:{name}"].Shader;

    private static ChromaShader earthOrbitShader = new OrbitShader(new Color(116, 133, 35), new Color(30, 72, 159));
    private static ChromaShader moonOrbitShader = new OrbitShader(new Color(177, 177, 177), new Color(33, 33, 33));
    private static ChromaShader moonShader = new MoonSubworldShader(new Color(177, 177, 177), new Color(33, 33, 33));
    private static ChromaShader pollutionShader = new PollutionShader(new Color[]
    {
        new(198, 239, 126),
        new(211, 211, 82),
        new(207, 166, 49),
        new(194, 45, 3),
        new(24, 0, 63)
    }, 0, 0.15f);

    public override void Load()
    {
        CurrencySystem.Load();

        if (!Main.dedServ)
        {
            EmptyTex = ModContent.Request<Texture2D>(EmptyTexPath);
            LoadResprites();
            LoadEffects();
            LoadRGB();
        }
    }

    public override void Unload()
    {
        UnloadResprites();
        UnloadEffects();
        UnloadRGB();
    }

    public override void PostSetupContent()
    {
    }

    private static void LoadResprites()
    {
        string respritePath = TexturesPath + "Resprites/";
        TextureAssets.Moon[0] = ModContent.Request<Texture2D>(respritePath + "Moon_0");
    }

    private static void UnloadResprites()
    {
        TextureAssets.Moon[0] = Main.Assets.Request<Texture2D>("Images/Moon_0", AssetRequestMode.ImmediateLoad);
    }

    private static void LoadEffects()
    {
        AssetRequestMode mode = AssetRequestMode.ImmediateLoad;

        // Load misc shaders
        foreach (var fullPath in Instance.RootContentSource.GetAllAssetsStartingWith("Assets/Effects/"))
        {
            string path = Path.ChangeExtension(fullPath, null);
            string name = Path.GetFileName(path);
            GameShaders.Misc[$"{Instance.Name}:{name}"] = new MiscShaderData(Instance.Assets.Request<Effect>(path, AssetRequestMode.ImmediateLoad), name);
        }

        // Load filters
        Filters.Scene["Macrocosm:RadiationNoise"] = new Filter(new ScreenShaderData(ModContent.Request<Effect>(ShadersPath + "RadiationNoise", mode), "RadiationNoise"));
        Filters.Scene["Macrocosm:RadiationNoise"].Load();

        Filters.Scene["Macrocosm:Shockwave"] = new Filter(new ScreenShaderData(ModContent.Request<Effect>(ShadersPath + "Shockwave", mode), "Shockwave"));
        Filters.Scene["Macrocosm:Shockwave"].Load();

        // Vanilla shader clones
        Filters.Scene["Macrocosm:FilterMoonLordShake"] = new Filter(new MoonLordScreenShaderData("FilterMoonLordShake", aimAtPlayer: false), EffectPriority.VeryHigh);
        Filters.Scene["Macrocosm:FilterMoonLordShake"].Load();

        Filters.Scene["Macrocosm:Graveyard"] = new Filter(new ScreenShaderData("FilterGraveyard"), EffectPriority.Medium);
        Filters.Scene["Macrocosm:Graveyard"].Load();
    }

    private static void UnloadEffects()
    {
        // What goes here?
    }
    private static void LoadRGB()
    {
        Main.Chroma.RegisterShader(earthOrbitShader, new EarthOrbitShaderCondition(), ShaderLayer.Biome);
        Main.Chroma.RegisterShader(moonOrbitShader, new MoonOrbitShaderCondition(), ShaderLayer.Biome);
        Main.Chroma.RegisterShader(moonShader, new MoonShaderCondition(), ShaderLayer.Biome);
        Main.Chroma.RegisterShader(pollutionShader, new PollutionShaderCondition(), ShaderLayer.BiomeModifier);
    }

    private static void UnloadRGB()
    {
        Main.Chroma.UnregisterShader(earthOrbitShader);
        Main.Chroma.UnregisterShader(moonOrbitShader);
        Main.Chroma.UnregisterShader(moonShader);
        Main.Chroma.UnregisterShader(pollutionShader);
    }
    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        PacketHandler.HandlePacket(reader, whoAmI);
    }
}