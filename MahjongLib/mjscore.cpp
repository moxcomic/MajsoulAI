
#include "mjscore.h"

//成立した役名一覧
void MJScore::Get_Yakuname(string s[]){
	for(int i=0;!YakuName[i].empty();i++)
		s[i]=YakuName[i];
}

//コンストラクタ
MJScore::MJScore()
{		
	memset(Tehai,0,sizeof(Tehai));
	memset(TehaiNormalize,0,sizeof(TehaiNormalize));
	memset(TehaiNormalizeMarge,0,sizeof(TehaiNormalizeMarge));
	memset(Fuuro,0,sizeof(Fuuro));
	memset(Dora,0,sizeof(Dora));
	memset(Yaku,false,sizeof(Yaku));
	memset(k_tmp_yaku,false,sizeof(k_tmp_yaku));
	memset(k_result_yaku,false,sizeof(k_result_yaku));
	for(int i=0;i<20;i++){
		YakuName[i]="";
	}
	Jikaze=
	Bakaze=
	Honba=
	KyoutakuRiichi=
	ErrorCode=
	Haino=
	TokutenKo=
	TokutenOya=
	Fuurosuu=
	dora=
	uradora=
	akadora=
	Agarihai=
	tmp_fu=
	result_fu=
	tmp_fan=
	result_fan=
	TokutenKoTumo=
	TokutenOyaTumo=
    inputteddora=//v1.03
	0;

	Nsyan=8;
	Tsyan=7;
	Ksyan=13;

	Akahai=
	Tenhou=
	Tiihou=
	Riichi=
	DoubleRiichi=
	Ippatu=
	Tyankan=
	Rinsyan=
	NagashiMangan=
	ManganKiriage=
	Tumo=
	Haitei=
	Ba1500=
	DoubleKokushi13=
	DoubleTyuuren9=
	DoubleDaisuusii=
	DoubleSuttan=
	IsMangan=
	is_fuuro=
	false;

	ErrorCode=MIKEISAN;
	Kuitan=true;

	YakuNameTable[TENHOU]="天和";
	YakuNameTable[TIIHOU]="地和";
	YakuNameTable[NAGASHIMANGAN]="流し満貫";
	YakuNameTable[KOKUSHI]="国士無双";
	YakuNameTable[KOKUSHI13]="国士無双１３面待";
	YakuNameTable[TYUUREN]="九連宝燈";
	YakuNameTable[TYUUREN9]="九連宝燈９面待";
	YakuNameTable[DAISUUSII]="大四喜";
	YakuNameTable[SYOUSUUSII]="小四喜";
	YakuNameTable[SUUANKOUTANKI]="四暗刻単騎待";
	YakuNameTable[SUUANKOU]="四暗刻";
	YakuNameTable[DAISANGEN]="大三元";
	YakuNameTable[TINROUTOU]="清老頭";
	YakuNameTable[RYUUIISOU]="緑一色";
	YakuNameTable[SUUKANTU]="四槓子";
	YakuNameTable[TUUIISOU]="字一色";

	YakuNameTable[SANKANTU]="三槓子 2";
	YakuNameTable[RIICHI]="リーチ 1";
	YakuNameTable[DOUBLERIICHI]="ダブルリーチ 2";
	YakuNameTable[IPPATU]="一発 1";
	YakuNameTable[TANYAO]="断ヤオ 1";
	YakuNameTable[PINFU]="平和 1";
	YakuNameTable[IIPEIKOU]="一盃口 1";
	YakuNameTable[SANANKOU]="三暗刻 2";
	YakuNameTable[TOITOI]="対々和 2";
	YakuNameTable[SANSYOKUDOUKOU]="三色同刻 2";
	YakuNameTable[RYANPEIKOU]="二盃口 3";
	YakuNameTable[TIITOITU]="七対子 2";
	YakuNameTable[HONROUTOU]="混老頭 2";
	YakuNameTable[HAKU]="白 1";
	YakuNameTable[HATU]="発 1";
	YakuNameTable[TYUN]="中 1";
	YakuNameTable[BAKAZE]="場風 1";
	YakuNameTable[JIKAZE]="自風 1";
	YakuNameTable[SYOUSANGEN]="小三元 2";
	YakuNameTable[HAITEITUMO]="海底撈月 1";
	YakuNameTable[HOUTEIRON]="河底撈魚 1";
	YakuNameTable[TYANKAN]="槍槓 1";
	YakuNameTable[RINSYAN]="嶺上開花 1";
	YakuNameTable[TUMO]="門前清模和 1";

	YakuNameTable[SANSYOKUDOUJYUN]="三色同順";
	YakuNameTable[ITTUU]="一気通貫";
	YakuNameTable[TYANTA]="全帯";
	YakuNameTable[JYUNTYANTA]="純全帯";
	YakuNameTable[HONITU]="混一色";
	YakuNameTable[TINITU]="清一色";

	YakuNameTable[DORA]="ドラ";
	YakuNameTable[URADORA]="裏ドラ";
	YakuNameTable[AKADORA]="赤ドラ";
}

//中身を全てクリア
void MJScore::Clear()
{
	memset(Tehai,0,sizeof(Tehai));
	memset(TehaiNormalize,0,sizeof(TehaiNormalize));
	memset(TehaiNormalizeMarge,0,sizeof(TehaiNormalizeMarge));
	memset(Fuuro,0,sizeof(Fuuro));
	memset(Dora,0,sizeof(Dora));
	memset(Yaku,false,sizeof(Yaku));
	memset(k_tmp_yaku,false,sizeof(k_tmp_yaku));
	memset(k_result_yaku,false,sizeof(k_result_yaku));
	for(int i=0;i<20;i++){
		YakuName[i]="";
	}
	Jikaze=
	Bakaze=
	Honba=
	KyoutakuRiichi=
	ErrorCode=
	Haino=
	TokutenKo=
	TokutenOya=
	TokutenKoTumo=
	TokutenOyaTumo=
	Fuurosuu=
	dora=
	uradora=
	akadora=
	Agarihai=
	tmp_fu=
	result_fu=
	tmp_fan=
	result_fan=
    inputteddora=//v1.03
	0;

	Nsyan=8;
	Tsyan=7;
	Ksyan=13;

	Akahai=
	Tenhou=
	Tiihou=
	Riichi=
	DoubleRiichi=
	Ippatu=
	Tyankan=
	Rinsyan=
	NagashiMangan=
	ManganKiriage=
	Tumo=
	Haitei=
	Ba1500=
	DoubleKokushi13=
	DoubleTyuuren9=
	DoubleDaisuusii=
	DoubleSuttan=
	IsMangan=
	is_fuuro=
	false;

	ErrorCode=MIKEISAN;
	Kuitan=true;

	YakuNameTable[SANSYOKUDOUJYUN]="三色同順";
	YakuNameTable[ITTUU]="一気通貫";
	YakuNameTable[TYANTA]="全帯";
	YakuNameTable[JYUNTYANTA]="純全帯";
	YakuNameTable[HONITU]="混一色";
	YakuNameTable[TINITU]="清一色";

	YakuNameTable[DORA]="ドラ";
	YakuNameTable[URADORA]="裏ドラ";
	YakuNameTable[AKADORA]="赤ドラ";
}

