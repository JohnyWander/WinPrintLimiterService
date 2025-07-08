#pragma once

//using namespace System;

namespace OxpsPrintWrapper {
    public ref class OxpsPrinter
    {
    public:
        static void PrintOxps(System::String^ oxpsFilePath, System::String^ printerName);
    };
}
