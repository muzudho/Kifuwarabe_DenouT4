﻿using Grayscale.P571_usiFrame1__.L___490_Option__;

namespace Grayscale.P571_usiFrame1__.L490____Option__
{
    public class EngineOption_ButtonImpl : EngineOption_StringImpl, EngineOption_Button
    {
        public EngineOption_ButtonImpl()
        {
            this.m_value_ = "";
            this.m_default_ = "";
        }

        /// <summary>
        /// 最初に入ってきた値を、既定値とします。
        /// </summary>
        /// <param name="value"></param>
        public EngineOption_ButtonImpl(string value)
        {
            this.m_value_ = value;
            this.m_default_ = value;
        }

        public EngineOption_ButtonImpl(string value, string defaultValue)
        {
            this.m_value_ = value;
            this.m_default_ = defaultValue;
        }

    }
}
