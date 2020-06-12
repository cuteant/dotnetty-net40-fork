﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DotNetty.Buffers
{
    using System;
    using DotNetty.Common;

    public class DefaultByteBufferHolder : IByteBufferHolder, IEquatable<IByteBufferHolder>
    {
        private readonly IByteBuffer _data;

        public DefaultByteBufferHolder(IByteBuffer data)
        {
            if (data is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.data); }

            _data = data;
        }

        public IByteBuffer Content
        {
            get
            {
                var refCnt = _data.ReferenceCount;
                if ((uint)(refCnt - 1) > SharedConstants.TooBigOrNegative) // <= 0
                {
                    ThrowHelper.ThrowIllegalReferenceCountException(refCnt);
                }

                return _data;
            }
        }

        public virtual IByteBufferHolder Copy() => Replace(_data.Copy());

        public virtual IByteBufferHolder Duplicate() => Replace(_data.Duplicate());

        public virtual IByteBufferHolder RetainedDuplicate() => Replace(_data.RetainedDuplicate());

        public virtual IByteBufferHolder Replace(IByteBuffer content) => new DefaultByteBufferHolder(content);

        public virtual int ReferenceCount => _data.ReferenceCount;

        public IReferenceCounted Retain()
        {
            _data.Retain();
            return this;
        }

        public IReferenceCounted Retain(int increment)
        {
            _data.Retain(increment);
            return this;
        }

        public IReferenceCounted Touch()
        {
            _data.Touch();
            return this;
        }

        public IReferenceCounted Touch(object hint)
        {
            _data.Touch(hint);
            return this;
        }

        public bool Release() => _data.Release();

        public bool Release(int decrement) => _data.Release(decrement);

        protected string ContentToString() => _data.ToString();

        /// <summary>
        /// This implementation of the <see cref="Equals(object)"/> operation is restricted to
        /// work only with instances of the same class. The reason for that is that
        /// Netty library already has a number of classes that extend <see cref="DefaultByteBufferHolder"/> and
        /// override <see cref="Equals(object)"/> method with an additional comparison logic and we
        /// need the symmetric property of the <see cref="Equals(object)"/> operation to be preserved.
        /// </summary>
        /// <param name="obj">the reference object with which to compare.</param>
        /// <returns><c>true</c> if this object is the same as the <paramref name="obj"/>
        /// argument; <c>false</c> otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) { return true; }

            return obj is object && GetType() == obj.GetType() && _data.Equals(((DefaultByteBufferHolder)obj)._data);
        }

        /// <summary>
        /// This implementation of the <see cref="Equals(IByteBufferHolder)"/> operation is restricted to
        /// work only with instances of the same class. The reason for that is that
        /// Netty library already has a number of classes that extend <see cref="DefaultByteBufferHolder"/> and
        /// override <see cref="Equals(IByteBufferHolder)"/> method with an additional comparison logic and we
        /// need the symmetric property of the <see cref="Equals(IByteBufferHolder)"/> operation to be preserved.
        /// </summary>
        /// <param name="other">the reference object with which to compare.</param>
        /// <returns><c>true</c> if this object is the same as the <paramref name="other"/>
        /// argument; <c>false</c> otherwise.</returns>
        public bool Equals(IByteBufferHolder other) => Equals(obj: other);

        public override int GetHashCode() => _data.GetHashCode();
    }
}
