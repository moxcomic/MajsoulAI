# MajsoulAI

基于 Majsoul Ex SDK 的雀魂麻将 AI，AI 逻辑原作者[MahjongAI](https://github.com/zhangjk95/MahjongAI)。

## History

[可能封号] [已停止更新] [0.x]: `MajsoulEx Helper`  
[可能封号] [已停止更新] [1.0]：[修罗之战挂机程序](https://github.com/moxcomic/no-asura-no)

## 2.0

!!! 请尽量使用 Cmd 或 Power Shell 运行 !!!
!!! 双击运行报错将会直接闪退无法截图错误信息 !!!

运行说明：

1. 首先运行一次 MahjongAI.exe (首次运行会直接报错退出)
2. 在目录下找到 config.json 打开进行配置
   {
   // 此项无需修改
   "Platform": 1,
   // 此项无需修改
   "TenhouID": "",
   // 此项无需修改
   "MajsoulRegion": 1,
   // 雀魂邮箱账号
   "MajsoulUsername": "",
   // 雀魂密码
   "MajsoulPassword": "",
   // 友人房间号，如果不加入友人房请置为 0
   "PrivateRoom": 0,
   // 此项无需修改
   "GameType": 1,
   // 需要打的场次，1 即为打一场，打完后会立即停止并退出账号
   "Repeat": 1,
   // 此项无需修改
   "strategy": {
   "DefenceLevel": 3
   },
   // 请从 雀魂 Ex 官方获取授权服务器地址填入
   "AuthServer": "",
   // 此项无需修改
   "AccessToken": "",
   // - "Bronze": 铜之间 / 銅の間 / Bronze Room
   // - "Silver": 银之间 / 銀の間 / Silver Room
   // - "Gold": 金之间 / 金の間 / Gold Room
   // - "Jade": 玉之间 / 玉の間 / Jade Room
   // - "Throne": 王座之间 / 王座の間 / Throne Room
   "GameLevel": "Normal",
   // - "Normal": 喰アリ赤
   // - "Fast": 喰アリ赤速
   "GameMode": "Normal",
   // - "East": 東風戦 / 四人东 / 四人東 / 4-Player East
   // - "EastSouth": 東南戦 / 四人南 / 四人南 / 4-Player South
   "MatchMode": "East",
   // - "3": (most nonaggressive, recommended) A comprehensive defense
   strategy.
   // - "2": May defend when at least one player has called riichi.
   // - "1": May defend when the dealer (oya) has called riichi.
   // - "0": (most aggressive) No defense.
   "DefenceLevel": 3,
   // 此项无需修改
   "DeviceUuid": ""
   }
3. 完成配置后再次启动 MahjongAI.exe
4. 看到 LoginSuccess 表示登录成功（其他报错情况请截图报错信息反馈
5. 如果要进友人房请先输入 j 然后输入 s （配置里友人房间号必须为 0，如果打匹配直接输入 s

可用命令:
q: 无论设置了打多少场在当前对局结束后立即结束并退出账号
j: 加入友人房 (如果未设置房间号会报错)
s: 开始匹配 (如果加入友人房则会变为准备)
g: 退出当前账号 (请勿在对局进行时调用, 该命令会立即退出账号和程序, 例如登录后不想匹配则需要先输入 g 再关闭程序而不是直接关闭否则你的账号依然会在线)

## Author

---

B 站 ID: [神崎·H·亚里亚](https://space.bilibili.com/898411/)  
B 站 ID: [关野萝可](https://space.bilibili.com/612462792/)  
QQ 交流群: [991568358](https://jq.qq.com/?_wv=1027&k=3gaKRwqg)

请作者喝一杯咖啡

<figure class="third">
    <img src="https://moxcomic.github.io/wechat.png" width=170>
    <img src="https://moxcomic.github.io/alipay.png" width=170>
    <img src="https://moxcomic.github.io/qq.png" width=170>
</figure>