# MajsoulAI

不需要用户安装运行环境和编译的傻瓜式一键运行的AI，双击即可运行；  
AI 逻辑原作者[MahjongAI](https://github.com/zhangjk95/MahjongAI)；

```diff
- 目前该项目处于测试阶段, 部分代码可能不稳定或有逻辑错误
- 如果您发现了错误的地方或者逻辑错误欢迎提交 PR
- 使用该工具有封号风险请勿使用大号登录, 请使用小号或新注册账号进行使用
- 本项目无法直接使用, 请加群获取AuthServer后才能使用
```

[![VersionLatest](https://img.shields.io/github/release/moxcomic/MajsoulAI) ![DownloadsLatest](https://img.shields.io/github/downloads/moxcomic/MajsoulAI/latest/total)](https://github.com/moxcomic/MajsoulAI/releases/latest)

[Github下载](https://github.com/moxcomic/MajsoulAI/releases/latest)  
[网盘下载](https://www.aliyundrive.com/s/w1toPgKkGjw)

## 测试结果

(截至 2021 年 09 月 08 日)
![雀豪](./imgs/majsoul-5.png)
(截至 2021 年 09 月 08 日)
![雀杰三](./imgs/majsoul-4.png)
(截至 2021 年 09 月 07 日)
![雀杰二](./imgs/majsoul-3.png)
(截至 2021 年 09 月 01 日)
![一姬当千](./imgs/yijidangqian-0.PNG)
(截至 2021 年 08 月 15 日)
![雀杰](./imgs/majsoul-0.jpg
(截至 2021 年 08 月 30 日 | 无东场)
![雀杰](./imgs/majsoul-1.png)
(截至 2021 年 08 月 30 日 | 无东场)
![雀杰](./imgs/majsoul-2.png)

## 如何运行

```diff
- 请尽量使用 Cmd 或 Power Shell 运行
- 双击运行报错将会直接闪退无法截图错误信息
```

运行说明：

1. 首先运行一次 MahjongAI.exe (首次运行会直接报错退出)
2. 在目录下找到 config.json 打开进行配置

```JSON
{
   "Platform": 1, // 无需修改，默认位1
   "TenhouID": "", // 无需修改，默认为空
   "MajsoulRegion": 1, // 无需修改，无用值
   "MajsoulUsername": "", // 雀魂邮箱账号
   "MajsoulPassword": "", // 雀魂密码
   "PrivateRoom": 0, // 友人房间号，如果不加入友人房请置为 0
   "GameType": 1, // 此项无需修改，默认为1
   "Repeat": 1, // 需要打的场次，1 即为打一场，打完后会立即停止并退出账号
   "strategy": {
   "DefenceLevel": 3 // 此项无需修改，默认为3
   },
   "AuthServer": "", // 请从 雀魂 Ex 官方获取授权服务器地址填入
   "AccessToken": "", // 此项由系统自动生成，请勿填写

   // GameLevel:
   // - "Bronze": 铜之间 / 銅の間 / Bronze Room
   // - "Silver": 银之间 / 銀の間 / Silver Room
   // - "Gold": 金之间 / 金の間 / Gold Room
   // - "Jade": 玉之间 / 玉の間 / Jade Room
   // - "Throne": 王座之间 / 王座の間 / Throne Room
   "GameLevel": "Normal",

   // GameMode:
   // - "Normal": 喰アリ赤
   // - "Fast": 喰アリ赤速
   "GameMode": "Normal",

   // MatchMode:
   // - "East": 東風戦 / 四人东 / 四人東 / 4-Player East
   // - "EastSouth": 東南戦 / 四人南 / 四人南 / 4-Player South
   "MatchMode": "East",

   // DefenceLevel
   // - "3": (most nonaggressive, recommended) A comprehensive defense
   // strategy.
   // - "2": May defend when at least one player has called riichi.
   // - "1": May defend when the dealer (oya) has called riichi.
   // - "0": (most aggressive) No defense.
   "DefenceLevel": 3,
   "DeviceUuid": "" // 此项由系统自动生成，请勿填写
}
```

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