//ルール以外の中身をクリア
void MJScore::Clear_WithoutRule()
{
	memset(Tehai,0,sizeof(Tehai));
	memset(TehaiNormalize,0,sizeof(TehaiNormalize));
	memset(TehaiNormalizeMarge,0,sizeof(TehaiNormalizeMarge));
	memset(Fuuro,0,sizeof(Fuuro));
	memset(Dora,0,sizeof(Dora));
	memset(Yaku,false,sizeof(Yaku));
	memset(k_tmp_yaku,false,sizeof(k_tmp_yaku));
	memset(k_result_yaku,false,sizeof(k_result_yaku));
	for(int i=0;i<20;i++){
		YakuName[i]="";
	}
	Jikaze=
	Bakaze=
	Honba=
	KyoutakuRiichi=
	ErrorCode=
	Haino=
	TokutenKo=
	TokutenOya=
	TokutenKoTumo=
	TokutenOyaTumo=
	Fuurosuu=
	dora=
	uradora=
	akadora=
	Agarihai=
	tmp_fu=
	result_fu=
	tmp_fan=
	result_fan=
    inputteddora=//v1.03
	0;

	Nsyan=8;
	Tsyan=7;
	Ksyan=13;

	Tenhou=
	Tiihou=
	Riichi=
	DoubleRiichi=
	Ippatu=
	Tyankan=
	Rinsyan=
	NagashiMangan=
	Tumo=
	Haitei=
	IsMangan=
	is_fuuro=
	false;

	ErrorCode=MIKEISAN;

	YakuNameTable[SANSYOKUDOUJYUN]="三色同順";
	YakuNameTable[ITTUU]="一気通貫";
	YakuNameTable[TYANTA]="全帯";
	YakuNameTable[JYUNTYANTA]="純全帯";
	YakuNameTable[HONITU]="混一色";
	YakuNameTable[TINITU]="清一色";

	YakuNameTable[DORA]="ドラ";
	YakuNameTable[URADORA]="裏ドラ";
	YakuNameTable[AKADORA]="赤ドラ";
}


//牌の数カウント
int MJScore::Check_Haino(){
	int i,cnt=0;
	for(i=0;i<=37;i++){
		for(;!TehaiNormalizeMarge[i];i++);if(i>=38) continue;
		if(TehaiNormalizeMarge[i]<0 || TehaiNormalizeMarge[i]>4) return -1;
		cnt+=TehaiNormalizeMarge[i];
	}
	return cnt;
}

//赤牌正規化
void MJScore::Tehai_Normalize(){
	memcpy(TehaiNormalize,Tehai,sizeof(Tehai));
	if(Akahai)
	{
		TehaiNormalize[5]+=Tehai[38];
		TehaiNormalize[15]+=Tehai[39];
		TehaiNormalize[25]+=Tehai[40];
	}
	TehaiNormalize[38]=-1;

	//作業用手配TehaiNormalizeMarge作成
	memcpy(TehaiNormalizeMarge,TehaiNormalize,sizeof(TehaiNormalizeMarge));
	for(int i=0;Fuuro[i]&&i<=12;i+=4){
		if((Fuuro[i]==PON)||(Fuuro[i]==AKA_PON1)||(Fuuro[i]==AKA_PON2))
		{
			TehaiNormalizeMarge[Fuuro[i+1]]+=3;
		}
		else if((Fuuro[i]==CHII)||(Fuuro[i]==AKA_CHII)){
			TehaiNormalizeMarge[Fuuro[i+1]]++;
			TehaiNormalizeMarge[Fuuro[i+1]+1]++;
			TehaiNormalizeMarge[Fuuro[i+1]+2]++;
		}
		else if((Fuuro[i]==ANKAN)||(Fuuro[i]==MINKAN)||(Fuuro[i]==KAKAN)){
			TehaiNormalizeMarge[Fuuro[i+1]]+=3;
		}
	}
}

//フーロしているか
bool MJScore::Is_Fuuro(){
	
	//v1.02 暗槓で一盃口などが消え、食い下がりのある役の翻数が減るバグ修正
	return Is_FuuroWithoutAnkan();

	//for(int i=0;i<=12;i+=4){
	//	if(Fuuro[i])return true;
	//}
	//return false;
}

//フーロしているか（アンカン除く）
bool MJScore::Is_FuuroWithoutAnkan(){
	for(int i=0;Fuuro[i]&&i<20;i+=4){
		if(Fuuro[i]!=ANKAN) return true;
	}
	return false;			
}

//フーロ数
void MJScore::Fuuro_Suu(){
	int f=0;
	for(int i=0;Fuuro[i]&&i<20;i+=4){
		f++;
	}
	Fuurosuu=f;
}

//赤牌の数
int MJScore::Get_AkahaiSuu(){
	int tmp=0;
	tmp+=Tehai[38];
	tmp+=Tehai[39];
	tmp+=Tehai[40];
	for(int i=0;Fuuro[i]&&i<20;i+=4){
		if(Fuuro[i]==ANKAN||
			Fuuro[i]==MINKAN||
			Fuuro[i]==KAKAN||
			Fuuro[i]==AKA_PON1)
		{
			if(Fuuro[i+1]%10==5&&i<30)tmp++;
		}
		else if(Fuuro[i]==AKA_PON2){
			tmp+=2;
		}
		else if(Fuuro[i]==AKA_CHII)
		{
			tmp++;
		}
	}
	return tmp;
}


//計算実行
void MJScore::Run()
{
	//流し満貫
	if(NagashiMangan){
		Fan=5;
		Fu=-1;
		Yaku[NAGASHIMANGAN]=true;
		goto decision;
	}

	//赤牌正規化
	Tehai_Normalize();

	//牌の数が14でない：エラーコード=1
	if(Check_Haino()!=14){
		ErrorCode=HAIGA14MAIJYANAI;
		return;
	}
	//あがり牌情報がおかしい：エラーコード=2
	// v1.071 あがり牌が赤だった場合の不具合修正
	if(Agarihai==0 || TehaiNormalize[Agarihai]==0){
		ErrorCode=AGARIHAIGAOKASII;
		return;
	}

	//あがっていない：エラーコード=3
	NormalSyanten();
	TiitoituSyanten();
	KokusiSyanten();
	if(!( Nsyan==-1||Tsyan==-1||Ksyan==-1)){
		ErrorCode=AGATTEINAI;
		return;
	}

	//場風か自風情報がない：エラーコード=4
	if(Bakaze==0 || Jikaze==0){
		ErrorCode=BAKAZEKAJIKAZEGANAI;
		return;
	}

	//鳴きリーチしている：エラーコード=5
	if(Is_FuuroWithoutAnkan() && (Riichi || DoubleRiichi)){
		ErrorCode=NAKIRIICHI;
		return;
	}

	//初期化
	memset(Yaku,false,sizeof(Yaku));
	Fan=Fu=0;
	for(int i=0;i<20;i++){YakuName[i]="";}

	//国士
	if(Ksyan==-1){
		Fan=YAKUMAN;
		if(Tenhou){
			Yaku[TENHOU]=true;
			Fan++;
		}
		else if(Tiihou){
			Yaku[TIIHOU]=true;
			Fan++;
		}
		if(Is_Kokusi13()){
			Yaku[KOKUSHI13]=true;
			if(DoubleKokushi13){
				Fan++;
			}
		}
		else{
			Yaku[KOKUSHI]=true;
			Fan=YAKUMAN;
		}
		Fu=-1;
		goto decision;
	}

	//チートイツ
	if(Tsyan==-1&&Nsyan>-1){
		//役満
		if(Tenhou){
			Yaku[TENHOU]=true;
			Fan=YAKUMAN;
		}
		else if(Tiihou){
			Yaku[TIIHOU]=true;
			Fan=YAKUMAN;
		}
		if(Is_Tuuiisou()){
			Yaku[TUUIISOU]=true;
			Fan==YAKUMAN ? Fan++:Fan=YAKUMAN;
		}
		if(Fan>=YAKUMAN){
			Fu=-1;
			goto decision;
		}

		//役満でない
		////////////////
		Yaku[TIITOITU]=true;
		Fu=25;
		Fan=2;
		//ダブリー
		if(DoubleRiichi){
			Yaku[DOUBLERIICHI]=true;
			Fan+=2;
		}
		//リーチ
		else if(Riichi){
			Yaku[RIICHI]=true;
			Fan++;
		}
		//一発
		if(Ippatu&&Riichi){
			Yaku[IPPATU]=true;
			Fan++;
		}
		//ハイテイツモ
		if(Haitei && Tumo){
			Yaku[HAITEITUMO]=true;
			Fan++;
		}
		//ホウテイロン
		else if(Haitei && !Tumo){
			Yaku[HOUTEIRON]=true;
			Fan++;
		}
		//ドラ
		if(Is_Dora()){
			Yaku[DORA]=true;
			Fan+=dora;
		}
		//赤ドラ
		if(Akahai&&Is_Akadora()){
			Yaku[AKADORA]=true;
			Fan+=akadora;
		}
		//混老頭
		if(Is_Honroutou()){
			Yaku[HONROUTOU]=true;
			Fan+=2;
		}
		//チンイツ
		if(Is_Tinitu()){
			Yaku[TINITU]=true;
			is_fuuro ? Fan+=5 : Fan+=6;
			//タンヤオ
			if(Is_Tanyao()){
				Yaku[TANYAO]=true;
				Fan++;
			}
		}
		//ホンイツ
		else if(Is_Honitu()){
			Yaku[HONITU]=true;
			is_fuuro ? Fan+=2 : Fan+=3;
		}
		else{
			//タンヤオ
			if(Is_Tanyao()){
				Yaku[TANYAO]=true;
				Fan++;
			}
		}
		//ツモ
		if(Tumo){
			Yaku[TUMO]=true;
			Fan++;
		}
		goto decision;
	}
	//通常手
	else{
		//フーロしているか
		is_fuuro=Is_Fuuro();
		//フーロ数を求める
		if(is_fuuro) Fuuro_Suu();
		//役満
		if(Chk_NormalYakuman()){
			goto decision;
		}
		//メンツ構成が関係無い役
		Chk_NotPatternYaku();
		//メンツ構成が関係有る役＋符
		Chk_PatternYaku();
		//ツモ
		if(Tumo&&!Is_FuuroWithoutAnkan()){
			Yaku[TUMO]=true;
			Fan++;
		}
		//ドラ
		if(Fan>0){
			if(Is_Dora()){
				Yaku[DORA]=true;
				Fan+=dora;
			}
			//赤ドラ
			if(Akahai&&Is_Akadora()){
				Yaku[AKADORA]=true;
				Fan+=akadora;
			}
		}
	}

	//得点決め
decision:
	if(!NagashiMangan)
	{
		//役がない：エラーコード=6
		if(Fan==0){
			ErrorCode=YAKUNASI;
			return;
		}
	}
	Decision_Score(Fan,Fu);
	ErrorCode=SEIJYOU;
}

