/*

麻雀得点計算C++ソース「MJScore.h」ver1.0 初出
使い方:http://cmj3.web.fc2.com/index.htm#mjscore

*/

#pragma once
#ifndef __MJSCORE__
#define __MJSCORE__

//汎用ヘッダ
#include <cstdio>
#include <cstdlib>
#include <cmath>
#include <string>
#include <iostream>
#include <cstring>
using namespace std;

#pragma warning(disable: 4800)

//本体の定義
class MJScore{
private:
		//汎用
		int Tehai[50]; //手牌
		int TehaiNormalize[50];	//手牌(赤牌を統合)
		int TehaiNormalizeMarge[50];//手牌(赤牌、鳴き牌を統合、作業用)
		int Fuuro[20]; //フーロ牌
		int Dora[11]; //ドラ牌
		int ErrorCode; //エラーコード

		//ルール
		bool Akahai; //赤有り
		bool Kuitan; //クイタン有り
		bool ManganKiriage; //満貫切り上げ有り
		bool Ba1500; //場千五有り

		bool DoubleKokushi13; //国士13面待ちダブル役満有り
		bool DoubleTyuuren9; //九連宝燈9面待ちダブル役満有り
		bool DoubleDaisuusii; //大四喜ダブル役満有り
		bool DoubleSuttan; //四暗刻単騎ダブル役満有り

		//情報
		int Jikaze; //自風
		int Bakaze; //場風
		int Honba; //本場
		int KyoutakuRiichi; //供託リーチ
		int Agarihai; //和了り牌
		bool Tumo; //ツモあがり

		//各フラグ
		bool Tenhou; //天和
		bool Tiihou; //地和
		bool Riichi; //リーチ
		bool DoubleRiichi; //ダブルリーチ
		bool Ippatu; //一発
		bool Tyankan; //チャンカン
		bool Rinsyan; //リンシャン
		bool Haitei; //海底（河底）
		bool NagashiMangan; //流し満貫

		//結果
		int OyaRon;	//親ロン
		int OyaTumo; //親ツモ
		int KoRon; //子ロン
		int KoTumoOya; //子ツモ(親)
		int KoTumoKo; //子ツモ(子)
		int Fu; //符
		int Fan; //飜数
		int TokutenOya;		//最終的に親が貰う点数(ロン)
		int TokutenKo;		//最終的に子が貰う点数(ロン)
		int TokutenOyaTumo;	//最終的に親が貰う点数(ツモ)
		int TokutenKoTumo;	//最終的に子が貰う点数(ツモ)
		string YakuName[20]; //役名(結果)
		bool IsMangan;//マンガンかどうか
        int inputteddora;//v1.03 ドラ外部入力

		//作業用
		int Haino; //今何枚持っているか
		int Nsyan; //通常手のシャンテン数
		int Tsyan; //チートイツのシャンテン数
		int Ksyan; //国士無双のシャンテン数
		int Fuurosuu; //フーロ数
		bool is_fuuro; //フーロしているか(flag)

		//***** 作業用関数
		//牌の数カウント
		int Check_Haino();

		//赤牌正規化
		void Tehai_Normalize();

		//フーロしているか
		bool Is_Fuuro();

		//フーロしているか（アンカン除く）
		bool Is_FuuroWithoutAnkan();

		//フーロ数
		void Fuuro_Suu();

		//赤牌の数
		int Get_AkahaiSuu();

		//シャンテン数計算用
		int nsayn_mentu; //メンツ数
		int nsayn_toitu; //トイツ数
		int nsayn_kouho; //ターツ数
		int nsayn_temp; //シャンテン数（計算用）
		int syanten_normal;	//シャンテン数（作業用）
		void NormalSyanten(); //通常手のシャンテン数
		void nsayn_mentu_cut(int i); //メンツ抜き関数(通常手)
		void nsayn_taatu_cut(int i); //ターツ抜き関数(通常手)
		void TiitoituSyanten(); //チートイツのシャンテン数
		void KokusiSyanten();  //国士無双のシャンテン数

