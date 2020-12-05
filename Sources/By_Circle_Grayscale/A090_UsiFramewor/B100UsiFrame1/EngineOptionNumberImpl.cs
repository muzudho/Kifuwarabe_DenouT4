using System;
using System.Collections.Generic;
using Grayscale.A090UsiFramewor.B100UsiFrame1.C490Option;

namespace Grayscale.A090UsiFramewor.B100UsiFrame1.C490Option
{
    public class EngineOptionNumberImpl : IEngineOptionNumber
    {
        public EngineOptionNumberImpl()
        {
        }

        public EngineOptionNumberImpl(long value)
        {
            this.m_value_ = value;
            this.m_default_ = value;
        }

        public EngineOptionNumberImpl(long value, long defaultValue)
        {
            this.m_value_ = value;
            this.m_default_ = defaultValue;
        }

        /// <summary>
        /// 既定値
        /// </summary>
        private long m_default_;
        public long Default
        {
            get { return this.m_default_; }
            set { this.m_default_ = value; }
        }

        /// <summary>
        /// 現在値
        /// </summary>
        private long m_value_;
        public long Value
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
            throw new ApplicationException("型変換エラー");
        }

        /// <summary>
        /// 数値型でのみ使用可能。数値型でない場合、エラー。
        /// </summary>
        /// <returns></returns>
        public long GetNumber()
        {
            return this.m_value_;
        }

        /// <summary>
        /// 現在値（文字列読取）
        /// </summary>
        /// <param name="value"></param>
        public void ParseValue(string value)
        {
            long result;
            if (long.TryParse(value, out result))
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