//最終的な得点
void MJScore::Decision_Score(const int fan,const int fu)
{
	int kihonten;
	int tumibou=300*Honba;
	if(Ba1500) tumibou=1500*Honba;//場千五

	//基本点を求める
	switch(fan){
		case 5: kihonten=2000; break; //満貫
		case 6:
		case 7: kihonten=3000; break; //ハネ満貫
		case 8:
		case 9:
		case 10: kihonten=4000; break; //倍満貫
		case 11:
		case 12: kihonten=6000; break; //3倍満貫

		case 13:case 14:case 15:case 16:case 17:
		case 18:case 19:case 20:case 21:case 22:
		case 23:case 24:case 25:case 26:case 27:
		case 28:case 29:case 30:case 31:case 32:
		case 33:case 34:case 35:case 36:case 37:
		case 38: kihonten=8000; break; //数え役満

		case YAKUMAN: kihonten=8000; break; //役満
		case YAKUMAN2: kihonten=8000*2; break; //2倍役満
		case YAKUMAN3: kihonten=8000*3; break; //3倍役満
		case YAKUMAN4: kihonten=8000*4; break; //4倍役満
		case YAKUMAN5: kihonten=8000*5; break; //5倍役満
		case YAKUMAN6: kihonten=8000*6; break; //6倍役満
		case YAKUMAN7: kihonten=8000*7; break; //7倍役満
		case YAKUMAN8: kihonten=8000*8; break; //8倍役満
		case YAKUMAN9: kihonten=8000*9; break; //9倍役満
		case YAKUMAN10: kihonten=8000*10; break; //10倍役満
	    case YAKUMAN11: kihonten=8000*11; break; //11倍役満
		default:{
			kihonten=fu*(int)pow(2.0,fan+2);
			if(kihonten>=2000) kihonten=2000;
			break; //4ハン未満
		}
	}
	//満貫切り上げ
	if(ManganKiriage){
		if((fan==4 && fu==30) || (fan==6 && fu==30))
			kihonten=2000;
	}
	//マンガンか
	if(kihonten==2000) IsMangan=true;

	//振り分け
	OyaRon=kihonten*6;
	OyaTumo=kihonten*2;
	KoRon=kihonten*4;
	KoTumoOya=kihonten*2;
	KoTumoKo=kihonten*1;

	//100未満切り上げ
	OyaRon=(OyaRon%100>0)?OyaRon+100-OyaRon%100:OyaRon;
	OyaTumo=(OyaTumo%100>0)?OyaTumo+100-OyaTumo%100:OyaTumo;
	KoRon=(KoRon%100>0)?KoRon+100-KoRon%100:KoRon;
	KoTumoOya=(KoTumoOya%100>0)?KoTumoOya+100-KoTumoOya%100:KoTumoOya;
	KoTumoKo=(KoTumoKo%100>0)?KoTumoKo+100-KoTumoKo%100:KoTumoKo;

	//積棒
	OyaRon+=tumibou;
	OyaTumo+=tumibou/3;
	KoRon+=tumibou;
	KoTumoOya+=tumibou/3;
	KoTumoKo+=tumibou/3;

	//供託リーチ
	TokutenOya=OyaRon+KyoutakuRiichi*1000;
	TokutenKo=KoRon+KyoutakuRiichi*1000;

	//ツモアガリ時に最終的にもらう点数 v1.02
	TokutenOyaTumo=OyaTumo*3+KyoutakuRiichi*1000;
	TokutenKoTumo=KoTumoOya+KoTumoKo*2+KyoutakuRiichi*1000;

	//役名前

	if(Yaku[HONITU]&&is_fuuro){
		YakuNameTable[HONITU]+=" ";
		YakuNameTable[HONITU]+=2+48;
	}
	else{
		YakuNameTable[HONITU]+=" ";
		YakuNameTable[HONITU]+=3+48;
	}

	if(Yaku[TINITU]&&is_fuuro){
		YakuNameTable[TINITU]+=" ";
		YakuNameTable[TINITU]+=5+48;
	}
	else{
		YakuNameTable[TINITU]+=" ";
		YakuNameTable[TINITU]+=6+48;
	}

	if(Yaku[TYANTA]&&is_fuuro){
		YakuNameTable[TYANTA]+=" ";
		YakuNameTable[TYANTA]+=1+48;
	}
	else{
		YakuNameTable[TYANTA]+=" ";
		YakuNameTable[TYANTA]+=2+48;
	}

	if(Yaku[JYUNTYANTA]&&is_fuuro){
		YakuNameTable[JYUNTYANTA]+=" ";
		YakuNameTable[JYUNTYANTA]+=2+48;
	}
	else{
		YakuNameTable[JYUNTYANTA]+=" ";
		YakuNameTable[JYUNTYANTA]+=3+48;
	}
	if(Yaku[ITTUU]&&is_fuuro){
		YakuNameTable[ITTUU]+=" ";
		YakuNameTable[ITTUU]+=1+48;
	}
	else{
		YakuNameTable[ITTUU]+=" ";
		YakuNameTable[ITTUU]+=2+48;
	}
	if(Yaku[SANSYOKUDOUJYUN]&&is_fuuro){
		YakuNameTable[SANSYOKUDOUJYUN]+=" ";
		YakuNameTable[SANSYOKUDOUJYUN]+=1+48;
	}
	else{
		YakuNameTable[SANSYOKUDOUJYUN]+=" ";
		YakuNameTable[SANSYOKUDOUJYUN]+=2+48;
	}


	for(int i=0,j=0;i<50;i++){
		if(Yaku[i]){
			YakuName[j]=YakuNameTable[i];
			j++;
		}
	}
}

