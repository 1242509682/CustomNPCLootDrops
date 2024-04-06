## 转载说明
作者源码地址：https://github.com/beerik94/LootControl

## 更新日志
```
2024年4月6日:对配置文件进行汉化，项目适配.net 6.0
```
## 汉化配置文件说明
位于`./tshock/自定义NPC掉落物.json`,若不存在则会自动创建。  

示例：  
```json
{
  "1": { //蓝史莱姆
    "自定义掉落表": {
      "Normal": [
        {
          "物品ID": 73, //金币
          "前缀ID": 0,
          "掉落率": 100.0,
          "最小数量": 1,
          "最大数量": 99,
          "普通难度": true,
          "专家难度": true,
          "大师难度": true
        }
      ]
    },
    "配置说明": "默认Normal 白天Day 晚上Night 日食Eclipse 满月Fullmoon 血月Bloodmoon",
    "是否掉落默认物品": false
  }
},
{
  "50": { //史莱姆王
    "自定义掉落表": {
      "Fullmoon": [ //血月条件
        {
          "物品ID": 2430, //粘鞍
          "前缀ID": 0,
          "掉落率": 100.0,
          "最小数量": 1,
          "最大数量": 1,
          "普通难度": true,
          "专家难度": true,
          "大师难度": true
        }
      ]
    },
    "配置说明": "默认Normal 白天Day 晚上Night 日食Eclipse 满月Fullmoon 血月Bloodmoon",
    "是否掉落默认物品": false
  }
},
```  
`"1"`：生物NPC的ID  
`"Normal"`：默认Normal 白天Day 晚上Night 日食Eclipse 满月Fullmoon 血月Bloodmoon  
