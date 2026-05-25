using Macrocosm.Common.UI.Themes;
using Macrocosm.Content.Machines.Consumers.Drills;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Macrocosm.Common.UI.Machines;

/// <summary>
/// Displays a grid snapshot of the tiles sampled beneath a drill/excavator.
/// <list type="bullet">
///   <item>Air tile (id == 0) dimmed empty slot</item>
///   <item>Solid non-drillable (id == -1)  mid-brightness empty slot</item>
///   <item>Drillable (id 0)  — full-brightness slot with item sprite</item>
/// </list>
/// </summary>
public class UITileSampleGrid : UIElement
{
    private const float SlotScale = 1.0f;
    private const int CellPixels = 26;
    private const int CellSpacing = 2;

    private static Asset<Texture2D> slotTex;
    private static Asset<Texture2D> slotBorderTex;

    public BaseDrillTE Drill { get; set; }

    public UITileSampleGrid(BaseDrillTE drill)
    {
        Drill = drill;
        RefreshSize();
    }

    private void RefreshSize()
    {
        int cols = Drill?.SampleGridWidth ?? 1;
        int rows = Drill?.SampleGridHeight ?? 1;
        Width.Set(cols * CellPixels + (cols - 1) * CellSpacing, 0f);
        Height.Set(rows * CellPixels + (rows - 1) * CellSpacing, 0f);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        RefreshSize();
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        if (Drill is null)
            return;

        slotTex ??= ModContent.Request<Texture2D>("Macrocosm/Assets/Textures/UI/SmallInventorySlot", AssetRequestMode.ImmediateLoad);
        slotBorderTex ??= ModContent.Request<Texture2D>("Macrocosm/Assets/Textures/UI/SmallInventorySlotBorder", AssetRequestMode.ImmediateLoad);

        int cols = Drill.SampleGridWidth;
        int rows = Drill.SampleGridHeight;
        int[] sample = Drill.SampledItems;

        CalculatedStyle dims = GetDimensions();
        float ox = dims.X;
        float oy = dims.Y;

        Color bgFull = UITheme.Current.InventorySlotStyle.BackgroundColor;
        Color borderFull = UITheme.Current.InventorySlotStyle.BorderColor;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int idx = row * cols + col;
                int id = idx < sample.Length ? sample[idx] : 0;

                Vector2 pos = new(ox + col * (CellPixels + CellSpacing), oy + row * (CellPixels + CellSpacing));

                Color bgColor, borderColor;
                if (id == 0)
                {
                    bgColor = bgFull * 0.20f;
                    borderColor = borderFull * 0.50f;
                }
                else if (id < 0)
                {
                    bgColor = Color.Lerp(bgFull, borderFull, 0.55f);
                    borderColor = borderFull;
                }
                else
                {
                    bgColor = bgFull;
                    borderColor = borderFull;
                }

                spriteBatch.Draw(slotTex.Value, pos, null, bgColor, 0f, Vector2.Zero, SlotScale, SpriteEffects.None, 0f);
                spriteBatch.Draw(slotBorderTex.Value, pos, null, borderColor, 0f, Vector2.Zero, SlotScale, SpriteEffects.None, 0f);

                if (id > 0)
                {
                    Item item = new(id);
                    Vector2 center = pos + new Vector2(CellPixels * 0.5f);
                    ItemSlot.DrawItemIcon(
                        screenPositionForItemCenter: center,
                        item: item,
                        context: 31,
                        spriteBatch: spriteBatch,
                        scale: item.scale,
                        sizeLimit: CellPixels - 8,
                        environmentColor: Color.White
                    );
                }
            }
        }
    }
}