//通常手役満チェック
bool MJScore::Chk_NormalYakuman()
{
	//v1.06 v1.02エンバグ、アンカンで九蓮宝燈等が成立してしまっていたのを修正
	bool is_fuuro_withAnkan = false;
	for(int i=0;i<=12;i+=4){
		if(Fuuro[i]){
			is_fuuro_withAnkan=true;
			break;
		}
	}

	//if(!is_fuuro)
	if(!is_fuuro_withAnkan)
	{
		if(Tenhou){
			Yaku[TENHOU]=true;
			Fan++;
		}
		else if(Tiihou){
			Yaku[TIIHOU]=true;
			Fan++;
		}	
		if(Is_Tyuuren9()){
			Yaku[TYUUREN9]=true;
			if(DoubleTyuuren9) Fan++;
			Fan++;
			goto end_thisfunc;
		}
		else if(Is_Tyuuren()){
			Yaku[TYUUREN]=true;
			Fan++;
			goto end_thisfunc;
		}	
	}
	if(!Is_FuuroWithoutAnkan())
	{
		if(Is_SuuankouuTanki()){
			Yaku[SUUANKOUTANKI]=true;
			if(DoubleSuttan) Fan++;
			Fan++;
		}
		else if(Is_Suuankouu()){
			Yaku[SUUANKOU]=true;
			Fan++;
		}
	}
	if(Is_Ryuuiisou()){
		Yaku[RYUUIISOU]=true;
		Fan++;
		if(Is_Suukantu()){
			Yaku[SUUKANTU]=true;
			Fan++;
		}
		goto end_thisfunc;
	}
	else if(Is_Tinroutou()){
		Yaku[TINROUTOU]=true;
		Fan++;
		if(Is_Suukantu()){
			Yaku[SUUKANTU]=true;
			Fan++;
		}
		Fu=-1;
		goto end_thisfunc;
	}
	if(Is_Daisuusii()){
		Yaku[DAISUUSII]=true;
		if(DoubleDaisuusii) Fan++;
		Fan++;
	}
	else if(Is_Syousuusii()){
		Yaku[SYOUSUUSII]=true;
		Fan++;
	}
	if(Is_Tuuiisou()){
		Yaku[TUUIISOU]=true;
		Fan++;
	}
	if(Is_Suukantu()){
		Yaku[SUUKANTU]=true;
		Fan++;
	}
	if(Is_Daisangen()){
		Yaku[DAISANGEN]=true;
		Fan++;
	}

end_thisfunc:
	if(Fan>0){
		Fan+=YAKUMAN-1;
		Fu=-1;
		return true;
	}
	return false;
}

//メンツ構成が関係無い役
void MJScore::Chk_NotPatternYaku()
{
		//ダブリー
		if(DoubleRiichi){
			Yaku[DOUBLERIICHI]=true;
			Fan+=2;
		}
		//リーチ
		else if(Riichi){
			Yaku[RIICHI]=true;
			Fan++;
		}
		//一発
		if(Ippatu&&(Riichi||DoubleRiichi)){
			Yaku[IPPATU]=true;
			Fan++;
		}
		//ハイテイツモ
		if(Haitei && Tumo){
			Yaku[HAITEITUMO]=true;
			Fan++;
		}
		//ホウテイロン
		else if(Haitei && !Tumo){
			Yaku[HOUTEIRON]=true;
			Fan++;
		}
		//混老頭
		if(Is_Honroutou()){
			Yaku[HONROUTOU]=true;
			Fan+=2;
		}
		//チンイツ
		if(!Yaku[HONROUTOU]&&Is_Tinitu()){
			Yaku[TINITU]=true;
			is_fuuro ? Fan+=5 : Fan+=6;
			//タンヤオ
			if(Is_Tanyao()){
				Yaku[TANYAO]=true;
				Fan++;
			}
		}
		//ホンイツ
		else if(!Yaku[TINITU]&&Is_Honitu()){
			Yaku[HONITU]=true;
			is_fuuro ? Fan+=2 : Fan+=3;
		}
		else{
			//タンヤオ
			if(Is_Tanyao()){
				Yaku[TANYAO]=true;
				Fan++;
			}
		}
		if(!Yaku[TANYAO])
		{
			//白
			if(TehaiNormalizeMarge[35]==3){
				Yaku[HAKU]=true;
				Fan++;
			}
			//発
			if(TehaiNormalizeMarge[36]==3){
				Yaku[HATU]=true;
				Fan++;
			}
			//中
			if(TehaiNormalizeMarge[37]==3){
				Yaku[TYUN]=true;
				Fan++;
			}
			//場風
			if(TehaiNormalizeMarge[Bakaze]==3){
				Yaku[BAKAZE]=true;
				Fan++;
			}
			//自風
			if(TehaiNormalizeMarge[Jikaze]==3){
				Yaku[JIKAZE]=true;
				Fan++;
			}
			//小三元
			if(Is_Syousangen()){
				Yaku[SYOUSANGEN]=true;
				Fan+=2;
			}
		}
		//槍槓
		if(Tyankan){
			Yaku[TYANKAN]=true;
			Fan++;
		}
		//嶺上開花
		if(Rinsyan){
			Yaku[RINSYAN]=true;
			Fan++;
		}
		//三槓子
		if(Is_Sankantu()){
			Yaku[SANKANTU]=true;
			Fan+=2;
		}
}

//メンツ構成が関係有る役
void MJScore::Chk_PatternYaku()
{
	//メンツ切り分け
	MentuKiriwake();
	//結果を統合
	Fan+=result_fan;
	Fu=result_fu;
	Yaku[PINFU]=k_result_yaku[PINFU];
	Yaku[IIPEIKOU]=k_result_yaku[IIPEIKOU];
	Yaku[SANSYOKUDOUJYUN]=k_result_yaku[SANSYOKUDOUJYUN];
	Yaku[ITTUU]=k_result_yaku[ITTUU];
	Yaku[SANANKOU]=k_result_yaku[SANANKOU];
	Yaku[TYANTA]=k_result_yaku[TYANTA];
	Yaku[JYUNTYANTA]=k_result_yaku[JYUNTYANTA];
	Yaku[TOITOI]=k_result_yaku[TOITOI];
	Yaku[SANSYOKUDOUKOU]=k_result_yaku[SANSYOKUDOUKOU];
	Yaku[RYANPEIKOU]=k_result_yaku[RYANPEIKOU];
}
void MJScore::MentuKiriwake()
{
	int i;
	//初期化
	memset(kiriwake,0,sizeof(kiriwake));
	memset(result_kiriwake,0,sizeof(result_kiriwake));
	p_kiriwake=0;
	tmp_fan=tmp_fu=0;
	memset(k_tmp_yaku,0,sizeof(k_tmp_yaku));
	memset(k_result_yaku,0,sizeof(k_result_yaku));

	//フーロ牌を統合
	for(i=0;i<20;i+=4){
	if(Fuuro[i]!=0){
		if(Fuuro[i]==CHII||Fuuro[i]==AKA_CHII){
			kiriwake[p_kiriwake]=CHII;
		}
		else if(Fuuro[i]==PON||Fuuro[i]==AKA_PON1||Fuuro[i]==AKA_PON2){
			kiriwake[p_kiriwake]=PON;
		}
		else{
			kiriwake[p_kiriwake]=Fuuro[i];
		}
		kiriwake[p_kiriwake+1]=Fuuro[i+1];
		p_kiriwake+=2;
	}
	else{
		break;
		}
	}
	//頭抜き出し
	//あがった状態でしか呼び出さない点に注意
	for(i=0;i<=37;i++){
	for(;!TehaiNormalize[i];i++);if(i>=38) continue;
		if(TehaiNormalize[i] >=2){
			TehaiNormalize[i] -=2;
		//データを格納しポインタを進める
		kiriwake[p_kiriwake]=TOITU;kiriwake[p_kiriwake+1] =i,p_kiriwake+=2;
		//メンツ抜き出し関数へ
		KiriwakeNukidasi();
		p_kiriwake-=2,kiriwake[p_kiriwake]=0,kiriwake[p_kiriwake+1]=0;
		TehaiNormalize[i] += 2;
		}
	}
}

