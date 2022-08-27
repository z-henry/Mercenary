# Mercenary

### 免责声明

使用辅助进行游戏的行为违反了暴雪用户协议 1.C.ii.2 使用机器人程序软件（BOT）： 任何未经暴雪和/或运营方明确授权，允许自动控制游戏，服务器和/或任何组件或其功能的的代码或/和软件，如自动操控游戏中的角色；因此根据协议，暴雪和/或运营方可以暂停、撤销或终止您使用本平台或本平台的部分功能或组成部分的许可。 本项目仅用作C#的学习研究，不可用于任何盈利活着非法用途，请你于下载之后的24小时内删除本项目。由于使用该项目导致账号被封禁，本人不承担任何责任。如果该插件的开源分享行为损害了暴雪/运营方的利益，请联系我停止更新/删库，我会全力配合。

### 说明

1. 需要配合HsMod(author:Pik-4)使用，HsMod配置如下
|  key   | value  |
|  ----  | ----  |
| 自动开盒  | true |
| 结算展示  | false |
| 应用焦点  | false |
| 报错退出  | true |
| 弹出消息  | false |
| 自动领奖  | true |
| HsMod状态  | true |
2. **[推荐方式]** 插件会识别炉石程序传入的`hsunitid`参数，作为路径的一部分
```Hearthstone.exe hsunitid:your_hsunitid```
配置文件`Hearthstone\BepInEx\config\your_hsunitid\io.github.jimowushuang.hs.cfg`
日志文件`Hearthstone\BepInEx\Log\your_hsunitid\`
3. 若启动炉石没有传递`hsunitid`参数（战网启动等），路径会发生变化
配置文件`Hearthstone\BepInEx\config\io.github.jimowushuang.hs.cfg`
日志文件`Hearthstone\BepInEx\Log\`

### 已知问题
1. 与HsMod联动的战斗过程中开启齿轮功能，尚未测试
2. 与HsMod联动的战斗记录功能还未进行多账户优化
