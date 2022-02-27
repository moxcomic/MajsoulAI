# MajsoulAI
[![VersionLatest](https://img.shields.io/github/release/moxcomic/MajsoulAI) ![DownloadsLatest](https://img.shields.io/github/downloads/moxcomic/MajsoulAI/latest/total)](https://github.com/moxcomic/MajsoulAI/releases/latest)  
```diff
- 由于雀魂不允许AI进入, 所以会封号, 请不要使用大号进行登录和使用
```
为了项目的可持续发展, 使用本项目需要赞助才可进行使用, 按照(30CNY/800JPY/7USD)7天/(100CNY/2500JPY/20USD)30天计算, 赞助请拉到最下方添加微信或者进Discord群询问.  
使用请先加群或[discord](https://discord.gg/eNKz25Xf3r)频道, 最新版本只要能跑Linux的地方都可以使用, 最低要求8核CPU 8G内存  
[## 如何运行 ##](#如何运行)

## 对于收费的一些说明
本项目最初版本`仅作为自动肝礼物的脚本发布且限定了只能进入修罗之战`, 本项目最早作为[Majsoul-Ex](https://github.com/moxcomic/majsoul-ex)的附属项目进行开发且`免费发布`(`MajsoulEx-Helper(该项目代码已删除)`), 后因为某游确实抠门导致发布后传播范超出了我们的预期结果并且因为最初免费发布`没有做任何限制的版本已经无法回收`导致后续发布的限制行为版本用户并不买账甚至一度向众多方寻求旧版本, 以及某宝和海鲜市场的参与, 于是我们放弃了该版本的维护与更新并重构了新的版本(`依然仅限于修罗之战`), 为了更好的区分和限制用户量和用户行为我们设置了服务器验证(该验证仅仅是`为了缩小扩散范围和用户量`), 所收取的费用是经过计算可以基本维持服务器自我运转的最小金额, 而实际结果也确实符合预期于是我们就停下了该项目的开发回去继续维护`Majsoul-Ex`项目了, 但由于某游在某一次大批量进行封号后的过河拆桥行为导致了我们决定重新构造一个新的方案并允许AI进入段位场, 对同伴出手的人是注定无法善终的, 目前我们已经构建了基于Web、移动端、Steam三端的方案, 并且已经着手进行更强力的新AI开发, 预计不久后本项目将会以新的姿态与大家见面.

## 测试结果
(截至 2021 年 10 月 14 日)
![雀圣3](./imgs/majsoul-7.png)
(截至 2021 年 09 月 09 日)
![雀豪](./imgs/majsoul-6.png)
(截至 2021 年 09 月 08 日)
![雀豪](./imgs/majsoul-5.png)
(截至 2021 年 09 月 08 日)
![雀杰三](./imgs/majsoul-4.png)
(截至 2021 年 09 月 07 日)
![雀杰二](./imgs/majsoul-3.png)
(截至 2021 年 09 月 01 日)
![一姬当千](./imgs/yijidangqian-0.PNG)
(截至 2021 年 08 月 15 日)
![雀杰](./imgs/majsoul-0.jpg)
(截至 2021 年 08 月 30 日 | 无东场)
![雀杰](./imgs/majsoul-1.png)
(截至 2021 年 08 月 30 日 | 无东场)
![雀杰](./imgs/majsoul-2.png)

## 如何运行
```
ubuntu镜像下载：magnet:?xt=urn:btih:9FC20B9E98EA98B4A35E6223041A5EF94EA27809&dn=ubuntu-20.04-desktop-amd64.iso&xl=2715254784

AI使用教程：
1. 使用前请先安装虚拟机（PD或者VM虚拟机均可）
2. 安装ubuntu或者debian之类apt体系的Linux系统（如果你需要可视化实时观战/手动打牌请安装有界面的版本）
3. 打开Linux里的 Terminal（终端）
4. 按照顺序执行以下命令, 一行为一条, 请一条一条执行不要全部复制一股脑粘贴进去, 输入时会让你输入密码（就是你创建的时候的密码）

#### 开始安装
1. 将ai.zip复制到虚拟机里解压
2. 右键ai文件夹Open in Terminal
3. sudo ./install
4. 输入你设置的Linux账户密码, 输入过程不可见你输入就行
5. 安装结束
6. ./kaguya
7. 根据提示获取UA填入对应区域
8. config.json在运行后会生成在ai目录下

#### 如何修改config
双击打开自己修改, 鼠标右键复制粘贴

请注意选择模式
auto:    无UI自动模式
calc:    手动打牌模式
display: 实时观战模式

请注意这里输入 0 1 2 数字, 否则会卡住
除auto以外两个模式均需要手动进行匹配
遇到填写Chrome路径和URL如果小白不知道请直接回车不用管
```

## 登录后命令
```
当前可用命令:
logout: 登出账号
join: 进入友人房, 例子: join 10086
ready: 友人房准备
auto: 自动匹配开始
info: 调试输出
exit: 打完当前对局后退出
```

## 配置微信推送对局结果
![](./imgs/push.PNG)
1、企业微信注册[https://work.weixin.qq.com/wework_admin/register_wx?from=loginpage](https://work.weixin.qq.com/wework_admin/register_wx?from=loginpage)

```diff
- 这里随便填写注册即可, 不需要验证
```
![](https://upload-images.jianshu.io/upload_images/22319199-f1aa61e705745597.png?imageMogr2/auto-orient/strip|imageView2/2/w/523)

2、企业微信登录

登录地址：[https://work.weixin.qq.com/wework_admin/loginpage_wx?from=myhome](https://work.weixin.qq.com/wework_admin/loginpage_wx?from=myhome)

使用管理员微信扫描二维码即可登录。

如果是刚注册的企业微信，则注册成功即为登录状态。
3、获取企业ID （corpid）

登录成功后，切换到“我的企业”，拉到最下面，找到企业ID，后面需要这个企业ID。
![](https://upload-images.jianshu.io/upload_images/22319199-7799de070beb1b28.png?imageMogr2/auto-orient/strip|imageView2/2/w/1107)
4、创建一个内置应用
![](https://upload-images.jianshu.io/upload_images/22319199-a7d0643e43911b94.png?imageMogr2/auto-orient/strip|imageView2/2/w/1096)
登录成功后，切换到“应用管理”，找到“自建”，然后点击“创建应用”。
![](https://upload-images.jianshu.io/upload_images/22319199-f68bb558850bfa32.png?imageMogr2/auto-orient/strip|imageView2/2/w/402)

选择logo，填写应用名称和简介，选择成员，点击“创建应用”。

此处只能选择已经申请加入到企业的成员，后面可以邀请加入企业。
5、获取创建应用的agentid和corpsecret

点击第4步中创建的应用，就可以看到agentid。

secret需要下载企业微信手机App后在网页点击查看secret后在手机端App查看。
6、将得到的内容输入config.json
```shell
// 在 /ai 目录下执行以下命令
vim config.json
// 然后将以上内容填入 qy_wechat 对应区域
// vim命令使用方式请参照百度
```

## Author

---

B 站 ID: [神崎·H·亚里亚](https://space.bilibili.com/898411/)  
B 站 ID: [关野萝可](https://space.bilibili.com/612462792/)  
QQ 交流群: [991568358](https://jq.qq.com/?_wv=1027&k=3gaKRwqg)  
Discord: [JoinDiscord](https://discord.gg/eNKz25Xf3r)

<figure class="third">
    <img src="./imgs/qrcode.JPG" width=170>
</figure>