//メンツ抜き出し関数
void MJScore::KiriwakeNukidasi()
{
	int i;
	for(i=0;i<38;i++){
	for(;!TehaiNormalize[i];i++);
	if(i>=38){
		if(kiriwake[9]!=0){//４メンツ１頭とれてるなら
		/********************************/
		/* ここで切り分けが終わります　　*/
		/* ここから符計算等に入ります　　*/
		/* 構成が関係する役はメンツごと　*/
		/* にここで判定すること　　　　　*/
		/********************************/

		bool fuuro=is_fuuro;
		if(!fuuro)
		{
			if(Is_Pinfu())//平和
			{
				k_tmp_yaku[PINFU]=true;
				tmp_fan++;
			}
			if(Is_Ryanpeikou())//二盃口
			{
				k_tmp_yaku[RYANPEIKOU]=true;
				tmp_fan+=3;
			}
			else if(Is_Iipeikou())//一盃口
			{
				k_tmp_yaku[IIPEIKOU]=true;
				tmp_fan++;
			}
		}
		
		if(Is_Ittuu())//一気通貫
		{
			k_tmp_yaku[ITTUU]=true;
			if(!fuuro)tmp_fan++;
			tmp_fan++;
		}
		
		if(Is_Sansyokudoujyun())//三色同順
		{
			k_tmp_yaku[SANSYOKUDOUJYUN]=true;
			if(!fuuro)tmp_fan++;
			tmp_fan++;
		}
		else if(Is_Sansyokudoukou())//三色同刻
		{
			k_tmp_yaku[SANSYOKUDOUKOU]=true;
			tmp_fan+=2;
		}
		if(!Yaku[HONROUTOU]&&Is_Jyuntyanta())//純全帯幺
		{
			k_tmp_yaku[JYUNTYANTA]=true;
			if(!fuuro)tmp_fan++;
			tmp_fan+=2;
		}
		else if(!Yaku[HONROUTOU]&&Is_Tyanta())//全帯幺
		{
			k_tmp_yaku[TYANTA]=true;
			if(!fuuro)tmp_fan++;
			tmp_fan++;
		}
		if(!k_tmp_yaku[ITTUU]&&!k_tmp_yaku[SANSYOKUDOUJYUN])
		{
			if(Is_Toitoi())//対々和
			{
				k_tmp_yaku[TOITOI]=true;
				tmp_fan+=2;
			}
			if(Is_Sanankou())
			{
				k_tmp_yaku[SANANKOU]=true;
				tmp_fan+=2;
			}
		}

		Chk_Fu();
		//これまでで最高の役数＋符ならば記憶
		if((tmp_fan>result_fan)||(tmp_fan==result_fan && tmp_fu>result_fu))
		{
			result_fan=tmp_fan;
			result_fu=tmp_fu;
			memcpy(result_kiriwake,kiriwake,sizeof(int)*10);
			memcpy(k_result_yaku,k_tmp_yaku,sizeof(k_result_yaku));
		}
		tmp_fan=tmp_fu=0;
		memset(k_tmp_yaku,0,sizeof(k_tmp_yaku));
	}
	return;
	}
	//アンコ抜き出し
	if(TehaiNormalize[i] >=3){
		TehaiNormalize[i] -=3;
		//ロンのあがり牌ならミンコウである
		if(i==Agarihai&& !Tumo){
			kiriwake[p_kiriwake]=PON;
		}
		else{
			kiriwake[p_kiriwake]=ANKO;
		}
		kiriwake[p_kiriwake+1] =i,p_kiriwake+=2;
		KiriwakeNukidasi();//再帰
		p_kiriwake-=2,kiriwake[p_kiriwake]=0,kiriwake[p_kiriwake+1]=0;
		TehaiNormalize[i] += 3;
	}
	//シュンツ抜き出し
	if(TehaiNormalize[i] && TehaiNormalize[i+1] && TehaiNormalize[i+2] &&i<30){
		TehaiNormalize[i]--,TehaiNormalize[i+1]--,TehaiNormalize[i+2]--;
		kiriwake[p_kiriwake]=SYUNTU,kiriwake[p_kiriwake+1] =i,p_kiriwake+=2;
 		KiriwakeNukidasi();//再帰
		p_kiriwake-=2,kiriwake[p_kiriwake]=0,kiriwake[p_kiriwake+1]=0;
		TehaiNormalize[i]++,TehaiNormalize[i+1]++,TehaiNormalize[i+2]++;
		}	            
	}
}

//符計算
void MJScore::Chk_Fu()
{
	bool menzen=!Is_FuuroWithoutAnkan();

	//０・ピンフなら
	if(k_tmp_yaku[PINFU]){
		if(Tumo)
			tmp_fu=20;
		else
			tmp_fu=30;
		return;
	}

	tmp_fu=20;//作業用の符、１・副底を代入
	//２・門前加符
	if(menzen & !Tumo)  tmp_fu += 10;
	//３・ツモ符
	if(Tumo) 
		tmp_fu+=2;
	//４・待ち
	for(int i=0;i<10;i+= 2){
	if(kiriwake[i]==TOITU){
	//単騎待ち
	if(kiriwake[i+1]==Agarihai){
		tmp_fu += 2;
		break;
		}
	}
	else if((kiriwake[i]==SYUNTU )||(kiriwake[i]==CHII)){
	if(
		(kiriwake[i+1]==Agarihai-2 &&kiriwake[i+1]%10==1) ||//ペン３待ち
		(kiriwake[i+1]==Agarihai &&kiriwake[i+1]%10==7) ||//ペン７待ち
		kiriwake[i+1]==Agarihai-1||//カンチャン
		kiriwake[i+1]==Agarihai+1//カンチャン
		)
		{
			tmp_fu += 2;
			break;
			}
		}
	}
	//５・雀頭
	for(int i=0;i<10;i+= 2){
	if(kiriwake[i]==TOITU){
		if(kiriwake[i+1] >=35||kiriwake[i+1]==Bakaze)//役牌
			tmp_fu += 2;
		if(kiriwake[i+1]==Jikaze)//連風牌
			tmp_fu += 2;
		}
	}
	//６・面子の構成による符
	for(int i=0; i<10 ;i+= 2){
		if(kiriwake[i]==ANKO){//暗コウ
			if(kiriwake[i+1]%10==1||kiriwake[i+1]%10==9||kiriwake[i+1] > 30)
				tmp_fu += 8;//ヤオチュウ
			else
				tmp_fu += 4;// 中張
		}
		else if(kiriwake[i] ==PON){	//明刻
			if(kiriwake[i+1]%10==1||kiriwake[i+1]%10==9||kiriwake[i+1] > 30)
				tmp_fu += 4;//ヤオチュウ
			else
				tmp_fu += 2;// 中張
		}
		else if(kiriwake[i]==ANKAN){ //アンカン
			if(kiriwake[i+1]%10==1||kiriwake[i+1]%10==9||kiriwake[i+1] > 30)
				tmp_fu += 32;//ヤオチュウ
			else
				tmp_fu += 16;// 中張
		}
		else if((kiriwake[i]==MINKAN)||(kiriwake[i]==KAKAN)){	    //ミンカン
			if(kiriwake[i+1]%10==1||kiriwake[i+1]%10==9||kiriwake[i+1] > 30)
				tmp_fu += 16;//ヤオチュウ
			else
				tmp_fu += 8;// 中張
		}
	} 
	//７・鳴きピンフ形
	//ここまでで加符が無い＝２０符＝鳴きピンフ形
	if(tmp_fu==20) tmp_fu=30;
	//１の位切り上げ
	tmp_fu=(tmp_fu % 10 > 0)  ?  tmp_fu + 10 - tmp_fu % 10 : tmp_fu;
}


//国士無双13面待ち
bool MJScore::Is_Kokusi13()
{
	int yaotyuu[13] = {1,9,11,19,21,29,31,32,33,34,35,36,37};
	int i=0; 
	Tehai[Agarihai]--;
	for(;i<13;i++){
		if(Tehai[yaotyuu[i]]!=1){
			Tehai[Agarihai]++;
			return false;
		}
	}
	Tehai[Agarihai]++;
	return true;
}

