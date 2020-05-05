using System;
using System.ComponentModel.Design;
using EnvDTE;
using Lardite.RefAssistant;
using Lardite.RefAssistant.UI;
using Lardite.RefAssistant.VsProxy;
using Microsoft.VisualStudio.Shell;
using RefAssistant.Extensibility;
using Task = System.Threading.Tasks.Task;

namespace RefAssistant
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class RemoveUnusedReferencesCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("b08c4f30-9191-4084-8c81-b1f251366d06");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        private DTE dte;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveUnusedReferencesCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private RemoveUnusedReferencesCommand(AsyncPackage package, OleMenuCommandService commandService, DTE dte)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);

            this.dte = dte;
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static RemoveUnusedReferencesCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in RemoveUnusedReferencesCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
            var dte = await package.GetServiceAsync(typeof(DTE)) as DTE;
            Instance = new RemoveUnusedReferencesCommand(package, commandService, dte);

        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            /*
            string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.GetType().FullName);
            string title = "Remove Unused References";

            // Show a message box to prove we were here
            VsShellUtilities.ShowMessageBox(
                this.package,
                message,
                title,
                OLEMSGICON.OLEMSGICON_INFO,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            */

            var activeProjectGuid = Guid.Parse(DTEHelper.GetActiveProject(this.package /*ServiceProvider*/).Kind);
            LogManager.ActivityLog.Information(string.Format(Resources.RemoveProjectReferencesCmd_StartRemoving, activeProjectGuid.ToString("D")));

            var shellGateway = new ShellGateway(package, GeneralOptions.Instance);  // package.GetDialogPage(typeof(GeneralOptionsPage)) as IExtensionOptions);

            using (var manager = new ExtensionManager(shellGateway))
            {
                manager.ProgressChanged += OnProgressChanged;
                manager.StartProjectCleanup();
            }
        }

        /// <summary>
        /// Removing progress changed.
        /// </summary>
        void OnProgressChanged(object sender, ProgressEventArgs e)
        {
            string text = e.Progress == 100 ? e.Text : string.Format("[{1}%] {0}", e.Text, e.Progress.ToString());
            
            dte.StatusBar.Text = text; // SetStatusBarText(text);
        }
    }
}
