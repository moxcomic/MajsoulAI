#include "syanten.h"
#include <algorithm>

//コンストラクタ
Syanten::Syanten(){
	fuurosuu=0;

	//作業用
	int key;
	int pattern1_m;
	int pattern1_t;
	int pattern2_m;
	int pattern2_t;
	int index=1,indexloop=0;
	vector<int> tmp;

	//シャンテンファイル読み込み
	ifstream ifs("./syanten.dat");
	string str;
	if(ifs.fail()) {
		exit(0);
	}

	while(getline(ifs, str)) {
		sscanf(str.data(),"%d %d %d %d %d", &key,&pattern1_m,&pattern1_t,&pattern2_m,&pattern2_t);
		tmp.push_back(pattern1_m);
		tmp.push_back(pattern1_t);
		tmp.push_back(pattern2_m);
		tmp.push_back(pattern2_t);
		map_[key] = tmp;
		tmp.clear();
	}

}

//中身クリア
void Syanten::clear()
{
	for(int i=0;i<38;i++){
		tehai[i]=0;
	}
	tehai[38]=-1;
	fuurosuu=0;
}

//手牌セット
void Syanten::set_tehai(int t[])
{
	for(int i=0;i<38;i++){
		tehai[i]=t[i];
	}
	tehai[38]=-1;
}

//国士シャンテン
int Syanten::KokusiSyanten()
{
	int kokusi_toitu=0,syanten_kokusi=13,i;             
	//老頭牌
	for(i=1;i<30;i++){        
		if(i%10==1||i%10==9||i%20==1||i%20==9){	   
			if(tehai[i])
				syanten_kokusi--;
			if(tehai[i] >=2 && kokusi_toitu==0)
				kokusi_toitu=1;	
		}
	}             
	//字牌
	for(i=31;i<38;i++){      
		if(tehai[i]){	        
			syanten_kokusi--;
			if(tehai[i] >=2 && kokusi_toitu==0)
				kokusi_toitu=1;			            
		}
	}             
	//頭
	syanten_kokusi-= kokusi_toitu;             
	return syanten_kokusi;
}

//チートイシャンテン
int Syanten::TiitoituSyanten()
{
	int i=1,toitu=0,syurui=0,syanten_tiitoi;
	//トイツ数を数える
	for(;i<=37;i++){ 
		for(;!tehai[i];i++);
		if(i>=38) continue;
		syurui++;
		if(tehai[i] >=2)
		toitu++;
	}
	syanten_tiitoi=6-toitu;
	//４枚持ちを考慮
	if(syurui<7)
		syanten_tiitoi+= 7-syurui;
	return syanten_tiitoi;
}

//通常手シャンテン
int Syanten::NormalSyanten()
{
	int result=99;
	int tmpresult=0;
	for(int i=1;i<38;i++)
	{
		//頭抜き出し
		if(2 <= tehai[i])
		{            
			tehai[i] -= 2;
			tmpresult=checkNormalSyanten()-1;
			if(tmpresult < result){
				result=tmpresult;
			}
			tehai[i] += 2;
		}
	}

    tmpresult=checkNormalSyanten();   //頭無しと仮定して計算
	if(tmpresult < result){
		result=tmpresult;
	}

	return result;
}

int Syanten::AnySyanten(OUT int& normalSyanten) {
	int res = normalSyanten = NormalSyanten();
	if (fuurosuu == 0) {
		res = min(res, TiitoituSyanten());
		res = min(res, KokusiSyanten());
	}
	return res;
}

//Checkシャンテン
int Syanten::checkNormalSyanten()
{
	int key=0;
	int pt1m=0,pt1t=0;
	int pt2m=0,pt2t=0;
	int pt=0,ptm=0,ptt=0;
	vector<int> tmp;

	for(int j=0;j<3;j++){
		key=0;
		for(int i=1+j*10,offset=100000000;i<=9+j*10;i++,offset/=10){
			key+=tehai[i]*offset;
		}

		tmp=map_[key];

		pt1m=tmp[0];
		pt1t=tmp[1];

		pt2m=tmp[2];
		pt2t=tmp[3];

		if(pt1m*2+pt1t>=pt2m*2+pt2t){
			ptm+=pt1m;
			ptt+=pt1t;
		}
		else{
			ptm+=pt2m;
			ptt+=pt2t;
		}
	}

	for(int i=31;i<=37;i++){
		if(tehai[i]>=3){
			ptm++;
		}
		else if(tehai[i]>=2){
			ptt++;
		}
	}
	
	while (ptm + ptt > 4 - fuurosuu && ptt > 0) ptt--;
	while (ptm + ptt > 4 - fuurosuu) ptm--;
	
	return 8-ptm*2-ptt - fuurosuu * 2;
}