//字一色
bool MJScore::Is_Tuuiisou()
{
	int i=0;
	for(;i<=37;i++){ 
		for(;!TehaiNormalize[i];i++); if(i>=38) continue;
		if(i<30)
			return false;
	}
	for(i=0;Fuuro[i]&&i<20;i+=4){
		if(Fuuro[i+1]<=30)
			return false;
	}
	return true;
}

//大三元
bool MJScore::Is_Daisangen()
{
	if(TehaiNormalizeMarge[35]==3 && TehaiNormalizeMarge[36]==3 && TehaiNormalizeMarge[37]==3)
		return true;
	return false;
}

//九蓮宝燈9面待ち
bool MJScore::Is_Tyuuren9()
{
	int syurui=0;
	int i;
	if((bool)TehaiNormalizeMarge[1] + (bool)TehaiNormalizeMarge[11] + (bool)TehaiNormalizeMarge[21] != 1 )   //C言語でTRUEは数字の１として扱える
		return false;
	//字牌があれば当然FALSE
	for(i=31;i<=37;i++){
		if(TehaiNormalizeMarge[i]) return false;
	}
	//種類を記憶
	if(TehaiNormalizeMarge[1])		syurui=0;
	else if(TehaiNormalizeMarge[11])	syurui=1;
	else if(TehaiNormalizeMarge[21])	syurui=2;
	TehaiNormalizeMarge[Agarihai]--;
	for(i=syurui*10+1;i<=syurui*10+9;i++){
	switch(i%10){
		case 1:
		case 9:
		if(TehaiNormalizeMarge[i]!=3){
			TehaiNormalizeMarge[Agarihai]++;
			return false;
		}
		break;
		default:
		if(TehaiNormalizeMarge[i]!=1){
			TehaiNormalizeMarge[Agarihai]++;
			return false;
		}
		break;
		}
	}
	TehaiNormalizeMarge[Agarihai]++;
	return true;
}

//九蓮宝燈
bool MJScore::Is_Tyuuren()
{
	int syurui=0;
	int i;
	if((bool)TehaiNormalizeMarge[1] + (bool)TehaiNormalizeMarge[11] + (bool)TehaiNormalizeMarge[21] != 1 )//C言語でTRUEは数字の１として扱える
		return false;
	//字牌があれば当然FALSE
	for(i=31;i<=37;i++){
		if(TehaiNormalizeMarge[i]) return false;
	}
	//種類を記憶
	if(TehaiNormalizeMarge[1])		syurui=0;
	else if(TehaiNormalizeMarge[11])	syurui=1;
	else if(TehaiNormalizeMarge[21])	syurui=2;
	for(i=syurui*10+1;i<=syurui*10+9;i++){
	switch(i%10){
		case 1:
		case 9:
		if(TehaiNormalizeMarge[i]<3)
			return false;
		break;
		default:
		if(TehaiNormalizeMarge[i]<1)
			return false;
		break;
		}
	}
	return true;
}

//緑一色
bool MJScore::Is_Ryuuiisou()
{
	int i=0;
	for(;i<=37;i++){ 
		for(;!TehaiNormalizeMarge[i];i++);	if(i>=38) continue;
		if(!(i==22||i==23||i==24||i==26||i==28||i==36))
			return false;
	}
	return true;
}

//清老頭
bool MJScore::Is_Tinroutou()
{
	int i=0;
	for(;i<=37;i++){ 
		for(;!TehaiNormalizeMarge[i];i++); if(i>=38) continue;
		if(!(i==1||i==9||i==11||i==19||i==21||i==29))
			return false;
	}
	return true;

}

//大四喜
bool MJScore::Is_Daisuusii()
{
	for(int i=31;i<=34;i++){
		if(TehaiNormalizeMarge[i] <3)
			return false;
	}
	return true;
}

//小四喜
bool MJScore::Is_Syousuusii()
{
	bool toitu=false;
	int anko=0;
	for(int i=31;i<=34;i++){
		if(toitu==false && TehaiNormalizeMarge[i]==2)
			toitu=true ;
		else if(TehaiNormalizeMarge[i]==3)
			anko++;
	}
	return (anko==3 && toitu) ? true : false;
}

//四暗刻単騎
bool MJScore::Is_SuuankouuTanki()
{
	int i,kootu=0;
	bool toitu=false;

	TehaiNormalizeMarge[Agarihai]--;
	for(i=0;i<=37;i++){
		for(;!TehaiNormalizeMarge[i];i++); if(i>=38) continue;
		 
		if(TehaiNormalizeMarge[i]==1 && (i==Agarihai))
			toitu=true;
		else if(TehaiNormalizeMarge[i]==3)
			kootu++;
	}
	TehaiNormalizeMarge[Agarihai]++;
	return (kootu==4 && toitu) ? true : false;
}

//四暗刻
bool MJScore::Is_Suuankouu()
{
	int i,kootu=0;
	bool toitu=false;

	if(!Tumo) return false;
	for(i=0;i<=37;i++){
		for(;!TehaiNormalizeMarge[i];i++); if(i>=38) continue;
		if(TehaiNormalizeMarge[i]==1)
			return false;
		else if(toitu==false && TehaiNormalizeMarge[i]==2)
			toitu=true ;
		else if(TehaiNormalizeMarge[i]==3)
			kootu++;
	}
	return (kootu==4 && toitu) ? true : false;
}

//四槓子
bool MJScore::Is_Suukantu()
{
	if(Fuurosuu!=4)
		return false;
	for(int i=0;Fuuro[i]&&i<20;i+=4){
		if((Fuuro[i]!=ANKAN)&&(Fuuro[i]!=MINKAN)&&(Fuuro[i]!=KAKAN))
			return false;
	}
	return true;
}

//小三元
bool MJScore::Is_Syousangen()
{
	bool toitu=false;
	int koutu=0;	int i;
	for(i=35;i<=37;i++){
		if(TehaiNormalizeMarge[i]==2&& toitu==false)
			toitu=true;
		if(TehaiNormalizeMarge[i]==3)
			koutu++;
	}
	return (koutu==2 && toitu==true) ? true : false;
}

//三槓子
bool MJScore::Is_Sankantu()
{
	if(Fuurosuu<3)
		return false;
	int kan=0;
	for(int i=0;Fuuro[i]&&i<20;i+=4){
		if((Fuuro[i]==ANKAN)||(Fuuro[i]==MINKAN)||(Fuuro[i]==KAKAN))
			kan++;
	}
	return (kan>=3) ? true : false;
}

//混老頭
bool MJScore::Is_Honroutou()
{
	int i = 0;
	bool jihai = false;
	bool routou = false; 
	for(;i <= 37;i++){ 
		for(;!TehaiNormalizeMarge[i];i++); if(i >= 38) continue; 
		if( ( i%10 > 1 && i%10 < 9) && i < 30 )     //老頭or字牌以外ならFALSE
			return false;
		else if( ( i%10 == 1 || i%10 == 9) && i < 30 && routou == false)  //老頭牌を記憶
				routou = true;
		else if( i > 30 && jihai == false)  //字牌を記憶
			jihai = true ;
	}
	return (routou & jihai) ? true : false;
}

//ホンイツ
bool MJScore::Is_Honitu()
{
	int syurui;
	int i ;
	bool jihai = false;             
	//最初の牌種を記憶
	for(i=0;!TehaiNormalizeMarge[i];i++);
	syurui = i / 10;
	if(syurui == 3) return false;//最初の牌種が字牌ならFALSE 
	//残りの牌＜＞最初の牌種ならFALSE
	//字牌があれば記憶
	for(;i <= 37;i++){
		for(;!TehaiNormalizeMarge[i];i++);if(i >= 38) continue;
		if(i/10 != syurui && i<30 )
			return false;
		else if(i>30 && !jihai)
			jihai=true;
	}
	return jihai ? true : false;
}

//チンイツ
bool MJScore::Is_Tinitu()
{
	int syurui,i;
	//最初の牌種を記憶
	for(i=0;!TehaiNormalizeMarge[i];i++);
	syurui = i / 10;
	if(syurui == 3) return false;//字牌ならFALSE 
	//最初の牌種と違うならFALSE
	for(;i <= 37;i++){
		for(;!TehaiNormalizeMarge[i];i++);if(i >= 38) continue;
		if(i/10 != syurui )	return false;
	}
	return true;
}

