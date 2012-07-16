// Extension.h : Declaration of the CExtension

#pragma once
#include "resource.h"       // main symbols



#include "LocalFileLink_i.h"



#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

using namespace ATL;


// CExtension

class ATL_NO_VTABLE CExtension :
    public CComObjectRootEx<CComSingleThreadModel>,
    public CComCoClass<CExtension, &CLSID_Extension>,
    public IDispatchImpl<IExtension, &IID_IExtension, &LIBID_LocalFileLink,
        /*wMajor =*/ 1, /*wMinor =*/ 0>,
    public IObjectSafetyImpl<CExtension, INTERFACESAFE_FOR_UNTRUSTED_CALLER|
         INTERFACESAFE_FOR_UNTRUSTED_DATA>
{
public:
    CExtension()
    {
    }

DECLARE_REGISTRY_RESOURCEID(IDR_EXTENSION)


BEGIN_COM_MAP(CExtension)
    COM_INTERFACE_ENTRY(IExtension)
    COM_INTERFACE_ENTRY(IDispatch)
    COM_INTERFACE_ENTRY(IObjectSafety)
END_COM_MAP()



    DECLARE_PROTECT_FINAL_CONSTRUCT()

    HRESULT FinalConstruct()
    {
        return S_OK;
    }

    void FinalRelease()
    {
    }

public:



    STDMETHOD(OpenFileFolder)(BSTR path);
};

OBJECT_ENTRY_AUTO(__uuidof(Extension), CExtension)
