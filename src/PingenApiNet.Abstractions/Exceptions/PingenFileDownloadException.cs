﻿/*
MIT License

Copyright (c) 2022 Philip Näf <philip.naef@amanda-technology.ch>
Copyright (c) 2022 Manuel Gysin <manuel.gysin@amanda-technology.ch>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Runtime.Serialization;

namespace PingenApiNet.Abstractions.Exceptions;

/// <summary>
/// Represents errors that occur from pingen file download
/// </summary>
[Serializable]
public class PingenFileDownloadException : Exception
{
    /// <summary>
    /// Error code
    /// </summary>
    public string? ErrorCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PingenFileDownloadException"/> class
    /// </summary>
    /// <param name="errorCode"></param>
    public PingenFileDownloadException(string? errorCode)
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PingenApiErrorException"/> class
    /// </summary>
    /// <param name="errorCode"></param>
    /// <param name="message"></param>
    public PingenFileDownloadException(string? errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PingenApiErrorException"/> class
    /// </summary>
    /// <param name="errorCode"></param>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public PingenFileDownloadException(string? errorCode, string message, Exception inner) : base(message, inner)
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PingenApiErrorException"/> class
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected PingenFileDownloadException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        ErrorCode = info.GetValue(nameof(ErrorCode), typeof(string)) as string;
    }

    /// <inheritdoc />
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(ErrorCode), ErrorCode);
    }
}