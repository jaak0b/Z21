// ReSharper disable InconsistentNaming

namespace Z21.Core.Model
{
  public static class Z21HardwareType
  {
    /// <summary>
    /// Black Z21. Hardware version till 2012.
    /// </summary>
    public const int Z21Old = 0x00000200;

    /// <summary>
    /// Black Z21. Hardware version beginning 2013.
    /// </summary>
    public const int Z21New = 0x00000201;

    /// <summary>
    /// Smartrail. Starting 2012.
    /// </summary>
    public const int Smartrail = 0x00000202;

    /// <summary>
    /// white z21. Starterset variant starting 2013.
    /// </summary>
    public const int z21Small = 0x00000203;

    /// <summary>
    /// z21 start. Starterset variant starting 2016.
    /// </summary>
    public const int z21Start = 0x00000204;

    /// <summary>
    /// 10806 „Z21 Single Booster” (zLink)
    /// </summary>
    public const int SingleBooster = 0x00000205;

    /// <summary>
    /// 10807 „Z21 Dual Booster” (zLink)
    /// </summary>
    public const int DualBooster = 0x00000206;

    /// <summary>
    /// 10870 „Z21 XL Series” (starting 2020)
    /// </summary>
    public const int Z21Xl = 0x00000211;

    /// <summary>
    /// 10869 „Z21 XL Booster” (starting 2021, zLink)
    /// </summary>
    public const int XlBooster = 0x00000212;

    /// <summary>
    /// 10836 „Z21 SwitchDecoder” (zLink)
    /// </summary>
    public const int Z21SwitchDecoder = 0x00000301;

    /// <summary>
    /// 10836 „Z21 SignalDecoder” (zLink)
    /// </summary>
    public const int Z21SignalDecoder = 0x00000302;
  }
}