		//メンツ切り分け用
		int kiriwake[10];//作業用配列
		int result_kiriwake[10];//結果用配列
		int mentu_yaku_tmp;//作業用役数
		int mentu_yaku_result;//結果用役数
		int p_kiriwake;  //kiriwake[10]のポインタ用変数
		void MentuKiriwake();//メンツ抜き出し入口
		void KiriwakeNukidasi();//メンツ抜き出し関数
		int tmp_fu; //切り分け時の作業符
		int result_fu; //切り分け時の結果符
		int tmp_fan; //切り分け時の作業役数
		int result_fan; //切り分け時の結果役数
		bool k_tmp_yaku[60]; //切り分け時の作業役名
		bool k_result_yaku[60]; //切り分け時の結果役名


		//得点計算用
		string YakuNameTable[60]; //役名結果用
		bool Yaku[60];//成立した役の配列

		bool Is_Kokusi13(); //国士13面待ち
		bool Is_Tuuiisou(); //字一色
		bool Is_Tyuuren9(); //九蓮宝燈9面待ち
		bool Is_Tyuuren(); //九蓮宝燈
		bool Is_Ryuuiisou(); //緑一色
		bool Is_Suukantu(); //四槓子
		bool Is_Tinroutou(); //清老頭
		bool Is_Daisuusii(); //大四喜
		bool Is_Syousuusii(); //小四喜
		bool Is_SuuankouuTanki(); //四暗刻単騎
		bool Is_Suuankouu();  //四暗刻
		bool Is_Daisangen(); //大三元
		bool Is_Honroutou(); //混老頭

		bool Is_Pinfu();//平和
		bool Is_Iipeikou();//一盃口
		bool Is_Ittuu();//一気通貫
		bool Is_Sansyokudoujyun();//三色同順
		bool Is_Tyanta();//全帯幺
		bool Is_Jyuntyanta();//純全帯幺
		bool Is_Toitoi();//対々和
		bool Is_Sansyokudoukou();//三色同刻
		bool Is_Ryanpeikou();//二盃口
		bool Is_Sanankou();//三暗刻


		bool Is_Dora(); //ドラ
		bool Is_Akadora(); //赤ドラ
		bool Is_Tanyao(); //タンヤオ
		bool Is_Honitu(); //混一色
		bool Is_Tinitu(); //清一色
		bool Is_Syousangen(); //小三元
		bool Is_Sankantu(); //三槓子

		int dora; //ドラ数
		int uradora; //裏ドラ数
		int akadora; //赤ドラ数

		void Chk_NotPatternYaku(); //メンツ構成が関係無い役
		void Chk_PatternYaku(); //メンツ構成が関係有る役
		void Chk_Fu(); //符計算

		bool Chk_NormalYakuman(); //通常手役満チェック
		void Decision_Score(const int,const int); //最終的な得点

public:
		//メンツ定数
		enum MENTU{
			PON=1,
			CHII,
			ANKAN,
			MINKAN,
			KAKAN,
			AKA_PON1,
			AKA_PON2,
			AKA_CHII,
			ANKO,//作業用
			SYUNTU,//作業用
			TOITU//作業用
		};

		//エラーコード定数
		enum ERRORCODE{
			MIKEISAN=-1,
			SEIJYOU=0,
			HAIGA14MAIJYANAI,
			AGARIHAIGAOKASII,
			AGATTEINAI,
			BAKAZEKAJIKAZEGANAI,
			NAKIRIICHI,
			YAKUNASI
		};

