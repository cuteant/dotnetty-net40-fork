﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DotNetty.Codecs
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using DotNetty.Buffers;
    using DotNetty.Transport.Channels;
    using DotNetty.Transport.Channels.Sockets;

    public class DatagramPacketEncoder2<T> : MessageToMessageEncoder2<IAddressedEnvelope<T>>
        where T : class
    {
        readonly MessageToMessageEncoder2<T> encoder;

        public DatagramPacketEncoder2(MessageToMessageEncoder2<T> encoder)
        {
            if (null == encoder) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.encoder); }

            this.encoder = encoder;
        }

        public override bool TryAcceptOutboundMessage(object msg, out IAddressedEnvelope<T> envelope)
        {
            envelope = msg as IAddressedEnvelope<T>;
            return envelope != null
                && this.encoder.TryAcceptOutboundMessage(envelope.Content, out _)
                && (envelope.Sender != null || envelope.Recipient != null);
        }

        protected internal override void Encode(IChannelHandlerContext context, IAddressedEnvelope<T> message, List<object> output)
        {
            this.encoder.Encode(context, message.Content, output);
            if (output.Count != 1)
            {
                ThrowHelper.ThrowEncoderException_MustProduceOnlyOneMsg(this.encoder.GetType());
            }

            var content = output[0] as IByteBuffer;
            if (content == null)
            {
                ThrowHelper.ThrowEncoderException_MustProduceOnlyByteBuf(this.encoder.GetType());
            }

            // Replace the ByteBuf with a DatagramPacket.
            output[0] = new DatagramPacket(content, message.Sender, message.Recipient);
        }

        public override Task BindAsync(IChannelHandlerContext context, EndPoint localAddress) =>
            this.encoder.BindAsync(context, localAddress);

        public override Task ConnectAsync(IChannelHandlerContext context, EndPoint remoteAddress, EndPoint localAddress) =>
            this.encoder.ConnectAsync(context, remoteAddress, localAddress);

        public override Task DisconnectAsync(IChannelHandlerContext context) => this.encoder.DisconnectAsync(context);

        public override Task CloseAsync(IChannelHandlerContext context) => this.encoder.CloseAsync(context);

        public override Task DeregisterAsync(IChannelHandlerContext context) => this.encoder.DeregisterAsync(context);

        public override void Read(IChannelHandlerContext context) => this.encoder.Read(context);

        public override void Flush(IChannelHandlerContext context) => this.encoder.Flush(context);

        public override void HandlerAdded(IChannelHandlerContext context) => this.encoder.HandlerAdded(context);

        public override void HandlerRemoved(IChannelHandlerContext context) => this.encoder.HandlerRemoved(context);

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception) =>
            this.encoder.ExceptionCaught(context, exception);
    }
}