//赤ドラ
bool MJScore::Is_Akadora()
{
	if(Akahai){
		int tmp=Get_AkahaiSuu();
		if(Get_AkahaiSuu()>0){
			Yaku[AKADORA]=true;
			akadora=tmp;
			YakuNameTable[AKADORA]+=" ";
			YakuNameTable[AKADORA]+=(char)(akadora+48);
			return true;
		}
	}
	return false;
}

//ドラ
bool MJScore::Is_Dora()
{
	int i,tmp;
	//表ドラ
	for(i=1;i<=5;i++){
		if(Dora[i]){
			if(Dora[i]<30&&Dora[i]%10==9) tmp=(Dora[i]/10)*10+1;
			else if(Dora[i]==34) tmp=31;
			else if(Dora[i]==37) tmp=35;
			else tmp=Dora[i]+1;			
			dora+=TehaiNormalizeMarge[tmp];
			for(int i=0;Fuuro[i]&&i<=12;i+=4){
				if(((Fuuro[i]==ANKAN)||(Fuuro[i]==MINKAN)||(Fuuro[i]==KAKAN))&&Fuuro[i+1]==tmp)
					dora++;
			}
		}
	}
	//裏ドラ
	if(Riichi&&!Is_FuuroWithoutAnkan())
	{
		for(i=6;i<=9;i++){
			if(Dora[i]){
				if(Dora[i]<30&&Dora[i]%10==9) tmp=(Dora[i]/10)*10+1;
				else if(Dora[i]==34) tmp=31;
				else if(Dora[i]==37) tmp=35;
				else tmp=Dora[i]+1;				
				uradora+=TehaiNormalizeMarge[tmp];
				for(int j=0;Fuuro[j]&&j<=12;j+=4){
					if(((Fuuro[j]==ANKAN)||(Fuuro[j]==MINKAN)||(Fuuro[j]==KAKAN))&&Fuuro[j+1]==tmp)
						dora++;
				}
			}
		}
	}
	//裏ドラあり
	if(uradora){
		Yaku[URADORA]=true;
		YakuNameTable[URADORA]+=" ";
		if(uradora>=10) YakuNameTable[URADORA]+=(char)(uradora/10)+48;
		YakuNameTable[URADORA]+=(uradora%10)+48;

		Fan+=uradora;//v1.02
	}
    //v1.03 ドラだけ外部入力有り
    if(inputteddora>0){
        dora+=inputteddora;
    }
	//ドラあり
	if(dora){
		YakuNameTable[DORA]+=" ";
		if(dora>=10) YakuNameTable[DORA]+=(char)(dora/10)+48;
		YakuNameTable[DORA]+=(dora%10)+48;
	}
	return (bool)(dora+uradora);
}

//タンヤオ
bool MJScore::Is_Tanyao()
{
	if(!Kuitan&&is_fuuro){
		return false;
	}
	int i=0;
	for(;i<=37;i++){ 
		for(;!TehaiNormalize[i];i++); if(i>=38) continue;
		if(i>30||i%10==1||i%10==9)
			return false;
	}
	for(i=0;Fuuro[i]&&i<20;i+=4){
		if(Fuuro[i+1]>30||Fuuro[i+1]%10==1||Fuuro[i+1]%10==9||
			Fuuro[i+1]%10==7&&(Fuuro[i]==CHII || Fuuro[i]==AKA_CHII))
			return false;
	}
	return true;
}

//平和
bool MJScore::Is_Pinfu()
{
	//頭が役牌でないか、シュンツ以外はないか
	for(int i=0;i<10;i+= 2){
		if(kiriwake[i]==TOITU){
			if(kiriwake[i+1]==Jikaze ||kiriwake[i+1]==Bakaze ||kiriwake[i+1] >=35 )
				return false;
		}
		else if(kiriwake[i] != SYUNTU)
			return false;
	}
	//両面待ち
	bool ryanmen=false;    
	for(int i=0;i<10;i+= 2){
		if(kiriwake[i]==SYUNTU ){
			if((kiriwake[i+1]==Agarihai-2 &&kiriwake[i+1]%10 >=2 )||
		(kiriwake[i+1]==Agarihai  &&kiriwake[i+1]%10 < 7 ) )
		 ryanmen=true;
		}
	} 
	if(ryanmen){
		return true;
	}
	return false;
}
//一盃口
bool MJScore::Is_Iipeikou()
{
	int chk[38];//作業用
	memset(chk,0,sizeof(chk));
	for(int i=2;i<10;i+=2){
		if(kiriwake[i]==SYUNTU ){
	        chk[kiriwake[i+1]]++;
		}
	}
	for(int i=0;i<=37;i++){
	for(;!chk[i];i++);if(i>=38) continue;
		if(chk[i]>=2){
			return true;
		}
	}
	return false;
}
//一気通貫
bool MJScore::Is_Ittuu()
{
	bool chk[10];//作業用
	for(int i=0;i<10;i++) chk[i]=0;
	for(int i=0;i<10;i+= 2){
	if(kiriwake[i]==SYUNTU ||kiriwake[i]==CHII){
		if(kiriwake[i+1]==1 ) chk[0]=1;
		else if(kiriwake[i+1]==4 ) chk[1]=1;
		else if(kiriwake[i+1]==7 ) chk[2]=1;
		else if(kiriwake[i+1]==11) chk[3]=1;
		else if(kiriwake[i+1]==14) chk[4]=1;
		else if(kiriwake[i+1]==17) chk[5]=1;
		else if(kiriwake[i+1]==21) chk[6]=1;
		else if(kiriwake[i+1]==24) chk[7]=1;
		else if(kiriwake[i+1]==27) chk[8]=1;
		}
	}
	if((chk[0]& chk[1] & chk[2])|| (chk[3] & chk[4]& chk[5]) || (chk[6]& chk[7] & chk[8]))
	{
		return true;
	}
	return false;

}
//三色同順
bool MJScore::Is_Sansyokudoujyun()
{
	int chk[30];       //作業用
	memset(chk,0,sizeof(chk));
	for(int i=0;i<10;i+= 2){
		if(kiriwake[i]==SYUNTU ||kiriwake[i]==CHII){
			chk[kiriwake[i+1]]++;
		}
	}
	for(int i=1;i<10;i++)
		if(chk[i]&&chk[i+10]&&chk[i+20])
			return true;
	return false;
}

//全帯幺
bool MJScore::Is_Tyanta()
{
	bool jihai=false;//字牌フラグ
	for(int i=0;i<10;i+= 2){
		//頭

		if(kiriwake[i]==TOITU){
			if( !(kiriwake[i+1]%10== 1||kiriwake[i+1]%10== 9) &&kiriwake[i+1] <30 )
				return false;
			if(kiriwake[i+1]  > 30)
				jihai=true;
		}
		//シュンツ
		else if(kiriwake[i]==SYUNTU ||kiriwake[i]==CHII){
			if(!(kiriwake[i+1]%10==1||kiriwake[i+1]%10==7) )
				return false;
		}
		//コーツ
		else if (kiriwake[i]==ANKO ||kiriwake[i]==ANKAN ||kiriwake[i]==MINKAN||
			kiriwake[i]==KAKAN||kiriwake[i]==ANKO||kiriwake[i]==PON){
			if(!(kiriwake[i+1]%10==1||kiriwake[i+1]%10==9  ||kiriwake[i+1]  > 30))
				return false;
			if(kiriwake[i+1]  > 30)
				jihai=true;
		}
	}
	return jihai? true : false;
}

//三暗刻
bool MJScore::Is_Sanankou()
{
	int chk=0;
	for(int i=0;i<10;i+= 2){
		if(kiriwake[i]==ANKO ||kiriwake[i]==ANKAN){
			chk++;
		}	            
	}
	return (chk >=3) ? true : false;
}

