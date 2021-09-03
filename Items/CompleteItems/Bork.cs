using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Bork : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blade of the Ruined King");
            Tooltip.SetDefault("5% increased melee and ranged damage" +
                "\n12% increased melee and ranged attack speed" +
                "\n+1 melee and ranged life steal"/* +
                "\n12% decreased maximum life" +
                "\n12% increased damage taken"*/);
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 50, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;

            Active = new Damnation(250, 60);
            Passives = new Passive[]
            {
                 new SoulTaint(2, 20, 50, this)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.05f;
            player.GetDamage(DamageClass.Ranged) += 0.05f;
            player.GetModPlayer<PLAYERGLOBAL>().lifeStealMelee += 1;//0.03;
            player.GetModPlayer<PLAYERGLOBAL>().lifeStealRange += 1;//0.03;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.12f;
            player.meleeSpeed += 0.12f;
            //player.GetModPlayer<PLAYERGLOBAL>().healthModifier -= 0.12;
            //player.GetModPlayer<PLAYERGLOBAL>().damageTakenModifier += 0.12;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Cutlass>(), 1)
            .AddIngredient(ItemType<RecurveBow>(), 1)
            .AddIngredient(ItemType<DamnedSoul>(), 50)
            .AddIngredient(ItemID.BrokenHeroSword, 1)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string GetStatText()
        {
            if (Active.currentlyActive)
            {
                if (Active.cooldownCount > 0)
                    return (Active.cooldownCount / 60).ToString();
            }
            return "";
        }

        public override bool OnCooldown(Player player)
        {
            if (Active.cooldownCount > 0 || !Active.currentlyActive)
                return true;
            else
                return false;
        }
    }
}
