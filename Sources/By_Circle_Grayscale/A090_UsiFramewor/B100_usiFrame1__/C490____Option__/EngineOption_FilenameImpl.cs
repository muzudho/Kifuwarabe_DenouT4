﻿using Grayscale.A090_UsiFramewor.B100_usiFrame1__.C___490_Option__;

namespace Grayscale.A090_UsiFramewor.B100_usiFrame1__.C490____Option__
{
    public class EngineOption_FilenameImpl : EngineOption_StringImpl, EngineOption_Filename
    {
        public EngineOption_FilenameImpl()
        {
            this.m_value_ = "";
            this.m_default_ = "";
        }

        /// <summary>
        /// 最初に入ってきた値を、既定値とします。
        /// </summary>
        /// <param name="value"></param>
        public EngineOption_FilenameImpl(string value)
        {
            this.m_value_ = value;
            this.m_default_ = value;
        }

        public EngineOption_FilenameImpl(string value, string defaultValue)
        {
            this.m_value_ = value;
            this.m_default_ = defaultValue;
        }

    }
}