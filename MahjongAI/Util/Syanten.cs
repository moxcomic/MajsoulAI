using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MahjongAI.Util
{
    class Syanten
    {
        int[] tehai = new int[39];
        int mentu;      //メンツ数
        int toitu;      //トイツ数
        int kouho;      //ターツ数
        int fuurosuu;       //フーロ数
        int temp;       //シャンテン数（計算用）
        int syanten_normal; //シャンテン数（結果用）
        Dictionary<int, List<int>> map_ = new Dictionary<int, List<int>>();

        //コンストラクタ
        public Syanten(string dataFilePath)
        {
            fuurosuu = 0;

            //作業用
            int key;
            int pattern1_m;
            int pattern1_t;
            int pattern2_m;
            int pattern2_t;
            int index = 1, indexloop = 0;

            //シャンテンファイル読み込み
            StreamReader reader = new StreamReader(dataFilePath);
            string str;

            while ((str = reader.ReadLine()) != null)
            {
                var fields = str.Split().Select(field => int.Parse(field)).ToArray();
                map_[fields[0]] = new List<int>() { fields[1], fields[2], fields[3], fields[4] };
            }

        }

        //中身クリア
        public void clear()
        {
            for (int i = 0; i < 38; i++)
            {
                tehai[i] = 0;
            }
            tehai[38] = -1;
            fuurosuu = 0;
        }

        //手牌セット
        public void set_tehai(int[] t)
        {
            for (int i = 0; i < 38; i++)
            {
                tehai[i] = t[i];
            }
            tehai[38] = -1;
        }

        //フーロ数
        public void set_fuurosuu(int a)
        {
            fuurosuu = a;
        }

        //国士シャンテン
        public int KokusiSyanten()
        {
            int kokusi_toitu = 0, syanten_kokusi = 13, i;
            //老頭牌
            for (i = 1; i < 30; i++)
            {
                if (i % 10 == 1 || i % 10 == 9 || i % 20 == 1 || i % 20 == 9)
                {
                    if (tehai[i] != 0)
                        syanten_kokusi--;
                    if (tehai[i] >= 2 && kokusi_toitu == 0)
                        kokusi_toitu = 1;
                }
            }
            //字牌
            for (i = 31; i < 38; i++)
            {
                if (tehai[i] != 0)
                {
                    syanten_kokusi--;
                    if (tehai[i] >= 2 && kokusi_toitu == 0)
                        kokusi_toitu = 1;
                }
            }
            //頭
            syanten_kokusi -= kokusi_toitu;
            return syanten_kokusi;
        }

        //チートイシャンテン
        public int TiitoituSyanten()
        {
            int i = 1, toitu = 0, syurui = 0, syanten_tiitoi;
            //トイツ数を数える
            for (; i <= 37; i++)
            {
                for (; tehai[i] == 0; i++) ;
                if (i >= 38) continue;
                syurui++;
                if (tehai[i] >= 2)
                    toitu++;
            }
            syanten_tiitoi = 6 - toitu;
            //４枚持ちを考慮
            if (syurui < 7)
                syanten_tiitoi += 7 - syurui;
            return syanten_tiitoi;
        }

        //通常手シャンテン
        public int NormalSyanten()
        {
            int result = 99;
            int tmpresult = 0;
            for (int i = 1; i < 38; i++)
            {
                //頭抜き出し
                if (2 <= tehai[i])
                {
                    tehai[i] -= 2;
                    tmpresult = checkNormalSyanten() - 1;
                    if (tmpresult < result)
                    {
                        result = tmpresult;
                    }
                    tehai[i] += 2;
                }
            }

            tmpresult = checkNormalSyanten();   //頭無しと仮定して計算
            if (tmpresult < result)
            {
                result = tmpresult;
            }

            return result;
        }

        public int AnySyanten(out int normalSyanten)
        {
            int res = normalSyanten = NormalSyanten();
            if (fuurosuu == 0)
            {
                res = Math.Min(res, TiitoituSyanten());
                res = Math.Min(res, KokusiSyanten());
                res = Math.Min(res, KokusiSyanten());
            }
            return res;
        }

        //Checkシャンテン
        public int checkNormalSyanten()
        {
            int key = 0;
            int pt1m = 0, pt1t = 0;
            int pt2m = 0, pt2t = 0;
            int pt = 0, ptm = 0, ptt = 0;
            List<int> tmp;

            for (int j = 0; j < 3; j++)
            {
                key = 0;
                for (int i = 1 + j * 10, offset = 100000000; i <= 9 + j * 10; i++, offset /= 10)
                {
                    key += tehai[i] * offset;
                }

                tmp = map_[key];

                pt1m = tmp[0];
                pt1t = tmp[1];

                pt2m = tmp[2];
                pt2t = tmp[3];

                if (pt1m * 2 + pt1t >= pt2m * 2 + pt2t)
                {
                    ptm += pt1m;
                    ptt += pt1t;
                }
                else
                {
                    ptm += pt2m;
                    ptt += pt2t;
                }
            }

            for (int i = 31; i <= 37; i++)
            {
                if (tehai[i] >= 3)
                {
                    ptm++;
                }
                else if (tehai[i] >= 2)
                {
                    ptt++;
                }
            }

            while (ptm + ptt > 4 - fuurosuu && ptt > 0) ptt--;
            while (ptm + ptt > 4 - fuurosuu) ptm--;

            return 8 - ptm * 2 - ptt - fuurosuu * 2;
        }
    }
}
