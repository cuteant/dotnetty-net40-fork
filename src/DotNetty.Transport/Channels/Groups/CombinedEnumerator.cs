﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DotNetty.Transport.Channels.Groups
{
    using System.Collections;
    using System.Collections.Generic;

    public sealed class CombinedEnumerator<T> : IEnumerator<T>
    {
        private readonly IEnumerator<T> _e1;
        private readonly IEnumerator<T> _e2;
        private IEnumerator<T> _currentEnumerator;

        public CombinedEnumerator(IEnumerator<T> e1, IEnumerator<T> e2)
        {
            if (e1 is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.e1); }
            if (e2 is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.e2); }
            _e1 = e1;
            _e2 = e2;
            _currentEnumerator = e1;
        }

        public T Current => _currentEnumerator.Current;

        public void Dispose() => _currentEnumerator.Dispose();

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            while (true)
            {
                if (_currentEnumerator.MoveNext())
                {
                    return true;
                }
                if (_currentEnumerator == _e1)
                {
                    _currentEnumerator = _e2;
                }
                else
                {
                    return false;
                }
            }
        }

        public void Reset() => _currentEnumerator.Reset();
    }
}