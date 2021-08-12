FROM mono:5.16.0
RUN curl -sL https://deb.nodesource.com/setup_11.x | bash - \
    && apt-get install -y nodejs
COPY . /usr/src/app
RUN cd /usr/src/app/MajsoulHelper && npm install
RUN cd /usr/src/app && nuget restore MahjongAI.sln && msbuild MahjongAI.sln /t:Build /p:Configuration=Release
CMD cd /usr/src/app/MahjongAI/bin/Release && mono ./MahjongAI.exe | grep -v Information
