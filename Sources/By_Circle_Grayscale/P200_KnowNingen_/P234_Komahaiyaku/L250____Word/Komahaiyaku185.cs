﻿
namespace Grayscale.P234_Komahaiyaku.L250____Word
{


    /// <summary>
    /// わたしが調べたところ、リーガルムーブは１８４パターンあるぜ☆
    /// 
    /// 配役の列挙型だぜ☆
    /// 
    /// </summary>
    public enum Komahaiyaku185
    {
        n000_未設定 = 0,
        n001_歩 = 1,
        n002_香,
        n003_左端金桂,
        n004_左端銀桂,
        n005_左端擦金桂,
        n006_左端擦銀桂,
        n007_金桂,
        n008_銀桂,
        n009_擦金桂,
        n010_擦銀桂,
        n011_右端金桂,
        n012_右端銀桂,
        n013_右端擦金桂,
        n014_右端擦銀桂,
        n015_左極銀,
        n016_左端奇銀,
        n017_左端偶銀,
        n018_左穴銀,
        n019_天奇銀,
        n020_天偶銀,
        n021_奇銀,
        n022_偶銀,
        n023_底奇銀,
        n024_底偶銀,
        n025_右穴銀,
        n026_右端奇銀,
        n027_右端偶銀,
        n028_右極銀,
        n029_左極金,
        n030_左端奇金,
        n031_左端偶金,
        n032_左穴金,
        n033_天奇金,
        n034_天偶金,
        n035_奇金,
        n036_偶金,
        n037_底奇金,
        n038_底偶金,
        n039_右穴金,
        n040_右端奇金,
        n041_右端偶金,
        n042_右極金,
        n043_左極王,
        n044_左端奇王,
        n045_左端偶王,
        n046_左穴王,
        n047_天奇王,
        n048_天偶王,
        n049_奇王,
        n050_偶王,
        n051_底奇王,
        n052_底偶王,
        n053_右穴王,
        n054_右端奇王,
        n055_右端偶王,
        n056_右極王,
        n057_左極飛,
        n058_左端飛,
        n059_左穴飛,
        n060_天飛,
        n061_飛,
        n062_底飛,
        n063_右穴飛,
        n064_右端飛,
        n065_右極飛,
        n066_左極角,
        n067_左端奇角,
        n068_左端偶角,
        n069_左穴角,
        n070_天奇角,
        n071_天偶角,
        n072_奇角,
        n073_偶角,
        n074_底奇角,
        n075_底偶角,
        n076_右穴角,
        n077_右端奇角,
        n078_右端偶角,
        n079_右極角,
        n080_左極竜,
        n081_左端奇竜,
        n082_左端偶竜,
        n083_左穴竜,
        n084_天奇竜,
        n085_天偶竜,
        n086_奇竜,
        n087_偶竜,
        n088_底奇竜,
        n089_底偶竜,
        n090_右穴竜,
        n091_右端奇竜,
        n092_右端偶竜,
        n093_右極竜,
        n094_左極馬,
        n095_左端奇馬,
        n096_左端偶馬,
        n097_左穴馬,
        n098_天奇馬,
        n099_天偶馬,
        n100_奇馬,
        n101_偶馬,
        n102_底奇馬,
        n103_底偶馬,
        n104_右穴馬,
        n105_右端奇馬,
        n106_右端偶馬,
        n107_右極馬,

        n108_左極と,
        n109_左端奇と,
        n110_左端偶と,
        n111_左穴と,
        n112_天奇と,
        n113_天偶と,
        n114_奇と,
        n115_偶と,
        n116_底奇と,
        n117_底偶と,
        n118_右穴と,
        n119_右端奇と,
        n120_右端偶と,
        n121_右極と,
        n122_左極杏,
        n123_左端奇杏,
        n124_左端偶杏,
        n125_左穴杏,
        n126_天奇杏,
        n127_天偶杏,
        n128_奇杏,
        n129_偶杏,
        n130_底奇杏,
        n131_底偶杏,
        n132_右穴杏,
        n133_右端奇杏,
        n134_右端偶杏,
        n135_右極杏,
        n136_左極圭,
        n137_左端奇圭,
        n138_左端偶圭,
        n139_左穴圭,
        n140_天奇圭,
        n141_天偶圭,
        n142_奇圭,
        n143_偶圭,
        n144_底奇圭,
        n145_底偶圭,
        n146_右穴圭,
        n147_右端奇圭,
        n148_右端偶圭,
        n149_右極圭,
        n150_左極全,
        n151_左端奇全,
        n152_左端偶全,
        n153_左穴全,
        n154_天奇全,
        n155_天偶全,
        n156_奇全,
        n157_偶全,
        n158_底奇全,
        n159_底偶全,
        n160_右穴全,
        n161_右端奇全,
        n162_右端偶全,
        n163_右極全,
        n164_歩打,
        n165_香打,
        n166_桂打,
        n167_銀打,
        n168_金打,
        n169_王打,
        n170_飛打,
        n171_角打,
        n172_駒袋歩,
        n173_駒袋香,
        n174_駒袋桂,
        n175_駒袋銀,
        n176_駒袋金,
        n177_駒袋王,
        n178_駒袋飛,
        n179_駒袋角,
        n180_駒袋竜,
        n181_駒袋馬,
        n182_駒袋と金,
        n183_駒袋杏,
        n184_駒袋圭,
        n185_駒袋全


    }
}
