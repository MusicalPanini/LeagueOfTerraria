using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class RavenousHydra : MasterworkItem
    {
        public override string MasterworkName => "Leviathan";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ravenous Hydra");
            Tooltip.SetDefault("7% increased melee damage" +
                "\nIncreases life regeneration by 2" +
                "\n+1 melee life steal" +
                //"\n12% reduced maximum life" +
                //"\n12% increased damage taken" +
                "\nCan only have one CLEAVE item equiped at a time");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<TitanicHydra>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<TitanicHydra>()) != -1)
                return false;

            return base.CanEquipAccessory(player, slot, modded);
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 45, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;

            Passives = new Passive[]
            {
                new Cleave(50, CleaveType.Lifesteal, this)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.07f;
            player.lifeRegen += 2;

            player.GetModPlayer<PLAYERGLOBAL>().lifeStealMelee += 1;// 0.05;
           // player.GetModPlayer<PLAYERGLOBAL>().healthModifier -= 0.12;
           // player.GetModPlayer<PLAYERGLOBAL>().damageTakenModifier += 0.12;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemType<Tiamat>(), 1)
            .AddIngredient(ItemType<VampiricScepter>(), 1)
            .AddIngredient(ItemType<Pickaxe>(), 1)
            .AddIngredient(ItemID.Gungnir, 1)
            .AddIngredient(ItemType<DarksteelBar>(), 10)
            .AddIngredient(ItemID.ChlorophyteBar, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            
        }

        public override string MasterworkTooltip()
        {
            return LeagueTooltip.CreateColorString(MasterColor, "10%") + " increased melee damage" +
                "\nIncreases life regeneration by " + LeagueTooltip.CreateColorString(MasterColor, "3") +
                "\n+" + LeagueTooltip.CreateColorString(MasterColor, "2") + " melee life steal" +
                "\nCan only have one CLEAVE item equiped at a time";
        }

        public override void UpdateMasterwork(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.03f;
            player.lifeRegen += 1;
            player.GetModPlayer<PLAYERGLOBAL>().lifeStealMelee += 1;
        }
    }
}
