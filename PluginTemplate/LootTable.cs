namespace LootControl
{
    public enum WorldState
    {
        Normal,
        Day,
        Night,
        Eclipse,
        Fullmoon,
        Bloodmoon
    }

    public class LootTable
    {
        public Dictionary<WorldState, List<Loot>> 自定义掉落表;
        public string 配置说明 = "默认Normal 白天Day 晚上Night 日食Eclipse 满月Fullmoon 血月Bloodmoon";
        public bool 是否掉落默认物品 = false;

        public LootTable()
        {
            自定义掉落表 = new Dictionary<WorldState, List<Loot>>();
        }
    }

    public class Loot
    {
        public int 物品ID;
        public int 前缀ID;
        public double 掉落率 = 100;
        public int 最小数量 = 0;
        public int 最大数量 = 1;
        public bool 普通难度 = true;
        public bool 专家难度 = false;
        public bool 大师难度 = false;

        public Loot(int p物品ID, double p掉落率, int p最小数量, int p最大数量, int p前缀ID = 0, bool p普通难度 = true, bool p专家难度 = true, bool p大师难度 = true)
        {
            物品ID = p物品ID;
            前缀ID = p前缀ID;
            掉落率 = p掉落率;
            最小数量 = p最小数量;
            最大数量 = p最大数量;
            普通难度 = p普通难度;
            专家难度 = p专家难度;
            大师难度 = p大师难度;
        }
    }
}