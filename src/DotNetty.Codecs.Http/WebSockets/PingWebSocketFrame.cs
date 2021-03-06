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
 * Copyright (c) 2020 The Dotnetty-Span-Fork Project (cuteant@outlook.com)
 *
 *   https://github.com/cuteant/dotnetty-span-fork
 *
 * Licensed under the MIT license. See LICENSE file in the project root for full license information.
 */

namespace DotNetty.Codecs.Http.WebSockets
{
    using DotNetty.Buffers;

    /// <summary>
    /// Web Socket frame containing binary data.
    /// </summary>
    public class PingWebSocketFrame : WebSocketFrame
    {
        public PingWebSocketFrame()
            : base(true, 0, Opcode.Ping, ArrayPooled.Buffer(0))
        {
        }

        /// <summary>
        /// Creates a new ping frame with the specified binary data.
        /// </summary>
        /// <param name="binaryData">the content of the frame.</param>
        public PingWebSocketFrame(IByteBuffer binaryData)
            : base(true, 0, Opcode.Ping, binaryData)
        {
        }

        /// <summary>
        /// Creates a new ping frame with the specified binary data.
        /// </summary>
        /// <param name="finalFragment">flag indicating if this frame is the final fragment</param>
        /// <param name="rsv">reserved bits used for protocol extensions</param>
        /// <param name="binaryData">the content of the frame.</param>
        public PingWebSocketFrame(bool finalFragment, int rsv, IByteBuffer binaryData)
            : base(finalFragment, rsv, Opcode.Ping, binaryData)
        {
        }

        /// <inheritdoc />
        public override IByteBufferHolder Replace(IByteBuffer content) => new PingWebSocketFrame(this.IsFinalFragment, this.Rsv, content);
    }
}
