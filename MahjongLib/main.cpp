#include <cstdlib>
#include <iostream>
#include "common.h"
#include "syanten.h"
#include "mjscore.h"
using namespace std;

const int typeCount = 38;
const int typeCount2 = 41;
const int doraCount = 10;

Syanten syanten;
MJScore mjscore;
int hand[typeCount];
int hand2[typeCount2];
int fuuro[20];
int dora[doraCount + 1];

void handleSyanten() {
	memset(hand, 0, sizeof(hand));
	syanten.clear();

	for (int i = 0; i < typeCount; i++) {
		cin >> hand[i];
	}
	syanten.set_tehai(hand);

	int fuurosuu;
	cin >> fuurosuu;
	syanten.set_fuurosuu(fuurosuu);

	int resNormal;
	int res = syanten.AnySyanten(OUT resNormal);
	cout << res << " " << resNormal << endl;
}

int parseFuuroType(const string& fuuroType) {
	if (fuuroType == "chii") {
		return MJScore::CHII;
	}
	else if (fuuroType == "pon") {
		return MJScore::PON;
	}
	else if (fuuroType == "minkan") {
		return MJScore::MINKAN;
	}
	else if (fuuroType == "ankan") {
		return MJScore::ANKAN;
	}
	else if (fuuroType == "kakan") {
		return MJScore::KAKAN;
	}
	else if (fuuroType == "aka_chii") {
		return MJScore::AKA_CHII;
	}
	else if (fuuroType == "aka_pon1") {
		return MJScore::AKA_PON1;
	}
}

void handleMjscore() {
	mjscore.Clear_WithoutRule();
	memset(hand2, 0, sizeof(hand2));
	memset(fuuro, 0, sizeof(fuuro));
	memset(dora, 0, sizeof(dora));

	for (int i = 0; i < typeCount2; i++) {
		cin >> hand2[i];
	}
	mjscore.Set_Tehai(hand2);

	int lastTile;
	cin >> lastTile;
	mjscore.Set_Agarihai(lastTile);

	int direction;
	cin >> direction;
	mjscore.Set_Bakaze(direction);
	cin >> direction;
	mjscore.Set_Jikaze(direction);

	int fuuroCount;
	cin >> fuuroCount;
	for (int i = 0; i < fuuroCount; i++) {
		string fuuroType;
		cin >> fuuroType;
		fuuro[i * 4] = parseFuuroType(fuuroType);
		cin >> fuuro[i * 4 + 1];
	}
	mjscore.Set_Fuuro(fuuro);

	for (int i = 1; i <= doraCount; i++) {
		cin >> dora[i];
	}
	mjscore.Set_Dora(dora);

	int isRiichi;
	cin >> isRiichi;
	mjscore.Is_Riichi(isRiichi);

	int tsumoAgari;
	cin >> tsumoAgari;
	mjscore.Set_Tumoagari(tsumoAgari);

	int isLastTile;
	cin >> isLastTile;
	mjscore.Is_Haitei(isLastTile);

	mjscore.Run();
	cout << 
		(mjscore.Get_ErrorCode() != 0 ? 0 :
			direction == 31 && tsumoAgari ? mjscore.Get_OyaTumo() * 3 :
			direction == 31 ? mjscore.Get_OyaRon() :
			tsumoAgari ? mjscore.Get_KoTumoKo() * 2 + mjscore.Get_KoTumoOya() :
			mjscore.Get_KoRon()) << 
		endl;
}

void initMjscore() {
	//ルール入力
	mjscore.Avail_Akahai(true);//赤牌有り
	mjscore.Avail_Kuitan(true);//クイタン有り
	mjscore.Avail_ManganKiriage(true);//満貫切り上げ有り	
	mjscore.Avail_Ba1500(false);//場千五有り
	mjscore.Avail_DoubleKokushi13(false);//国士13面待ちダブル役満有り
	mjscore.Avail_DoubleTyuuren9(false);//九連宝燈9面待ちダブル役満有り
	mjscore.Avail_DoubleDaisuusii(false);//大四喜ダブル役満有り
	mjscore.Avail_DoubleSuttan(false);//四暗刻単騎ダブル役満有り

	//特殊役成立フラグ入力	
	mjscore.Is_Riichi(false);//リーチ
	mjscore.Is_Tenhou(false);//天和	
	mjscore.Is_Tiihou(false);//地和	
	mjscore.Is_DoubleRiichi(false);//ダブルリーチ	
	mjscore.Is_Ippatu(false);//一発	
	mjscore.Is_Tyankan(false);//チャンカン	
	mjscore.Is_Rinsyan(false);//リンシャン	
	mjscore.Is_NagashiMangan(false);//流し満貫
	mjscore.Is_Haitei(false);//ハイテイ
}

int main() {
	ios_base::sync_with_stdio(false);
	initMjscore();
	string input;
	while (cin >> input) {
		if (input == "syanten") {
			handleSyanten();
		}
		else if (input == "mjscore") {
			handleMjscore();
		}
		else if (input == "exit") {
			break;
		}
	}
}