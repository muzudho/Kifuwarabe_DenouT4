using Grayscale.P091_usiFrame1__.L___490_Option__;
using System;
using System.Collections.Generic;

namespace Grayscale.P091_usiFrame1__.L490____Option__
{
    public class EngineOption_SpinImpl : EngineOption_Spin
    {
        public EngineOption_SpinImpl()
        {
        }

        public EngineOption_SpinImpl(string value, string min, string max)
        {
            this.ParseValue(value);
            this.m_default_ = this.m_value_;
            this.m_min_ = this.m_value_;
            this.m_max_ = this.m_value_;
        }

        public EngineOption_SpinImpl(int value, int defaultValue, int min, int max)
        {
            this.m_value_ = value;
            this.m_default_ = defaultValue;
            this.m_min_ = min;
            this.m_max_ = max;
        }

        /// <summary>
        /// 既定値
        /// </summary>
        private int m_default_;
        public int Default
        {
            get { return this.m_default_; }
            set { this.m_default_ = value; }
        }

        /// <summary>
        /// 現在値
        /// </summary>
        private int m_value_;
        public int Value
        {
            get { return this.m_value_; }
            set { this.m_value_ = value; }
        }

        /// <summary>
        /// 最小値
        /// </summary>
        private int m_min_;
        public int Min
        {
            get { return this.m_min_; }
            set { this.m_min_ = value; }
        }

        /// <summary>
        /// 最大値
        /// </summary>
        private int m_max_;
        public int Max
        {
            get { return this.m_max_; }
            set { this.m_max_ = value; }
        }

        public void Reset(
            string valueDefault,
            List<string> valueVars,
            string valueMin,
            string valueMax
            )
        {
            this.ParseValue(valueMin);
            this.m_min_ = this.m_value_;

            this.ParseValue(valueMax);
            this.m_max_ = this.m_value_;

            this.ParseValue(valueDefault);
            this.m_default_ = this.m_value_;
        }


        /// <summary>
        /// 論理値型でのみ使用可能。論理値型でない場合、エラー。
        /// </summary>
        /// <returns></returns>
        public bool IsTrue()
        {
            throw new ApplicationException("型変換エラー");
        }

        /// <summary>
        /// 数値型でのみ使用可能。数値型でない場合、エラー。
        /// </summary>
        /// <returns></returns>
        public long GetNumber()
        {
            return this.Value;
        }

        /// <summary>
        /// 現在値（文字列読取）
        /// </summary>
        /// <param name="value"></param>
        public void ParseValue(string value)
        {
            int result;
            if (int.TryParse(value, out result))
            {
                this.m_value_ = result;
            }
        }


        public override string ToString()
        {
            return "value " + this.m_value_;
        }
    }
}
