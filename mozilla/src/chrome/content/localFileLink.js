

var mozLocalFileLink = {
  
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
    element.setAttribute("extensiontype", "mozilla");
  },
    
  showFile : function (aPathOrUrl) {
    let file = this.getLocalFileFromNativePathOrUrl(aPathOrUrl);    
    if (!file || !file.exists()) {
      let prompts = Components.classes["@mozilla.org/embedcomp/prompt-service;1"]  
        .getService(Components.interfaces.nsIPromptService); 
      let flags = prompts.STD_YES_NO_BUTTONS;
      let button0Text = "";
      let button1Text = "";
      let button2Text = "";
      let checkboxText = null;
      let showCheckbox = { value : false };
      let retVal = prompts.confirmEx(null, "Error", "Path '" + aPathOrUrl +
        "' does not exist.\n\n" + "Would you like to open the parent directory?", 
        flags, button0Text, button1Text, button2Text, checkboxText, showCheckbox);
      if (retVal == 0) { // user clicked yes
        file = file.parent.QueryInterface(Components.interfaces.nsILocalFile);
        if (!file || !file.exists()) {
          prompts.alert(null, "Error", "Directory '" + file.path + "' does not exist.");
          return;
        }
      }
    }
    try {
    // Show the directory containing the file and select the file
      file.reveal();
    } catch (e) { }
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