# Mahjong AI

A Japanese Mahjong AI, which can play Mahjong on two online Japanese Mahjong services. It can easily defeat other players in 一般 (Normal level) games and also plays well in 上級 (High level) games.

## Supported platform

- Tenhou ([http://tenhou.net](http://tenhou.net))

```diff
- IMPORTANT: We no longer support Majsoul starting from 0.6.64.w due to abuse of this project.
```

## Results

### Tenhou

After 196 matches:

![_20180224162757](https://user-images.githubusercontent.com/4890396/36635239-b7660618-197f-11e8-939e-4dabd88c89d8.png)

### Majsoul

<img src="https://user-images.githubusercontent.com/4890396/52532016-61181280-2d72-11e9-99b5-c834470c2677.png" width="300"/>

## How to run

### Windows

#### Requirements

- Visual Studio **2017** or later
- Node.js v8.11.2 or later (required only if you are going to play on Majsoul)

#### Steps

1. Download source code.

		git clone https://github.com/zhangjk95/MahjongAI.git

2. Change the working directory to the source code folder.

		cd MahjongAI

2. If you are going to play on Majsoul, run

		npm install --prefix ./MajsoulHelper ./MajsoulHelper

2. Copy `MahjongAI\App.config.example` to `MahjongAI\App.config` and edit it. (Instructions are included in the example.)
3. Open the solution with Visual Studio.
4. Switch the Build Configuration to **Release**. (The program may be slow in Debug mode and exceed the time limit when making dicisions.)
5. Build and Run!

#### Replays

After the game ends, you can find the replay URLs in `MahjongAI\bin\Release\replays.txt`.

### Docker 

You can run this program in Docker on either Windows or Linux without ANY requirements other than Docker.

1. Download source code.

		git clone https://github.com/zhangjk95/MahjongAI.git
		
2. Change the working directory to the source code folder.

		cd MahjongAI

2. If you are going to play on Majsoul, run

		npm install --prefix ./MajsoulHelper ./MajsoulHelper

3. Copy `MahjongAI/App.config.example` to `MahjongAI/App.config` and edit it. (Instructions are included in the example.)

		cp MahjongAI/App.config.example MahjongAI/App.config
		$EDITOR MahjongAI/App.config

4. Build Docker image.

		./build.sh

5. Run.

		 docker run -it mahjong-ai
	
	> Note: The replay URLs are stored in `/usr/src/app/MahjongAI/bin/Release/replays.txt` inside the docker container. If you want to watch replays, You can map a file volume to it in order to save the replays outside the container. Which means, instead of running the command above, you may run the commands below instead:
	>
	>	  touch ~/replays.txt
	>	  docker run -v ~/replays.txt:/usr/src/app/MahjongAI/bin/Release/replays.txt -it mahjong-ai
	>
	> Then after the game ends, you can find the replay URLs in `~/replays.txt`.

## License

This program is under GPL. You can use this program at your OWN risk.

## Acknowledgements

The code calculating "syanten" and "score" are downloaded from [http://cmj3.web.fc2.com/](http://cmj3.web.fc2.com/).
