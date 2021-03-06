﻿using System.Text;
using Grayscale.Kifuwaragyoku.Entities.Evaluation;

namespace Grayscale.Kifuwaragyoku.UseCases.Features
{
    public abstract class Format_FeatureVector_Scale
    {

        /// <summary>
        /// テキストを作ります。
        /// 駒割。
        /// </summary>
        /// <param name="fv"></param>
        /// <returns></returns>
        public static string Format_Text(IFeatureVector fv)
        {
            StringBuilder sb = new StringBuilder();

            //
            // コメント
            //
            sb.AppendLine("\"#紹介文\",");
            sb.AppendLine("\"#\",");
            sb.AppendLine("\"#きふわらべ評価値　スケール\",");
            sb.AppendLine("\"#\",");
            sb.AppendLine("\"#ここにコメントを書いても、自動的に上書きされてしまうぜ☆？\",");

            //
            // 仕様バージョン
            //
            sb.AppendLine();
            sb.AppendLine("\"Version\",1.0,");

            //
            // 二駒関係PP
            //

            sb.Append("\"NikomaKankeiPp\",\"二駒関係ＰＰへの倍率\",");
            sb.Append(fv.Bairitu_NikomaKankeiPp);
            sb.AppendLine(",");

            sb.Append("\"NikomaKankeiPp_TyoseiryoSmallest\",\"二駒関係ＰＰの調整量最小値\",");
            sb.Append(fv.TyoseiryoSmallest_NikomaKankeiPp);
            sb.AppendLine(",");

            sb.Append("\"NikomaKankeiPp_TyoseiryoLargest\",\"二駒関係ＰＰの調整量最大値\",");
            sb.Append(fv.TyoseiryoLargest_NikomaKankeiPp);
            sb.AppendLine(",");

            sb.Append("\"NikomaKankeiPp_TyoseiryoInit\",\"二駒関係ＰＰの調整量初期値\",");
            sb.Append(fv.TyoseiryoInit_NikomaKankeiPp);
            sb.AppendLine(",");

            return sb.ToString();
        }
    }
}
