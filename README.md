# MajsoulAI
[![VersionLatest](https://img.shields.io/github/release/moxcomic/MajsoulAI) ![DownloadsLatest](https://img.shields.io/github/downloads/moxcomic/MajsoulAI/latest/total)](https://github.com/moxcomic/MajsoulAI/releases/latest)

使用请先加群或Discord频道, 最新版本已不在github发布. 最新版本再docker hub发布, github下载版本已停用.

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
1. 安装docker
2. 使用cmd或powershell执行以下命令(注意空格, 复制时不要漏掉空格否则无法执行)
```shell
docker pull moxcomic/mjai
docker run -itd --name ai moxcomic/mjai /bin/bash
docker ps
    这里会得到如下输出
    CONTAINER ID   IMAGE          COMMAND       CREATED       STATUS      PORTS     NAMES
    2b9c0ff81e96   6a7ed490783d   "/bin/bash"   2 weeks ago   Up 6 days             ai
docker exec -it [CONTAINER ID] /bin/bash
    这里的[CONTAINER ID]需要进行替换
    例如这里替换后的命令为: docker exec -it 2b9c0ff81e96 /bin/bash
cd ai
./mjai
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

请作者喝一杯咖啡

<figure class="third">
    <img src="https://moxcomic.github.io/wechat.png" width=170>
    <img src="https://moxcomic.github.io/alipay.png" width=170>
    <img src="https://moxcomic.github.io/qq.png" width=170>
</figure>
