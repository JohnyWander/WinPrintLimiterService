#include "pch.h"

#include "OxpsPrintWrapper.h"
#include <Windows.h>
#include <xpsobjectmodel.h>
#include <XpsPrint.h>
#include <comdef.h>
#include <shlwapi.h>
#include <vcclr.h>
using namespace System::Runtime::InteropServices;

namespace OxpsPrintWrapper {

    void ThrowIfFailed(HRESULT hr)
    {
        if (FAILED(hr))
            throw gcnew System::ComponentModel::Win32Exception(hr);
    }

    void OxpsPrinter::PrintOxps(System::String^ oxpsFilePath, System::String^ printerName)
    {
        pin_ptr<const wchar_t> filePath = PtrToStringChars(oxpsFilePath);
        pin_ptr<const wchar_t> printer = PtrToStringChars(printerName);

        // Open the file
        HANDLE fileHandle = CreateFileW(filePath, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
        if (fileHandle == INVALID_HANDLE_VALUE)
            throw gcnew System::IO::IOException("Failed to open OXPS file.");

        // Create stream from handle
        IStream* inputStream = nullptr;
        HRESULT hr = CreateStreamOnHGlobal(NULL, TRUE, &inputStream);
        ThrowIfFailed(hr);

        DWORD bytesRead;
        const DWORD bufferSize = 4096;
        BYTE buffer[bufferSize];
        while (ReadFile(fileHandle, buffer, bufferSize, &bytesRead, NULL) && bytesRead > 0)
        {
            ULONG written;
            inputStream->Write(buffer, bytesRead, &written);
        }
        SetFilePointer(fileHandle, 0, NULL, FILE_BEGIN);
        CloseHandle(fileHandle);

        // Seek to beginning
        LARGE_INTEGER liZero = {};
        inputStream->Seek(liZero, STREAM_SEEK_SET, NULL);

        // Print using StartXpsPrintJob
        IXpsPrintJob* printJob = nullptr;
        IXpsPrintJobStream* jobStream = nullptr;

        hr = StartXpsPrintJob(
            printer,
            L"Silent OXPS Print Job",
            NULL,
            NULL,
            NULL,
            NULL,
            0,
            &printJob,
            &jobStream,
            NULL
        );
        ThrowIfFailed(hr);

        // Copy stream to print job
        const ULONG copyBufSize = 4096;
        BYTE copyBuf[copyBufSize];
        ULONG read;

        while (SUCCEEDED(inputStream->Read(copyBuf, copyBufSize, &read)) && read > 0)
        {
            ULONG written;
            jobStream->Write(copyBuf, read, &written);
        }

        jobStream->Close();
        inputStream->Release();
        jobStream->Release();
        printJob->Release();
    }
}