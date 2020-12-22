using System;
using System.Collections.Generic;

namespace Grayscale.Kifuwaragyoku.UseCases.Features
{
    public class EngineOptionBoolImpl : IEngineOptionBool
    {
        public EngineOptionBoolImpl()
        {
        }

        public EngineOptionBoolImpl(string value)
        {
            this.ParseValue(value);
            this.m_default_ = this.m_value_;
        }

        public EngineOptionBoolImpl(bool value, bool defaultValue)
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



        public void Reset(
            string valueDefault,
            List<string> valueVars,
            string valueMin,
            string valueMax
            )
        {
            this.ParseValue(valueDefault);
            this.m_default_ = this.m_value_;
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

        public override string ToString()
        {
            return "value " + this.m_value_;
        }
    }
}
