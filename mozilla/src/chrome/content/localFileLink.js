

var mozLocalFileLink = {
  
  onPageLoad: function (aEvent) {
    doc = aEvent.originalTarget;  
    this.element = doc.getElementById("localFileLinkElement");
    alert(this.element);
  },  
  
  openFileFolderEventHandler : function (aEvent) {   
    let path = aEvent.target.getAttribute("path")  
    try {      
      this.showFile(path);
    } catch (ex) {
      
      alert("Error while opening '" + path + "'" + "\n\n" + ex);
    }
  },  
  
  queryBrowserEventHandler : function (aEvent) {
    let element = aEvent.target;
    element.setAttribute("browser", "mozilla");
  },
  
  // this method is from mozillas download handler (original name: showDownload)
  // http://mxr.mozilla.org/mozilla-release/source/toolkit/mozapps/downloads/content/downloads
  showFile : function (aPathOrUrl) {
    var file = this.getLocalFileFromNativePathOrUrl(aPathOrUrl);

    try {
      // Show the directory containing the file and select the file
      if (!file.exists()) {
        throw "File does not exist";
      }
      file.reveal();
    } catch (e) {
      // If reveal fails for some reason (e.g., it's not implemented on unix or
      // the file doesn't exist), try using the parent if we have it.
      let parent = file.parent.QueryInterface(Ci.nsILocalFile);
      if (!parent || !parent.exists()) {
        alert(aPathOrUrl + " does not exist.");
        return;
      }

      try {
        // "Double click" the parent directory to show where the file should be
       parent.launch();
      } catch (e) {
        // If launch also fails (probably because it's not implemented), let the
        // OS handler try to open the parent
        this.openExternal(parent);
      }
    }
  },
  
  openExternal : function (aFile) {
    var uri = Components.classes["@mozilla.org/network/io-service;1"].
      getService(Components.interfaces.nsIIOService).newFileURI(aFile);

    var protocolSvc = Components.classes["@mozilla.org/uriloader/external-protocol-service;1"].
      getService(Components.interfaces.nsIExternalProtocolService);
    protocolSvc.loadUrl(uri);

    return;
  },
  
  // this method is from mozillas download handler
  // http://mxr.mozilla.org/mozilla-release/source/toolkit/mozapps/downloads/content/downloads
  getLocalFileFromNativePathOrUrl : function (aPathOrUrl) {
    if (aPathOrUrl.substring(0,7) == "file://") {
      // if this is a URL, get the file from that
      let ioSvc = Components.classes["@mozilla.org/network/io-service;1"].
        getService(Components.interfaces.nsIIOService);

      // XXX it's possible that using a null char-set here is bad
      const fileUrl = ioSvc.newURI(aPathOrUrl, null, null).
      QueryInterface(Components.interfaces.nsIFileURL);
      return fileUrl.file.clone().QueryInterface(Components.interfaces.nsILocalFile);
      } else {
      // if it's a pathname, create the nsILocalFile directly
      const nsLocalFile = Components.Constructor("@mozilla.org/file/local;1",
        "nsILocalFile", "initWithPath");
      var f = new nsLocalFile(aPathOrUrl);

      return f;
    }
  }
};

document.addEventListener("localFileLinkOpenFileFolderEvent", function (aEvent) { 
  mozLocalFileLink.openFileFolderEventHandler(aEvent);
}, false, true);

document.addEventListener("localFileLinkQueryBrowserEvent", function (aEvent) { 
  mozLocalFileLink.queryBrowserEventHandler(aEvent);
}, false, true);