using System;

namespace Microsoft.DiaSymReader
{
    public struct SymUnmanagedAsyncStepInfo : IEquatable<SymUnmanagedAsyncStepInfo>
    {
        public int YieldOffset { get; }
        public int ResumeOffset { get; }
        public int ResumeMethod { get; }

        public SymUnmanagedAsyncStepInfo(int yieldOffset, int resumeOffset, int resumeMethod)
        {
            YieldOffset = yieldOffset;
            ResumeOffset = resumeOffset;
            ResumeMethod = resumeMethod;
        }

        public override bool Equals(object obj)
        {
            return obj is SymUnmanagedAsyncStepInfo && Equals((SymUnmanagedAsyncStepInfo)obj);
        }

        public bool Equals(SymUnmanagedAsyncStepInfo other)
        {
            return YieldOffset == other.YieldOffset
                && ResumeMethod == other.ResumeMethod
                && ResumeOffset == other.ResumeOffset;
        }

        public override int GetHashCode()
        {
            return YieldOffset ^ ResumeMethod ^ ResumeOffset;
        }
    }

}
