// AlphaBlendToolbar //////////////////////////////////////////////////////////
//
// Description: This addin creates a new toolbar in visual studio called
// "Alpha Toolbar" and then adds two new commands to it which feature
// alpha-transparent icon images.  The images are added directly via a
// reference to a System.Drawing.Bitmap, without needing to provide an
// index or resource ID.  Additionally, it is no longer necessary to provide
// a transparency mask to support transparency.
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Windows.Forms;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;

namespace AlphaBlendToolbar
{
    /// <summary>The object for implementing an Add-in.</summary>
    /// <seealso class='IDTExtensibility2' />
    public class Connect : IDTExtensibility2, IDTCommandTarget
    {
        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
        public Connect()
        {
        }

        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            _applicationObject = (DTE2)application;
            _addInInstance = (AddIn)addInInst;

            if (connectMode == ext_ConnectMode.ext_cm_Startup || connectMode == ext_ConnectMode.ext_cm_AfterStartup)
            {
                object[] contextGUIDS = new object[] { };
                Commands2 commands = (Commands2)_applicationObject.Commands;

                // Add new toolbar and set enabled and visible
                CommandBars commandBars = _applicationObject.CommandBars as CommandBars;
                CommandBar toolbar = commandBars.Add("AlphaToolbar", MsoBarPosition.msoBarTop, System.Type.Missing, true);
                toolbar.Visible = true;
                toolbar.Enabled = true;

                // Make sure the alpha command doesn't already exist
                Command commandAlpha = null, commandOmega = null;

                try
                {
                    commandAlpha = commands.Item("AlphaBlendToolbar.Connect.AlphaButton");
                }
                catch (System.ArgumentException)
                {
                    // Load the alpha button image from a 32-bit alpha-transparent portable network graphic file which is an 
                    // embedded resource in this project.  This image could also be loaded directly from disk.
                    System.Drawing.Bitmap alpha = new System.Drawing.Bitmap(typeof(AlphaBlendToolbar.Connect), @"alpha.png");

                    // Create the alpha button command, specifying the command name, button text, tooltip, bitmap image,
                    // status, style, and control type.  Add this command to the AlphaToolbar.
                    commandAlpha = commands.AddNamedCommand2(_addInInstance, "AlphaButton", "Alpha", "Alpha Command", false, alpha, ref contextGUIDS,
                        (int)vsCommandStatus.vsCommandStatusEnabled, (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);
                }
                commandAlpha.AddControl(toolbar, 1);

                // Make sure the omega command doesn't already exist
                try
                {
                    commandOmega = commands.Item("AlphaBlendToolbar.Connect.OmegaButton");
                }
                catch (System.ArgumentException)
                {
                    // Load the omega button image from a 32-bit alpha-transparent portable network graphic file which is an 
                    // embedded resource in this project.  This image could also be loaded directly from disk.
                    System.Drawing.Bitmap omega = new System.Drawing.Bitmap(typeof(AlphaBlendToolbar.Connect), @"omega.png");

                    // Create the omega button command, specifying the command name, button text, tooltip, bitmap image,
                    // status, style, and control type.  Add this command to the AlphaToolbar.
                    commandOmega = commands.AddNamedCommand2(_addInInstance, "OmegaButton", "Omega", "Omega Command", false, omega, ref contextGUIDS,
                        (int)vsCommandStatus.vsCommandStatusEnabled, (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);
                }
                commandOmega.AddControl(toolbar, 2);
            }
        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
        }

        /// <summary>Implements the QueryStatus method of the IDTCommandTarget interface. This is called when the command's availability is updated</summary>
        /// <param term='commandName'>The name of the command to determine state for.</param>
        /// <param term='neededText'>Text that is needed for the command.</param>
        /// <param term='status'>The state of the command in the user interface.</param>
        /// <param term='commandText'>Text requested by the neededText parameter.</param>
        /// <seealso class='Exec' />
        public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
        {
            if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
            {
                // Let the caller know that the alpha command is supported and enabled
                if (commandName == "AlphaBlendToolbar.Connect.AlphaButton")
                {
                    status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
                    return;
                }
                // Let the caller know that the omega button is supported and enabled
                else if (commandName == "AlphaBlendToolbar.Connect.OmegaButton")
                {
                    status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
                    return;
                }
            }
        }

        /// <summary>Implements the Exec method of the IDTCommandTarget interface. This is called when the command is invoked.</summary>
        /// <param term='commandName'>The name of the command to execute.</param>
        /// <param term='executeOption'>Describes how the command should be run.</param>
        /// <param term='varIn'>Parameters passed from the caller to the command handler.</param>
        /// <param term='varOut'>Parameters passed from the command handler to the caller.</param>
        /// <param term='handled'>Informs the caller if the command was handled or not.</param>
        /// <seealso class='Exec' />
        public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
        {
            handled = false;

            if (executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
            {
                // Handle the alpha button command
                if (commandName == "AlphaBlendToolbar.Connect.AlphaButton")
                {
                    System.Windows.Forms.MessageBox.Show("Alpha");
                    handled = true;
                    return;
                }
                // Handle the omega button command
                else if (commandName == "AlphaBlendToolbar.Connect.OmegaButton")
                {
                    System.Windows.Forms.MessageBox.Show("Omega");
                    handled = true;
                    return;
                }
            }
        }

        private DTE2 _applicationObject;
        private AddIn _addInInstance;
    }
}