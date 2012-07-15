// dllmain.h : Declaration of module class.

class CLocalFileLinkModule : public ATL::CAtlDllModuleT< CLocalFileLinkModule >
{
public :
	DECLARE_LIBID(LIBID_LocalFileLink)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_LOCALFILELINK, "{55107954-3082-4C4D-BA31-07EBA4642DF7}")
};

extern class CLocalFileLinkModule _AtlModule;
