using Lechnology.LocalFileLink;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace LocalFileLinkTestProject
{

  /// <summary>
  ///This is a test class for LocalFileLinkTest and is intended
  ///to contain all LocalFileLinkTest Unit Tests
  ///</summary>
  [TestClass()]
  public class LocalFileLinkTest
  {
    /// <summary>
    ///A test for OpenPath
    ///</summary>
    [TestMethod()]
    public void OpenPathTest()
    {
      // use this assembly as a test file
      string path = Assembly.GetExecutingAssembly().Location;

      // make sure directory is not already displayed before we start or we
      // could have false results

      // try closing 
      SHDocVw.InternetExplorer ie = GetExplorerForPath(path);
      if (ie != null) {
        ie.Quit();
        Thread.Sleep(1000);
      }

      // if it is still open, fail test
      ie = GetExplorerForPath(path);
      Assert.IsNull(ie, "Test directory '" + Path.GetDirectoryName(path) +
        "' is already open.\n" + "Please close it and run test again.");

      LocalFileLink target = new LocalFileLink();
      target.OpenPath(path);
      Thread.Sleep(1000); //give it a sec. to open

      ie = GetExplorerForPath(path);
      Assert.IsNotNull(ie, "Failed to open '" + Path.GetDirectoryName(path) +
        "'");
      ie.Quit();
    }

    /// <summary>
    /// Check to see if windows explorer is open to the specified path
    /// </summary>
    /// <param name="path">path of file or directory</param>
    /// <returns>Internet Explorer object or null</returns>
    private SHDocVw.InternetExplorer GetExplorerForPath(string path)
    {
      path = Path.GetDirectoryName(path);
      SHDocVw.ShellWindows shellWindows = new SHDocVw.ShellWindowsClass();
            
      foreach (SHDocVw.InternetExplorer ie in shellWindows) {
        string filename = Path.GetFileNameWithoutExtension(ie.FullName);
        if (filename.Equals("explorer", StringComparison.OrdinalIgnoreCase)) {
          Uri uri = new Uri(ie.LocationURL);
          if (uri.LocalPath == path) {
            return ie;
          }
        }
      }
      return null;
    }
  }
}