		//役名の定数
		enum YAKU{
			RIICHI=0,
			DOUBLERIICHI,
			IPPATU,
			TUMO,
			TANYAO,
			PINFU,
			IIPEIKOU,
			BAKAZE,
			JIKAZE,
			HAKU,
			HATU,
			TYUN,
			RINSYAN,
			TYANKAN,
			HAITEITUMO,
			HOUTEIRON,
			SANSYOKUDOUJYUN,
			ITTUU,
			TYANTA,
			TIITOITU,
			TOITOI,
			SANANKOU,
			HONROUTOU,
			SANSYOKUDOUKOU,
			SANKANTU,
			SYOUSANGEN,
			HONITU,
			JYUNTYANTA,
			RYANPEIKOU,
			TINITU,

			TENHOU,
			TIIHOU,

			KOKUSHI,
			KOKUSHI13,
			TYUUREN,
			TYUUREN9,
			SUUANKOU,
			SUUANKOUTANKI,
			DAISUUSII,
			SYOUSUUSII,
			DAISANGEN,
			TUUIISOU,
			TINROUTOU,
			RYUUIISOU,
			SUUKANTU,

			DORA,
			URADORA,
			AKADORA,

			NAGASHIMANGAN
		};
		//役満定数
		enum FUKUGOU_YAKUMAN{
			YAKUMAN=40,//役満
			YAKUMAN2,//2倍役満
			YAKUMAN3,//3倍役満
			YAKUMAN4,//4倍役満
			YAKUMAN5,//5倍役満
			YAKUMAN6,//6倍役満
			YAKUMAN7,//7倍役満
			YAKUMAN8,//8倍役満
			YAKUMAN9,//9倍役満
			YAKUMAN10,//10倍役満
			YAKUMAN11//11倍役満
		};

		//コンストラクタ
		MJScore();

		/********************************************************/
		/***** 入力                                         *****/
		/********************************************************/

		//***** ルール入力

		//赤牌有り
		void Avail_Akahai(bool arg){
			Akahai=arg;
		}

		//クイタン有り
		void Avail_Kuitan(bool arg){
			Kuitan=arg;
		}

		//満貫切り上げ有り
		void Avail_ManganKiriage(bool arg){
			ManganKiriage=arg;
		}

		//場千五有り
		void Avail_Ba1500(bool arg){
			Ba1500=arg;
		}

		//国士13面待ちダブル役満有り
		void Avail_DoubleKokushi13(bool arg){
			DoubleKokushi13=arg;
		}

		//九連宝燈9面待ちダブル役満有り
		void Avail_DoubleTyuuren9(bool arg){
			DoubleTyuuren9=arg;
		}

		//大四喜ダブル役満有り
		void Avail_DoubleDaisuusii(bool arg){
			DoubleDaisuusii=arg;
		}

		//四暗刻単騎ダブル役満有り
		void Avail_DoubleSuttan(bool arg){
			DoubleSuttan=arg;
		}

		//***** 手牌等入力

		//非フーロ牌入力
		void Set_Tehai(int t[]){
			memcpy(Tehai,t,sizeof(int)*41);
		}

		//フーロ牌入力
		void Set_Fuuro(int f[]){
			memcpy(Fuuro,f,sizeof(int)*20);
		}

		//ドラ牌入力
		void Set_Dora(int d[]){
			memcpy(Dora,d,sizeof(int)*9);
		}

		//自風
		void Set_Jikaze(int arg){
			Jikaze=arg;
		}

		//場風
		void Set_Bakaze(int arg){
			Bakaze=arg;
		}

		//あがり牌
		void Set_Agarihai(int arg){
			Agarihai=arg;
		}

		//ツモあがり
		void Set_Tumoagari(int arg){
			Tumo=arg;
		}

		//N本場
		void Set_Honba(int arg){
			Honba=arg;
		}

		//供託リーチ
		void Set_KyoutakuRIichi(int arg){
			KyoutakuRiichi=arg;
		}
    
        //ドラ数外部入力 v1.03
        void Set_Dorasuu(int arg){
            inputteddora=arg;
        }
    
    
		//***** 以下の偶然役は成立時のみフラグをセットします

		//リーチ
		void Is_Riichi(bool arg){
			Riichi=arg;
		}

		//天和
		void Is_Tenhou(bool arg){
			Tenhou=arg;
		}