//純全帯幺
bool MJScore::Is_Jyuntyanta()
{
	for(int i=0;i<10;i+= 2){
		//頭
		if(kiriwake[i]==TOITU){
			if(!(kiriwake[i+1]%10==1||kiriwake[i+1]%10==9 ) ||kiriwake[i+1]  > 30 )
				return false;
		}
		//シュンツ
		else if(kiriwake[i]==SYUNTU ||kiriwake[i]==CHII){
			if(!(kiriwake[i+1]%10==1||kiriwake[i+1]%10==7) )
				return false;
		}
		//コーツ
		else if (kiriwake[i]==ANKO ||kiriwake[i]==ANKAN ||kiriwake[i]==MINKAN||
			kiriwake[i]==KAKAN||kiriwake[i]==ANKO||kiriwake[i]==PON){
			if(!(kiriwake[i+1]%10==1||kiriwake[i+1]%10==9 ) ||kiriwake[i+1]  > 30)
				return false;
		}
	}
	return true;

}
//対々和
bool MJScore::Is_Toitoi()
{
	for(int i=0;i<10;i+= 2){
		if(kiriwake[i]==SYUNTU ||kiriwake[i]==CHII)
			return false;
	}
	return true;

}
//三色同刻
bool MJScore::Is_Sansyokudoukou()
{
	int chk[50];       //作業用
	memset(chk,0,sizeof(chk));
	for(int i=0;i<10;i+= 2){
		if (kiriwake[i]==PON||kiriwake[i]==ANKO ||kiriwake[i]==ANKAN ||kiriwake[i]==KAKAN ||kiriwake[i]==MINKAN||kiriwake[i]==AKA_PON1||kiriwake[i]==AKA_PON2)
		{
			chk[kiriwake[i+1]]++;
		}
	}
	for(int i=1; i<10;i++){
		if(chk[i]&&chk[i+10]&&chk[i+20]){
			return true;
		}
	}
	return false;
}
//二盃口
bool MJScore::Is_Ryanpeikou()
{
	int chk[38],kazu=0;//作業用
	memset(chk,0,sizeof(chk));
	for(int i=0;i<10;i+= 2){
		if(kiriwake[i]==SYUNTU ){
			chk[kiriwake[i+1]]++;
		}
	} 
	for(int i=0;i<=37;i++){
		for(;!chk[i];i++);if(i>=38) continue;
		if(chk[i] >=2){
		if(++kazu==2|| chk[i]==4)
			return true;
		}
	}
	return false;

}

//国士無双のシャンテン数
void MJScore::KokusiSyanten()
{
	int kokusi_toitu = 0,syanten_kokusi = 13,i;             
	//老頭牌
	for(i = 1;i < 30;i++){        
		if(i%10 == 1 || i%10 == 9){	   
			if(TehaiNormalize[i])
				syanten_kokusi--;
			if(TehaiNormalize[i] >= 2 && kokusi_toitu == 0)
				kokusi_toitu=1;	
		}
	}             
	//字牌
	for(i = 31;i < 38;i++){      
		if(TehaiNormalize[i]){	        
			syanten_kokusi--;
			if(TehaiNormalize[i] >= 2 && kokusi_toitu == 0)
				kokusi_toitu = 1;			            
		}
	}             
	//頭
	syanten_kokusi -= kokusi_toitu;
	Ksyan=syanten_kokusi;
}

//チートイツのシャンテン数
void MJScore::TiitoituSyanten()
{
	int i = 1,toitu = 0,syurui = 0,syanten_tiitoi; 
	//トイツ数を数える
	for(;i <= 37;i++){ 
		for(;!TehaiNormalize[i];i++);
		if(i >= 38) continue;
		syurui++;
		if(TehaiNormalize[i] >= 2) toitu++;
	}
	syanten_tiitoi = 6-toitu;
 
	//４枚持ちを考慮
	if(syurui < 7) syanten_tiitoi += 7-syurui;             
	Tsyan=syanten_tiitoi;
}

//通常手のシャンテン数
//入口
void MJScore::NormalSyanten()
{
	//初期化
	nsayn_mentu = 0;
	nsayn_toitu = 0;
	nsayn_kouho = 0;
	nsayn_temp = 0;
	syanten_normal = 8; 

	Fuuro_Suu();//フーロ数取得

	for(int i = 1;i < 38;i++)
	{
		//頭抜き出し
		if(2 <= TehaiNormalize[i])
		{            
			nsayn_toitu++;
			TehaiNormalize[i] -= 2;
			nsayn_mentu_cut(1);
			TehaiNormalize[i] += 2;
			nsayn_toitu--;
		}
	} 
	nsayn_mentu_cut(1); //頭無しと仮定して計算             

	

	/*
	//死に孤立字牌
	int sini_jihai=0;
	for(int i=31;i<38;i++){
		if(TehaiNormalize[i]==4){
			sini_jihai=1;
			break;
		}
	}
	*/

	Nsyan=syanten_normal-Fuurosuu*2; //最終的な結果
}

//メンツ抜き出し
void MJScore::nsayn_mentu_cut(int i)
{
	for(;!TehaiNormalize[i];i++);
	if(i>=38){nsayn_taatu_cut(1);return;} //メンツを抜き終わったのでターツ抜きへ             
	//コーツ
	if(TehaiNormalize[i]>=3)
	{
		nsayn_mentu++;
		TehaiNormalize[i]-=3;
		nsayn_mentu_cut(i);
		TehaiNormalize[i]+=3;
		nsayn_mentu--;
	}             
	//シュンツ
	if(TehaiNormalize[i+1]&&TehaiNormalize[i+2]&&i<30)
	{
		nsayn_mentu++;
		TehaiNormalize[i]--,TehaiNormalize[i+1]--,TehaiNormalize[i+2]--;
		nsayn_mentu_cut(i);
		TehaiNormalize[i]++,TehaiNormalize[i+1]++,TehaiNormalize[i+2]++;
		nsayn_mentu--;
	}             
	//メンツ無しと仮定
	nsayn_mentu_cut(i+1);
}

//ターツ抜き出し
void MJScore::nsayn_taatu_cut(int i)
{													   
	for(;!TehaiNormalize[i];i++); 
	if(i>=38)  //抜き出し終了
	{
		nsayn_temp=8-nsayn_mentu*2-nsayn_kouho-nsayn_toitu;
		if(nsayn_temp<syanten_normal) { syanten_normal=nsayn_temp; }
		return;
	} 
	//面子＋ターツ＋フーロ数
	if(nsayn_mentu+nsayn_kouho+Fuurosuu<4)
	{            
		//トイツ
		if(TehaiNormalize[i]==2)
		{
			nsayn_kouho++;
			TehaiNormalize[i]-=2;
			nsayn_taatu_cut(i);
			TehaiNormalize[i]+=2;
			nsayn_kouho--;
		}
	             
		//ペンチャンorリャンメン
		if(TehaiNormalize[i+1]&&i<30)
		{
			nsayn_kouho++;
			TehaiNormalize[i]--,TehaiNormalize[i+1]--;
			nsayn_taatu_cut(i);
			TehaiNormalize[i]++,TehaiNormalize[i+1]++;
			nsayn_kouho--;
		}
	             
		//カンチャン
		if(TehaiNormalize[i+2]&&i<30&&i%10<=8)
		{
			nsayn_kouho++;
			TehaiNormalize[i]--,TehaiNormalize[i+2]--;
			nsayn_taatu_cut(i);
			TehaiNormalize[i]++,TehaiNormalize[i+2]++;
			nsayn_kouho--;
		}
	}
	nsayn_taatu_cut(i+1);
}
/*
***************************************************************************************
* 【シャンテン数計算問題点】
* 例えば「東東東東南南南南西西西北北北」のシャンテン数は１ですが
* このソースでは０（テンパイ）になってしまいます。
* これは４枚目の字牌はターツになる事ができない「死に孤立字牌」なのが理由です。これを回避するには
* 「どれか一つでも「４枚目の字牌」を持っていたらシャンテン数＋１ 」とすれば良いかもしれませんが未検証です。
***************************************************************************************
*/
