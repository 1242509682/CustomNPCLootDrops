using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;

namespace LootControl
{
    [ApiVersion(2, 1)]
    public class LootControl : TerrariaPlugin
    {
        public override string Author => "Beerik 修改汉化：羽学";
        public override string Description => "这个插件允许用户控制所有NPC的战利品表";
        public override string Name => "自定义NPC掉落物（Loot Control）";
        public override Version Version => new Version(2, 0, 0, 0);

        private Config _config;
        private string _filePath = "";

        public LootControl(Main game) : base(game)
        {
            Order = 1;
        }

        public override void Initialize()
        {
            _filePath = Path.Combine(TShock.SavePath, "自定义NPC掉落物.json");
            _config = new Config();
            _config.ReadConfig(_filePath);
            ServerApi.Hooks.NpcKilled.Register(this, OnNPCKilled);
            ServerApi.Hooks.NpcLootDrop.Register(this, OnNPCLootDrop);
            GeneralHooks.ReloadEvent += OnReloadEvent;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.NpcKilled.Deregister(this, OnNPCKilled);
                ServerApi.Hooks.NpcLootDrop.Deregister(this, OnNPCLootDrop);
                GeneralHooks.ReloadEvent -= OnReloadEvent;
            }

            base.Dispose(disposing);
        }

        private void OnNPCKilled(NpcKilledEventArgs eventArgs)
        {
#if DEBUG
            Console.WriteLine("LootControl - OnNPCKilled: NPC ID: {0} - NPCArrayIndex: {1} - NPCPos: {2},{3}",
              eventArgs.npc.netID,
              eventArgs.npc.whoAmI, eventArgs.npc.position.X, eventArgs.npc.position.Y);
#endif

            LootTable outTable;
            Random random = new Random();
            if (_config.NPCLootTables.TryGetValue(eventArgs.npc.netID, out outTable))
            {
                if (Main.bloodMoon && outTable.自定义掉落表.ContainsKey(WorldState.Bloodmoon))
                {
                    foreach (Loot lootItem in outTable.自定义掉落表[WorldState.Bloodmoon])
                    {
                        DropLoot(lootItem, eventArgs, WorldState.Bloodmoon);
                    }
                }

                if (Main.eclipse && outTable.自定义掉落表.ContainsKey(WorldState.Eclipse))
                {
                    foreach (Loot lootItem in outTable.自定义掉落表[WorldState.Eclipse])
                    {
                        DropLoot(lootItem, eventArgs, WorldState.Eclipse);
                    }
                }

                if (Main.moonPhase == 0 && !Main.dayTime && outTable.自定义掉落表.ContainsKey(WorldState.Fullmoon))
                {
                    foreach (Loot lootItem in outTable.自定义掉落表[WorldState.Fullmoon])
                    {
                        DropLoot(lootItem, eventArgs, WorldState.Fullmoon);
                    }
                }

                if (!Main.dayTime && outTable.自定义掉落表.ContainsKey(WorldState.Night))
                {
                    foreach (Loot lootItem in outTable.自定义掉落表[WorldState.Night])
                    {
                        DropLoot(lootItem, eventArgs, WorldState.Night);
                    }
                }

                if (Main.dayTime && outTable.自定义掉落表.ContainsKey(WorldState.Day))
                {
                    foreach (Loot lootItem in outTable.自定义掉落表[WorldState.Day])
                    {
                        DropLoot(lootItem, eventArgs, WorldState.Day);
                    }
                }

                if (outTable.自定义掉落表.ContainsKey(WorldState.Normal))
                {
                    foreach (Loot lootItem in outTable.自定义掉落表[WorldState.Normal])
                    {
                        DropLoot(lootItem, eventArgs, WorldState.Normal);
                    }
                }
            }
        }

        // If successful returns stack size dropped else returns -1
        private void DropLoot(Loot pLootItem, NpcKilledEventArgs eventArgs, WorldState pState)
        {
#if DEBUG
            Console.WriteLine(
              "LootControl - OnNPCKilled: ExpertMode: {0} - MasterMode: {1} - LootClassic: {2} - LootExpert: {3} - LootMaster: {4}",
              Main.expertMode, Main.masterMode, pLootItem.普通难度, pLootItem.专家难度, pLootItem.大师难度);
#endif

            if (!Main.expertMode && !Main.masterMode && pLootItem.普通难度 ||
                Main.expertMode && !Main.masterMode && pLootItem.专家难度 ||
                Main.expertMode && Main.masterMode && pLootItem.大师难度)
            {
                Random random = new Random();
                double randomChance = random.NextDouble() * 100.0;
                if (pLootItem.掉落率 > randomChance)
                {
                    int stack = random.Next(pLootItem.最小数量, pLootItem.最大数量 + 1);
                    int newPrefix = (pLootItem.前缀ID < 0) ? random.Next(0, 84) : pLootItem.前缀ID;
                    Item.NewItem(eventArgs.npc.GetItemSource_Loot(), eventArgs.npc.position, eventArgs.npc.Size,
                      pLootItem.物品ID, stack, false, newPrefix);

#if DEBUG
                    Console.WriteLine(
                      "LootControl - OnNPCKilled: State: {0} - 物品ID: {1} - Prefix: {2} - Amount: {3} - (X: {4}, Y: {5})",
                      pState.ToString(), pLootItem.物品ID, newPrefix, stack, eventArgs.npc.position.X, eventArgs.npc.position.Y);
#endif
                }
            }
        }

        private void OnNPCLootDrop(NpcLootDropEventArgs eventArgs)
        {
            LootTable outTable;
            if (_config.NPCLootTables.TryGetValue(eventArgs.NpcId, out outTable))
            {
                if (!outTable.是否掉落默认物品)
                {
                    eventArgs.Handled = true;
                }
            }
        }

        private void OnReloadEvent(ReloadEventArgs eventArgs)
        {
            _config.ReadConfig(_filePath);
        }
    }
}