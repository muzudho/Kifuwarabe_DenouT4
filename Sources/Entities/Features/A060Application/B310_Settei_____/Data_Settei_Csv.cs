﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using Grayscale.A000Platform.B011Csv.C500Parser;
using Grayscale.A060Application.B210Tushin.C500Util;
using Grayscale.Kifuwaragyoku.Entities.Logging;

namespace Grayscale.A060Application.B310Settei.C500Struct
{

    /// <summary>
    /// bin/data/data_settei.csv
    /// </summary>
    public class Data_Settei_Csv
    {

        /// <summary>
        /// カンマ区切りで。
        /// </summary>
        public string FilepathsCsv { get; set; }

        /// <summary>
        /// プロパティーズ
        /// </summary>
        public Dictionary<string, string> Properties { get; set; }

        public Data_Settei_Csv()
        {
            this.FilepathsCsv = "";
            this.Properties = new Dictionary<string, string>();
        }

        public void Read_Add(string filepath, Encoding encoding)
        {
            if (!File.Exists(filepath))
            {
                Util_Message.Show("設定ファイルが見つかりません。[" + filepath + "]");
                goto gt_EndMethod;
            }

            List<List<string>> rows = Util_Csv.ReadCsv(filepath, encoding);

            // 最初の1行は削除。
            rows.RemoveRange(0, 1);

            //------------------------------
            // データ部だけが残っています。
            //------------------------------
            if ("" == this.FilepathsCsv)
            {
                this.FilepathsCsv = filepath;
            }
            else
            {
                this.FilepathsCsv = this.FilepathsCsv + "," + filepath;
            }
            //this.Properties.Clear();

            foreach (List<string> row in rows)
            {
                // 各行は、name,value の2列以上あるはずです。
                if (row.Count < 2)
                {
                    goto gt_NextColumn;
                }

                // name列が空っぽの行は無視します。
                if ("" == row[0])
                {
                    goto gt_NextColumn;
                }

                if (this.Properties.ContainsKey(row[0]))
                {
                    Util_Message.Show("項目[" + row[0] + "]を上書きします。");
                    this.Properties[row[0]] = row[1];
                }
                else
                {
                    this.Properties.Add(row[0], row[1]);
                }

            gt_NextColumn:
                ;
            }

        gt_EndMethod:
            ;
        }

        public void DebugOut()
        {
            Logger.Trace("┏━━━━━━━━┓サイズ＝[" + this.Properties.Count + "]");

            foreach (KeyValuePair<string, string> entry in this.Properties)
            {
                Logger.Trace(entry.Key + "=「" + entry.Value + "」");

            }

            Logger.Trace("┗━━━━━━━━┛");
        }

        public string Get(string name)
        {
            string result;

            if (!this.Properties.ContainsKey(name))
            {
                Util_Message.Show("設定ファイル[" + this.FilepathsCsv + "]の中に、項目が見つかりません。\n項目名[" + name + "]\n"
                    + "もしかして？\n"
                    + "・　.odsを編集していて、.csvに出力していないとか？\n"
                    + "・　エンコーディングは合っている？\n"
                    );
                result = "";
                goto gt_EndMethod;
            }

            result = this.Properties[name];

        gt_EndMethod:
            return result;
        }

    }

}
