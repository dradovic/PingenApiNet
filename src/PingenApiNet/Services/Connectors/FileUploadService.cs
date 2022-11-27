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
using PingenApiNet.Abstractions.Enums;
using PingenApiNet.Abstractions.Models.API;
using PingenApiNet.Abstractions.Models.Data;
using PingenApiNet.Abstractions.Models.FileUpload;
using PingenApiNet.Interfaces;
using PingenApiNet.Interfaces.Connectors;
using PingenApiNet.Services.Connectors.Base;

namespace PingenApiNet.Services.Connectors;

/// <summary>
///
/// </summary>
public sealed class FileUploadService : ConnectorService, IFileUploadService
{
    /// <summary>
    /// Pingen connection handler
    /// </summary>
    private readonly IPingenConnectionHandler _pingenConnectionHandler;

    /// <summary>
    /// Inject connection handler at construction
    /// </summary>
    /// <param name="pingenConnectionHandler"></param>
    public FileUploadService(IPingenConnectionHandler pingenConnectionHandler)
    {
        _pingenConnectionHandler = pingenConnectionHandler;
    }

    /// <inheritdoc />
    public async Task<ApiResult<SingleResult<FileUploadData>>> GetPath([Optional] CancellationToken cancellationToken)
    {
        return await _pingenConnectionHandler.GetAsync<SingleResult<FileUploadData>>(requestPath: "file-upload", cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> UploadFile(FileUploadData fileUploadData, MemoryStream data, [Optional] CancellationToken cancellationToken)
    {
        using var httpClient = new HttpClient();
        return (await httpClient.PutAsync(fileUploadData.Attributes.Url, new ByteArrayContent(data.ToArray()), cancellationToken)).IsSuccessStatusCode;
    }
}
