docker run --rm -v "$PWD/MahjongLib":/usr/src/myapp -w /usr/src/myapp gcc:4.9 bash -c "g++ *.cpp -o MahjongLib -O3" 
docker build -t mahjong-ai .
