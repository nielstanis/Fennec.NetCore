namespace Fennec.NetCore.Patch
{
    public class Patcher
    {
        public void ThrowNotSupported()
        {
            throw new System.NotSupportedException("Fennec.NetCore NotSupported");
        }
    }
}