		//地和
		void Is_Tiihou(bool arg){
			Tiihou=arg;
		}

		//ダブルリーチ
		void Is_DoubleRiichi(bool arg){
			DoubleRiichi=arg;
		}

		//一発
		void Is_Ippatu(bool arg){
			Ippatu=arg;
		}

		//チャンカン
		void Is_Tyankan(bool arg){
			Tyankan=arg;
		}

		//リンシャン
		void Is_Rinsyan(bool arg){
			Rinsyan=arg;
		}

		//流し満貫
		void Is_NagashiMangan(bool arg){
			NagashiMangan=arg;
		}

		//ハイテイ
		void Is_Haitei(bool arg){
			Haitei=arg;
		}

		/********************************************************/
		/***** 実行                                         *****/
		/********************************************************/

		//計算を実行
		void Run();

		/********************************************************/
		/***** 出力                                         *****/
		/********************************************************/

		//エラーコード取得
		int Get_ErrorCode(){
			return ErrorCode;
		}


               std::string Get_ErrorCodeString(){
                   std::string r = "エラーコード取得エラー";
                   if(ErrorCode == MIKEISAN) r = "未計算";
                   if(ErrorCode == SEIJYOU) r = "正常終了";
                   if(ErrorCode == HAIGA14MAIJYANAI) r = "牌が14枚じゃない";
                   if(ErrorCode == AGARIHAIGAOKASII) r = "あがり牌がおかしい";
                   if(ErrorCode == AGATTEINAI) r = "あがっていない";
                   if(ErrorCode == BAKAZEKAJIKAZEGANAI) r = "場風か自風がない";
                   if(ErrorCode == NAKIRIICHI) r = "鳴きリーチ";
                   if(ErrorCode == YAKUNASI) r = "役なし";
                   return r;
               }

		//親ロン取得
		int Get_OyaRon(){
			return OyaRon;
		}

		//親ツモ取得
		int Get_OyaTumo(){
			return OyaTumo;
		}

		//子ロン取得
		int Get_KoRon(){
			return KoRon;
		}

		//子ツモ(親)取得
		int Get_KoTumoOya(){
			return KoTumoOya;
		}

		//子ツモ(子)取得
		int Get_KoTumoKo(){
			return KoTumoKo;
		}

		//符取得
		int Get_Fu(){
			return Fu;
		}

		//飜数取得
		int Get_Fan(){
			return Fan;
		}

		//最終的に親が貰う点数(ロン)
		int Get_TokutenOya(){
			return TokutenOya;
		}

		//最終的に子が貰う点数(ロン)
		int Get_TokutenKo(){
			return TokutenKo;
		}

		//最終的に親が貰う点数(ツモ)
		int Get_TokutenOyaTumo(){
			return TokutenOyaTumo;
		}

		//最終的に子が貰う点数(ツモ)
		int Get_TokutenKoTumo(){
			return TokutenKoTumo;
		}

		//成立した役テーブル取得
		void Get_Yakutable(bool t[]){
			memcpy(t,Yaku,sizeof(bool)*60);
		}

		//成立した役名一覧
		void Get_Yakuname(string []);

		//ドラ数(赤除く)
		int Get_Dorasuu(){
			return dora;
		}

		//裏ドラ数
		int Get_Uradorasuu(){
			return uradora;
		}

		//赤ドラ数
		int Get_Akadorasuu(){
			return akadora;
		}

		//マンガンかどうか
		bool Get_Ismangan(){
			return IsMangan;
		}
    
        //v1.03
        //int[10]
        //切り分け確定後のメンツ
        void Get_ResultKiriwake(int *arg){
            memcpy(arg,result_kiriwake,sizeof(result_kiriwake));
        }

		/********************************************************/
		/***** その他                                       *****/
		/********************************************************/

		//中身を全てクリア
		void Clear();
		//ルール以外の中身をクリア
		void Clear_WithoutRule();
};


#endif
