// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Microsoft.DiaSymReader
{
    internal static class EmptyArray<T>
    {
        public static readonly T[] Instance = new T[0];
    }

    internal class InteropUtilities
    {
        internal const int S_OK = 0x0;
        internal const int S_FALSE = 0x1;
        internal const int E_FAIL = unchecked((int)0x80004005);
        internal const int E_NOTIMPL = unchecked((int)0x80004001);

        private static readonly IntPtr s_ignoreIErrorInfo = new IntPtr(-1);

        internal static T[] NullToEmpty<T>(T[] items) => (items == null) ? EmptyArray<T>.Instance : items;

        internal static void ThrowExceptionForHR(int hr)
        {
            // E_FAIL indicates "no info".
            // E_NOTIMPL indicates a lack of ISymUnmanagedReader support (in a particular implementation).
            if (hr < 0 && hr != E_FAIL && hr != E_NOTIMPL)
            {
                Marshal.ThrowExceptionForHR(hr, s_ignoreIErrorInfo);
            }
        }

        // PERF: The purpose of all this code duplication is to avoid allocating any display class instances.
        // Effectively, we will use the stack frames themselves as display classes.
        internal delegate int CountGetter<TEntity>(TEntity entity, out int count);
        internal delegate int ItemsGetter<TEntity, TItem>(TEntity entity, int bufferLength, out int count, TItem[] buffer);
        internal delegate int ItemsGetter<TEntity, TArg1, TItem>(TEntity entity, TArg1 arg1, int bufferLength, out int count, TItem[] buffer);
        internal delegate int ItemsGetter<TEntity, TArg1, TArg2, TItem>(TEntity entity, TArg1 arg1, TArg2 arg2, int bufferLength, out int count, TItem[] buffer);

        internal static string BufferToString(char[] buffer)
        {
            Debug.Assert(buffer[buffer.Length - 1] == 0);
            return new string(buffer, 0, buffer.Length - 1);
        }

        internal static void ValidateItems(int actualCount, int bufferLength)
        {
            if (actualCount != bufferLength)
            {
                // TODO: localize
                throw new InvalidOperationException(string.Format("Read only {0} of {1} items.", actualCount, bufferLength));
            }
        }

        internal static TItem[] GetItems<TEntity, TItem>(TEntity entity, CountGetter<TEntity> countGetter, ItemsGetter<TEntity, TItem> itemsGetter)
        {
            int count;
            ThrowExceptionForHR(countGetter(entity, out count));
            if (count == 0)
            {
                return null;
            }

            var result = new TItem[count];
            ThrowExceptionForHR(itemsGetter(entity, count, out count, result));
            ValidateItems(count, result.Length);
            return result;
        }

        internal static TItem[] GetItems<TEntity, TItem>(TEntity entity, ItemsGetter<TEntity, TItem> getter)
        {
            int count;
            ThrowExceptionForHR(getter(entity, 0, out count, null));
            if (count == 0)
            {
                return null;
            }

            var result = new TItem[count];
            ThrowExceptionForHR(getter(entity, count, out count, result));
            ValidateItems(count, result.Length);
            return result;
        }

        internal static TItem[] GetItems<TEntity, TArg1, TItem>(TEntity entity, TArg1 arg1, ItemsGetter<TEntity, TArg1, TItem> getter)
        {
            int count;
            ThrowExceptionForHR(getter(entity, arg1, 0, out count, null));
            if (count == 0)
            {
                return null;
            }

            var result = new TItem[count];
            ThrowExceptionForHR(getter(entity, arg1, count, out count, result));
            ValidateItems(count, result.Length);
            return result;
        }

        internal static TItem[] GetItems<TEntity, TArg1, TArg2, TItem>(TEntity entity, TArg1 arg1, TArg2 arg2, ItemsGetter<TEntity, TArg1, TArg2, TItem> getter)
        {
            int count;
            ThrowExceptionForHR(getter(entity, arg1, arg2, 0, out count, null));
            if (count == 0)
            {
                return null;
            }

            var result = new TItem[count];
            ThrowExceptionForHR(getter(entity, arg1, arg2, count, out count, result));
            ValidateItems(count, result.Length);
            return result;
        }
    }
}
