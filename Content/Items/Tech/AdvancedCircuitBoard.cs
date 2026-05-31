using Macrocosm.Content.Items.Ores;
using Macrocosm.Content.Items.Refined;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Macrocosm.Content.Items.Tech;

public class AdvancedCircuitBoard : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 5;
    }

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Tech.PrintedCircuitBoard>(), 1);
        Item.width = 20;
        Item.height = 20;
        Item.value = 100;
        Item.rare = ItemRarityID.Purple;
    }

    public override void AddRecipes()
    {
        
        CreateRecipe()
          .AddIngredient<Silicon>(9)
          .AddIngredient<Plastic>(5)
          .AddIngredient(ItemID.Wire, 15)
          .AddIngredient(ItemID.GoldBar, 1)
          .AddTile<Tiles.Crafting.Fabricator>()
          .Register();
        
    }
}
