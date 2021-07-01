# Kifuwarabe_DenouT4

2020年11月の 電竜戦から きふわらぎょく(Kifuwaragyoku)にリネームして開発再開だぜ☆（＾～＾）  

|                         | ファイル                                                                                      |
| ----------------------- | --------------------------------------------------------------------------------------------- |
| ソース                  | `Kifuwarabe_DenouT4/Sources/By_Circle_Grayscale/Kifuwaragyoku.sln`                            |
| 将棋エンジン ソース     | A500ShogiEngine                                                                               |
| 将棋GUI ソース          | P800GuiCshapeVs                                                                               |
| 将棋エンジン ランタイム | `Kifuwarabe_DenouT4/Engine01_Binaries/A500_ShogiEngine/Grayscale.A500_ShogiEngine.exe`        |
| 設定ファイル1           | `Kifuwarabe_DenouT4/Engine01_Binaries/A500_ShogiEngine/Grayscale.A500_ShogiEngine.exe.config` |
| 設定ファイル2           | `Kifuwarabe_DenouT4/Profile/Engine.toml`                                                      |

* `Kifuwarabe_WCSC25` のトップ・ディレクトリーに `Logs` ディレクトリーを作成してください。
* `Kifuwaragyoku.sln` を `Release` モードで ビルドしてください。
* 設定ファイル1 の `Grayscale.A500_ShogiEngine.exe.config` の中にある `Profile` のパスを、 設定ファイル2 の親ディレクトリー `Profile` に合わせてください。  

## Manual

Debug でも Release でもなく、 LEARN モードでしか動かない☆（＾～＾）？  
将棋所の連続対局で ２局目の isready に反応しない☆（＾～＾）？  

## Debug

`go` に返信しない？  

```plain
>2:usi
<2:option name 子 type check default true
<2:option name USI type spin default 2 min 1 max 13
<2:option name 寅 type combo default tiger var マウス var うし var tiger var ウー var 龍 var へび var 馬 var ひつじ var モンキー var バード var ドッグ var うりぼー
<2:option name 卯 type button default うさぎ
<2:option name 辰 type string default DRAGON
<2:option name 巳 type filename default スネーク.html
<2:id name Kifuwaragyoku Bld4
<2:id author Satoshi TAKAHASHI
<2:usiok
>2:setoption name USI_Ponder value true
>2:setoption name USI_Hash value 256
>2:setoption name 子 value true
>2:setoption name USI value 2
>2:setoption name 寅 value tiger
>2:setoption name 辰 value DRAGON
>2:setoption name 巳 value スネーク.html
>2:isready
<2:readyok
>2:usinewgame
>2:position startpos moves 2g2f
>2:go btime 298000 wtime 300000 binc 2000 winc 2000
```
