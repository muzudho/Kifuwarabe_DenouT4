using Grayscale.A090_UsiFramewor.B100_usiFrame1__.C___490_Option__;
using System;
using System.Collections.Generic;

namespace Grayscale.A090_UsiFramewor.B100_usiFrame1__.C490____Option__
{
    public class EngineOption_StringImpl : EngineOption_String
    {
        public EngineOption_StringImpl()
        {
            this.m_value_ = "";
            this.m_default_ = "";
        }

        /// <summary>
        /// 最初に入ってきた値を、既定値とします。
        /// </summary>
        /// <param name="value"></param>
        public EngineOption_StringImpl(string value)
        {
            this.m_value_ = value;
            this.m_default_ = value;
        }

        public EngineOption_StringImpl(string value, string defaultValue)
        {
            this.m_value_ = value;
            this.m_default_ = defaultValue;
        }

        /// <summary>
        /// 既定値
        /// </summary>
        protected string m_default_;
        public string Default
        {
            get { return this.m_default_; }
            set { this.m_default_ = value; }
        }

        /// <summary>
        /// 現在値
        /// </summary>
        protected string m_value_;
        public string Value
        {
            get { return this.m_value_; }
            set { this.m_value_ = value; }
        }

        public void Reset(
            string valueDefault,
            List<string> valueVars,
            string valueMin,
            string valueMax
            )
        {
            this.m_value_ = valueDefault;
            this.m_default_ = valueDefault;
        }

        /// <summary>
        /// 論理値型でのみ使用可能。論理値型でない場合、エラー。
        /// </summary>
        /// <returns></returns>
        public bool IsTrue()
        {
            bool result;
            if (bool.TryParse(this.m_value_,out result))
            {
                return result;
            }

            throw new ApplicationException("型変換エラー");
        }


        /// <summary>
        /// 数値型でのみ使用可能。数値型でない場合、エラー。
        /// </summary>
        /// <returns></returns>
        public long GetNumber()
        {
            long result;
            if (long.TryParse(this.m_value_, out result))
            {
                return result;
            }

            throw new ApplicationException("型変換エラー");
        }

        /// <summary>
        /// 現在値（文字列読取）
        /// </summary>
        /// <param name="value"></param>
        public void ParseValue(string value)
        {
            this.m_value_ = value;
        }


        public override string ToString()
        {
            return "value " + this.m_value_;
        }
    }
}
