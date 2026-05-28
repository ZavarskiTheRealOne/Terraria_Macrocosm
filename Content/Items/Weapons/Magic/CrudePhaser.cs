using Macrocosm.Content.Projectiles.Friendly.Magic;
using Macrocosm.Content.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Macrocosm.Content.Items.Weapons.Magic;

public class CrudePhaser : ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 105;
        Item.DamageType = DamageClass.Magic;
        Item.mana = 8;
        Item.width = 48;
        Item.height = 26;
        Item.useTime = 34;
        Item.useAnimation = 34;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.noUseGraphic = true;
        Item.noMelee = true;
        Item.knockBack = 2.5f;
        Item.value = 10000;
        Item.rare = ModContent.RarityType<MoonRarity1>();
        Item.UseSound = null;
        Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<CrudePhaserHeld>();
        Item.shootSpeed = 13f;
        Item.reuseDelay = 10;
    }

    public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
        return false;
    }
}
