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

using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using PingenApiNet.Abstractions.Exceptions;
using PingenApiNet.Abstractions.Models.Webhooks.WebhookEvents;

namespace PingenApiNet.Abstractions.Helpers;

/// <summary>
///
/// </summary>
public static class PingenWebhookHelper
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="signingKey"></param>
    /// <param name="signature"></param>
    /// <param name="requestStream"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="PingenWebhookValidationErrorException"></exception>
    public static async Task<WebhookEvent?> ValidateWebhookAndGetData(string signingKey, string signature, Stream requestStream, [Optional] CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(requestStream);
        var payload = await reader.ReadToEndAsync(cancellationToken);

        if (await ValidateWebhook(signingKey, signature, payload, cancellationToken))
        {
            return JsonSerializer.Deserialize<WebhookEvent>(payload);
        }

        throw new PingenWebhookValidationErrorException(
            JsonSerializer.Deserialize<WebhookEvent>(payload) ??
            new WebhookEvent(string.Empty, new(""), DateTime.MinValue),
            "Validation of webhook signature failed");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="signingKey"></param>
    /// <param name="signature"></param>
    /// <param name="payload"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<bool> ValidateWebhook(string signingKey, string signature, string payload, [Optional] CancellationToken cancellationToken)
    {
        using var hash = new HMACSHA256(Encoding.UTF8.GetBytes(signingKey));
        return signature == Convert.ToBase64String(
            await hash.ComputeHashAsync(
                new MemoryStream(Encoding.UTF8.GetBytes(payload)),
                cancellationToken));
    }
}