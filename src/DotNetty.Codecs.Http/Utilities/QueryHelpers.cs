﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using CuteAnt.Pool;
using Microsoft.Extensions.Primitives;

namespace DotNetty.Codecs.Http.Utilities
{
    public static class QueryHelpers
    {
        static readonly TextEncoder s_urlEncoder = System.Text.Encodings.Web.UrlEncoder.Default;

        /// <summary>
        /// Append the given query key and value to the URI.
        /// </summary>
        /// <param name="uri">The base URI.</param>
        /// <param name="name">The name of the query key.</param>
        /// <param name="value">The query value.</param>
        /// <returns>The combined result.</returns>
        public static string AddQueryString(string uri, string name, string value)
        {
            if (uri == null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.uri); }
            if (name == null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.name); }
            if (value == null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value); }

            return AddQueryString(uri, new[] { new KeyValuePair<string, string>(name, value) });
        }

        /// <summary>
        /// Append the given query keys and values to the uri.
        /// </summary>
        /// <param name="uri">The base uri.</param>
        /// <param name="queryString">A collection of name value query pairs to append.</param>
        /// <returns>The combined result.</returns>
        public static string AddQueryString(string uri, IDictionary<string, string> queryString)
        {
            if (uri == null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.uri); }
            if (queryString == null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.queryString); }

            return AddQueryString(uri, queryParams:queryString);
        }

        private static string AddQueryString(string uri, IEnumerable<KeyValuePair<string, string>> queryParams)
        {
            //if (uri == null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.uri); }
            //if (queryString == null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.queryString); }

            var anchorIndex = uri.IndexOf('#');
            var uriToBeAppended = uri;
            var anchorText = "";
            // If there is an anchor, then the query string must be inserted before its first occurance.
            if (anchorIndex != -1)
            {
                anchorText = uri.Substring(anchorIndex);
                uriToBeAppended = uri.Substring(0, anchorIndex);
            }

            var queryIndex = uriToBeAppended.IndexOf('?');
            var hasQuery = queryIndex != -1;

            var sb = StringBuilderManager.Allocate();
            sb.Append(uriToBeAppended);
            foreach (var parameter in queryParams)
            {
                sb.Append(hasQuery ? '&' : '?');
                sb.Append(s_urlEncoder.Encode(parameter.Key));
                sb.Append('=');
                sb.Append(s_urlEncoder.Encode(parameter.Value));
                hasQuery = true;
            }

            sb.Append(anchorText);
            return StringBuilderManager.ReturnAndFree(sb);
        }

        /// <summary>
        /// Parse a query string into its component key and value parts.
        /// </summary>
        /// <param name="queryString">The raw query string value, with or without the leading '?'.</param>
        /// <returns>A collection of parsed keys and values.</returns>
        public static Dictionary<string, StringValues> ParseQuery(string queryString)
        {
            var result = ParseNullableQuery(queryString);

            if (result == null)
            {
                return new Dictionary<string, StringValues>();
            }

            return result;
        }


        /// <summary>
        /// Parse a query string into its component key and value parts.
        /// </summary>
        /// <param name="queryString">The raw query string value, with or without the leading '?'.</param>
        /// <returns>A collection of parsed keys and values, null if there are no entries.</returns>
        public static Dictionary<string, StringValues> ParseNullableQuery(string queryString)
        {
            var accumulator = new KeyValueAccumulator();

            if (string.IsNullOrEmpty(queryString) || queryString == "?")
            {
                return null;
            }

            int scanIndex = 0;
            if (queryString[0] == '?')
            {
                scanIndex = 1;
            }

            int textLength = queryString.Length;
            int equalIndex = queryString.IndexOf('=');
            if (equalIndex == -1)
            {
                equalIndex = textLength;
            }
            while (scanIndex < textLength)
            {
                int delimiterIndex = queryString.IndexOf('&', scanIndex);
                if (delimiterIndex == -1)
                {
                    delimiterIndex = textLength;
                }
                if (equalIndex < delimiterIndex)
                {
                    while (scanIndex != equalIndex && char.IsWhiteSpace(queryString[scanIndex]))
                    {
                        ++scanIndex;
                    }
                    string name = queryString.Substring(scanIndex, equalIndex - scanIndex);
                    string value = queryString.Substring(equalIndex + 1, delimiterIndex - equalIndex - 1);
                    accumulator.Append(
                        Uri.UnescapeDataString(name.Replace('+', ' ')),
                        Uri.UnescapeDataString(value.Replace('+', ' ')));
                    equalIndex = queryString.IndexOf('=', delimiterIndex);
                    if (equalIndex == -1)
                    {
                        equalIndex = textLength;
                    }
                }
                else
                {
                    if (delimiterIndex > scanIndex)
                    {
                        accumulator.Append(queryString.Substring(scanIndex, delimiterIndex - scanIndex), string.Empty);
                    }
                }
                scanIndex = delimiterIndex + 1;
            }

            if (!accumulator.HasValues)
            {
                return null;
            }

            return accumulator.GetResults();
        }
    }
}