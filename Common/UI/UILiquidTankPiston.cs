using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Macrocosm.Common.UI;

/// <summary>
/// Liquid-tank piston cell for engine UIs.
/// The owner sets <see cref="Phase"/> (0–1) and <see cref="Running"/> each frame;
/// the piston advances through its 9-frame sprite strip accordingly.
/// </summary>
public class UILiquidTankPiston : UILiquidTank
{
    private static Asset<Texture2D> pistonTex;

    private const int FrameCount = 9;
    private int pistonFrame = 0;

    /// <summary>Normalized engine phase [0, 1) </summary>
    public float Phase { get; set; } = 0f;
    public bool Running { get; set; } = false;

    public UILiquidTankPiston(int liquidType) : base(liquidType) { }

    public override void OnInitialize()
    {
        base.OnInitialize();
        Width = new(62, 0);
        Height = new(60, 0);
        Bubbles = false;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        pistonFrame = Running ? (int)(Phase * FrameCount) % FrameCount : 0;

        LiquidLevel = pistonFrame switch
        {
            0 => 0.85f,
            1 or 6 => 0.20f,
            2 or 3 or 4 or 5 => 0.10f,
            7 => 0.45f,
            8 => 0.55f,
            _ => 0f
        };
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        pistonTex ??= ModContent.Request<Texture2D>(Macrocosm.TexturesPath + "UI/Piston");

        CalculatedStyle dim = GetDimensions();
        spriteBatch.Draw(pistonTex.Value, dim.Position() + new Vector2(2, 2),
            pistonTex.Value.Frame(FrameCount, frameX: pistonFrame), Color.White);
    }
}
