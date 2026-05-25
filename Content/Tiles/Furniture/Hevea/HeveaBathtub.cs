using Macrocosm.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Macrocosm.Content.Tiles.Furniture.Hevea;

public class HeveaBathtub : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;
        TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2);
        TileObjectData.newTile.Origin = new Point16(1, 1);
        TileObjectData.addTile(Type);
        // TODO: Uncomment when Hevea furniture items are added.
        // RegisterItemDrop(ModContent.ItemType<Items.Furniture.Hevea.HeveaBathtub>());

        AddMapEntry(HeveaFurnitureUtils.MapColor, Language.GetText("ItemName.Bathtub"));

        DustType = ModContent.DustType<HeveaDust>();
        AdjTiles = [TileID.Bathtubs];
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
}
