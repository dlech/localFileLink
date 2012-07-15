// Extension.cpp : Implementation of CExtension

#include "stdafx.h"
#include "Extension.h"
#include <string>

// CExtension

STDMETHODIMP CExtension::OpenFileFolder(BSTR aPath)
{
    DWORD fileAttr;
    STARTUPINFO si;
    PROCESS_INFORMATION pi;
    std::wstring sPath(aPath);
    std::wstring sCmdline;

    ZeroMemory( &si, sizeof(si) );
    si.cb = sizeof(si);
    ZeroMemory( &pi, sizeof(pi) );

    fileAttr = GetFileAttributes(aPath);
    
    if (fileAttr == INVALID_FILE_ATTRIBUTES) {
        // file not found
        MessageBox(NULL, (LPCWSTR)aPath, (LPCWSTR)L"File not found", MB_ICONEXCLAMATION);
        // TODO ask to try parent directory
    } else {
        sCmdline = L"explorer ";
        if (!(fileAttr & FILE_ATTRIBUTE_DIRECTORY)) {
            // if this is not a directory, then we can actually select the file
            sCmdline.append(L"/select,");
        }        
        sCmdline.append(L"\"");
        sCmdline.append(sPath);
        sCmdline.append(L"\"");
        
        CreateProcess(NULL, (LPWSTR)sCmdline.c_str(), NULL, NULL, 
            FALSE, 0, NULL, NULL, &si, &pi);
       
        // close handles right away because we don't need them
        CloseHandle( pi.hProcess );
        CloseHandle( pi.hThread );    
    }
    
    return S_OK;
}

