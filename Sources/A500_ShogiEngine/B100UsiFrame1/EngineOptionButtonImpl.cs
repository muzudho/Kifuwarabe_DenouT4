using Grayscale.A090UsiFramewor.B100UsiFrame1.C490Option;

namespace Grayscale.A090UsiFramewor.B100UsiFrame1.C490Option
{
    public class EngineOptionButtonImpl : EngineOptionStringImpl, IEngineOptionButton
    {
        public EngineOptionButtonImpl()
        {
            this.m_value_ = "";
            this.m_default_ = "";
        }

        /// <summary>
        /// 最初に入ってきた値を、既定値とします。
        /// </summary>
        /// <param name="value"></param>
        public EngineOptionButtonImpl(string value)
        {
            this.m_value_ = value;
            this.m_default_ = value;
        }

        public EngineOptionButtonImpl(string value, string defaultValue)
        {
            this.m_value_ = value;
            this.m_default_ = defaultValue;
        }

        public override string ToString()
        {
            return "value " + this.m_value_;
        }
    }
}
