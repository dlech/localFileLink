option explicit

sub openPath(filePath)

	dim objLFL
	set objLFL = CreateObject("LocalFileLink.ActiveX")
	objLFL.openPath(FilePath)
	set objLFL = Nothing

end sub