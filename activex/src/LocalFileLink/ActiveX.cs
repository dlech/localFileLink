using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;


namespace LocalFileLink
{

    [ProgId("LocalFileLink.ActiveX")]

    [Guid("4EBA1E78-3660-4d56-9EAE-3554C36BDD7F")]
    public class ActiveX : IObjectSafety
    {
        //Main function
        public void OpenPath(string path)
        {
            try
            {
                //use FileSystemInfo becasuse path could be a file or a directory
                FileSystemInfo fi = new FileInfo(path); //try as a file first
                if ((fi.Attributes & FileAttributes.Directory) == FileAttributes.Directory) //following does not work in .net 2 but reads better - (fi.Attributes.HasFlag(FileAttributes.Directory))
                {
                    //use DirectoryInfo if it is a directory
                    fi = new DirectoryInfo(path);
                }

                //if file or directory does not exist
                if (!fi.Exists)
                {
                    // ask if the user wants to try opening the parent directory - maybe the file was renamed or there is similar info
                    DialogResult dr;
                    dr = MessageBox.Show("The path \"" + fi.FullName + "\" does not exist.\n"
                        + "Would you like to open the parent directory?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        fi = new DirectoryInfo(new FileInfo(path).DirectoryName);
                    }
                    else
                    {
                        return;
                    }
                }

                if (fi.Exists)
                {
                    if ((fi.Attributes & FileAttributes.Directory) == FileAttributes.Directory) //following does not work in .net 2 but reads better - (fi.Attributes.HasFlag(FileAttributes.Directory))
                    {
                        Process.Start("explorer", fi.FullName);
                    }
                    else
                    {

                        Process.Start("explorer", "/select," + fi.FullName);
                    }
                }
                else
                {
                    MessageBox.Show("The path \"" + fi.FullName + "\" does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //used to indicate that this class is safe for scripting
        #region IObjectSafety

        public IObjectSafetyRetVals GetInterfaceSafetyOptions(ref Guid riid, out IObjectSafetyOpts supportedOpts, out IObjectSafetyOpts enabledOpts)
        {
            supportedOpts = IObjectSafetyOpts.INTERFACESAFE_FOR_UNTRUSTED_CALLER | IObjectSafetyOpts.INTERFACESAFE_FOR_UNTRUSTED_DATA;
            enabledOpts = IObjectSafetyOpts.INTERFACESAFE_FOR_UNTRUSTED_CALLER | IObjectSafetyOpts.INTERFACESAFE_FOR_UNTRUSTED_DATA;
            return IObjectSafetyRetVals.S_OK;
        }

        //used to indicate that this is safe for scripting
        public IObjectSafetyRetVals SetInterfaceSafetyOptions(ref Guid riid, IObjectSafetyOpts optsMask, IObjectSafetyOpts enabledOpts)
        {
            return IObjectSafetyRetVals.S_OK;
        }
        #endregion


    }

    // code for implementing IObjectSafety
    #region IObjectSafety

    // code from http://www.atalasoft.com/cs/blogs/rickm/archive/2009/06/03/net-2-0-activex-control-gotchas-safe-for-scripting-and-hooking-into-events.aspx

    [Flags]
    public enum IObjectSafetyOpts : int //DWORD
    {
        // Object is safe for untrusted callers.
        INTERFACESAFE_FOR_UNTRUSTED_CALLER = 0x00000001,
        // Object is safe for untrusted data.
        INTERFACESAFE_FOR_UNTRUSTED_DATA = 0x00000002,
        // Object uses IDispatchEx.
        INTERFACE_USES_DISPEX = 0x00000004,
        // Object uses IInternetHostSecurityManager.
        INTERFACE_USES_SECURITY_MANAGER = 0x00000008
    }

    public enum IObjectSafetyRetVals : uint //HRESULT
    {
        //The object is safe for loading.
        S_OK = 0x0,
        //The riid parameter specifies an interface that is unknown to the object.
        E_NOINTERFACE = 0x80000004
    }

    [ComImport()]
    //This GUID is that of IObjectSafety. Do not replace!
    [Guid("CB5BDC81-93C1-11CF-8F20-00805F2CD064")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]

    public interface IObjectSafety
    {
        [PreserveSig()]
        IObjectSafetyRetVals GetInterfaceSafetyOptions(ref Guid riid, out IObjectSafetyOpts supportedOpts, out IObjectSafetyOpts enabledOpts);
        [PreserveSig()]
        IObjectSafetyRetVals SetInterfaceSafetyOptions(ref Guid riid, IObjectSafetyOpts optsMask, IObjectSafetyOpts enabledOpts);
    }

    #endregion

}
