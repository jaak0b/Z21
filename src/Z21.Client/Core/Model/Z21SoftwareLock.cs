namespace Z21.Core.Model
{
  public static class Z21SoftwareLock
  {
    /// <summary>
    /// No feature locked.
    /// </summary>
    public const int NoLock = 0x00;

    /// <summary>
    /// z21 start: Driving and switching per lan locked.
    /// </summary>
    public const int Z21StartLocked = 0x01;
    
    /// <summary>
    /// z21 start: no feature locked.
    /// </summary>
    public const int Z21StartUnlocked = 0x02;
  }
}