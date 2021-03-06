﻿/*
 * Copyright 2012 The Netty Project
 *
 * The Netty Project licenses this file to you under the Apache License,
 * version 2.0 (the "License"); you may not use this file except in compliance
 * with the License. You may obtain a copy of the License at:
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations
 * under the License.
 *
 * Copyright (c) Microsoft. All rights reserved.
 * Licensed under the MIT license. See LICENSE file in the project root for full license information.
 *
 * Copyright (c) 2020 The Dotnetty-Span-Fork Project (cuteant@outlook.com)
 *
 *   https://github.com/cuteant/dotnetty-span-fork
 *
 * Licensed under the MIT license. See LICENSE file in the project root for full license information.
 */

namespace DotNetty.Buffers
{
    using System;
    using System.Buffers;

    partial class PooledSlicedByteBuffer
    {
        public sealed override ReadOnlyMemory<byte> GetReadableMemory(int index, int count)
        {
            CheckIndex0(index, count);
            return Unwrap().GetReadableMemory(Idx(index), count);
        }
        protected internal sealed override ReadOnlyMemory<byte> _GetReadableMemory(int index, int count)
        {
            return UnwrapCore()._GetReadableMemory(Idx(index), count);
        }

        public sealed override ReadOnlySpan<byte> GetReadableSpan(int index, int count)
        {
            CheckIndex0(index, count);
            return Unwrap().GetReadableSpan(Idx(index), count);
        }
        protected internal sealed override ReadOnlySpan<byte> _GetReadableSpan(int index, int count)
        {
            return UnwrapCore()._GetReadableSpan(Idx(index), count);
        }

        public sealed override ReadOnlySequence<byte> GetSequence(int index, int count)
        {
            CheckIndex0(index, count);
            return Unwrap().GetSequence(Idx(index), count);
        }

        protected internal sealed override ReadOnlySequence<byte> _GetSequence(int index, int count)
        {
            return UnwrapCore()._GetSequence(Idx(index), count);
        }

        public sealed override Memory<byte> GetMemory(int index, int count)
        {
            CheckIndex0(index, count);
            return Unwrap().GetMemory(Idx(index), count);
        }
        protected internal sealed override Memory<byte> _GetMemory(int index, int count)
        {
            return UnwrapCore()._GetMemory(Idx(index), count);
        }

        public sealed override Span<byte> GetSpan(int index, int count)
        {
            CheckIndex0(index, count);
            return Unwrap().GetSpan(Idx(index), count);
        }
        protected internal sealed override Span<byte> _GetSpan(int index, int count)
        {
            return UnwrapCore()._GetSpan(Idx(index), count);
        }

        protected internal sealed override void _GetBytes(int index, Memory<byte> destination, int length)
        {
            CheckIndex0(index, length);
            UnwrapCore()._GetBytes(Idx(index), destination, length);
        }

        protected internal sealed override void _GetBytes(int index, Span<byte> destination, int length)
        {
            CheckIndex0(index, length);
            UnwrapCore()._GetBytes(Idx(index), destination, length);
        }

        public sealed override IByteBuffer SetBytes(int index, in ReadOnlyMemory<byte> src)
        {
            CheckIndex0(index, src.Length);
            _ = Unwrap().SetBytes(Idx(index), src);
            return this;
        }

        public sealed override IByteBuffer SetBytes(int index, in ReadOnlySpan<byte> src)
        {
            CheckIndex0(index, src.Length);
            _ = Unwrap().SetBytes(Idx(index), src);
            return this;
        }
    }
}
