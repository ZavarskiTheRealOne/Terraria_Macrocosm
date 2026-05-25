using Macrocosm.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Macrocosm.Content.Tiles.Furniture.Hevea;

public class HeveaBookcase : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileSolidTop[Type] = true;
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
        TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 18];
        TileObjectData.addTile(Type);
        // TODO: Uncomment when Hevea furniture items are added.
        // RegisterItemDrop(ModContent.ItemType<Items.Furniture.Hevea.HeveaBookcase>());

        HitSound = SoundID.Dig;
        DustType = ModContent.DustType<HeveaDust>();

        AddMapEntry(HeveaFurnitureUtils.MapColor, Language.GetText("ItemName.Bookcase"));
    }
}
