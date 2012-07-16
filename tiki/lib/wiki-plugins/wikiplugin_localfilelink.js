/* file: wikiplugin_localfilelink.js
 * 
 * works with wikiplugin_localfilelink.php to make hyperlinks to local files
 * actually work in web browser
 */


// make sure we only define once per page
if (typeof tikiLocalFileLink === "undefined") {

  var tikiLocalFileLink = {
  
    // trys to invoke a browser plugin to open a folder containing the file
    // requested (aPath)
    openFileFolder : function (aPath) {
      var extensionType = this.eventElement.getAttribute("extensionType");
      switch (extensionType) {
      case "mozilla":
        this.eventElement.setAttribute("path", aPath);
        var evt = document.createEvent("Events");
        evt.initEvent("localFileLinkOpenFileFolderEvent", true, false);
        this.eventElement.dispatchEvent(evt);
        break;
      case "IE":
        if (typeof this.activex === "undefined") {
          try {         
            this.activex = new ActiveXObject("LocalFileLink.Extension");
          } catch (ex) {
            alert("LocalFileLink extension is blocked or not installed.");
          }
        }
        if (this.activex) {
          this.activex.OpenFileFolder(aPath);
        }
        break;
      default:
        alert("LocalFileLink browser extension component is not installed.");
      }
    },
    
    // used to see if the browser extension is installed
    queryBrowser : function () {
      if (typeof window.ActiveXObject === "undefined") {
        // non IE browsers
        // send event to extension to see if it is there
        this.eventElement.setAttribute("extensionType", "");
        var evt = document.createEvent("Events");
        evt.initEvent("localFileLinkQueryBrowserEvent", true, false);
        this.eventElement.dispatchEvent(evt);  
      } else {
        // IE or compatable browser
        try {
          // if this is the first time this is called, it will trigger the 
          // 'Allow' popup in IE. new ActiveXObject throws exception if user
          // has not allowed this activex object
          this.activex = new ActiveXObject("LocalFileLink.Extension");          
        } catch (ex) {}
        this.eventElement.setAttribute("extensionType", "IE");
      }
    }
    
  };
  
  // init
  tikiLocalFileLink.eventElement = document.createElement("localFileLinkElement");
  document.documentElement.appendChild(tikiLocalFileLink.eventElement);
  tikiLocalFileLink.queryBrowser();  
}