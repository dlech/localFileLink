// Extension.cpp : Implementation of CExtension

#include "stdafx.h"
#include "Extension.h"
#include <atlstr.h>


// CExtension

STDMETHODIMP CExtension::OpenFileFolder(BSTR aPath)
{
    DWORD fileAttr;
    CString path = aPath;

    fileAttr = GetFileAttributes(path);
    
    if (fileAttr == INVALID_FILE_ATTRIBUTES) {
        // file not found
        CString title, message;

        title.LoadStringW(IDS_ERROR);
        message.FormatMessage(IDS_FILE_DOES_NOT_EXIST, path);
        message += _T("\n\n");
        message.AppendFormat(IDS_PROMPT_OPEN_PARENT);

        MessageBeep(MB_ICONQUESTION);
        int retVal = MessageBox(NULL, message, title, MB_YESNO |
            MB_ICONQUESTION | MB_TASKMODAL);
        
        if (retVal == IDYES) {
                        
            int length, sepLastIndex;
            do { // loop to ensure we get rid of trailing '\' chars
                length = path.GetLength();            
                sepLastIndex = path.ReverseFind(_T('\\'));
                if (sepLastIndex > 0) {
                    path.Truncate(sepLastIndex);
                }
            } while ((sepLastIndex > 0) && (sepLastIndex == length - 1));

            fileAttr = GetFileAttributes(path);
    
            if (fileAttr == INVALID_FILE_ATTRIBUTES) {

                message.FormatMessage(IDS_FILE_DOES_NOT_EXIST, path);

                MessageBeep(MB_ICONEXCLAMATION);
                int retVal = MessageBox(NULL, message, title,
                    MB_ICONEXCLAMATION | MB_TASKMODAL);

                return S_OK;
            }

        } else {
            // user clicked 'No', so don't do anything else
            return S_OK;
        }
    }

    STARTUPINFO si;
    PROCESS_INFORMATION pi;   
    CString sCmdline;
        
    ZeroMemory( &si, sizeof(si) );
    si.cb = sizeof(si);
    ZeroMemory( &pi, sizeof(pi) );

    sCmdline = _T("explorer ");
    if (!(fileAttr & FILE_ATTRIBUTE_DIRECTORY)) {
        // if this is not a directory, then we can actually select the file
        sCmdline += _T("/select,");
    }        
    sCmdline += _T("\"");
    sCmdline += path;
    sCmdline += _T("\"");
        
    CreateProcess(NULL, (LPWSTR)(LPCWSTR)sCmdline, NULL, NULL, 
        FALSE, 0, NULL, NULL, &si, &pi);
       
    // close handles right away because we don't need them
    CloseHandle( pi.hProcess );
    CloseHandle( pi.hThread );    
       
    return S_OK;
}


