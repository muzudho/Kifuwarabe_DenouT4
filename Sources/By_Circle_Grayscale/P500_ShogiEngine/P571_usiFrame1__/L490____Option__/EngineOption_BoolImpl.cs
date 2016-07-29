using Grayscale.P571_usiFrame1__.L___490_Option__;
using System;

namespace Grayscale.P571_usiFrame1__.L490____Option__
{
    public class EngineOption_BoolImpl : EngineOption_Bool
    {
        public EngineOption_BoolImpl()
        {
        }

        public EngineOption_BoolImpl(bool value, bool defaultValue)
        {
            this.m_value_ = value;
            this.m_default_ = defaultValue;
        }

        /// <summary>
        /// 既定値
        /// </summary>
        private bool m_default_;
        public bool Default
        {
            get { return this.m_default_; }
            set { this.m_default_ = value; }
        }

        /// <summary>
        /// 現在値
        /// </summary>
        private bool m_value_;
        public bool Value
        {
            get { return this.m_value_; }
            set { this.m_value_ = value; }
        }

        /// <summary>
        /// 論理値型でのみ使用可能。論理値型でない場合、エラー。
        /// </summary>
        /// <returns></returns>
        public bool IsTrue()
        {
            return this.m_value_;
        }

        /// <summary>
        /// 数値型でのみ使用可能。数値型でない場合、エラー。
        /// </summary>
        /// <returns></returns>
        public long GetNumber()
        {
            throw new ApplicationException("型変換エラー");
        }

        /// <summary>
        /// 現在値（文字列読取）
        /// </summary>
        /// <param name="value"></param>
        public void ParseValue(string value)
        {
            bool result;
            if (bool.TryParse(value, out result))
            {
                this.m_value_ = result;
            }
        }
    }
}
