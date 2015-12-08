using System;
using System.Collections.Specialized;
using System.Windows.Forms;
using Mulholland.QSet.Application.Controls;
using Mulholland.QSet.Application.WebServices;
using Mulholland.QSet.Resources;
using Mulholland.WinForms;
using TD.SandBar;

namespace Mulholland.QSet.Application
{
	/// <summary>
	/// Provides a base class to identify primary control holder classes.
	/// </summary>
	internal abstract class PrimaryControlsBase {}
	
	#region internal class PrimaryMenus : PrimaryControlsBase

	/// <summary>
	/// Groups together all primary environment menus.
	/// </summary>
	internal class PrimaryMenus : PrimaryControlsBase
	{
		private MenuBarItem _fileMenu;
		private MenuBarItem _viewMenu;
		private MenuBarItem _qSetMenu;
		private MenuBarItem _queueMenu;
		private MenuBarItem _messageMenu;
		private MenuBarItem _toolsMenu;
		private MenuBarItem _helpMenu;		
		private ContextMenuBarItem _messageBrowserContextMenu;
		private ContextMenuBarItem _qSetContextMenu;

		private event PrimaryMenus.MenuItemsChangedEvent _recentFileListChanged;

		#region events

		/// <summary>
		/// Event delegate for MenuItemsChanged event.
		/// </summary>
		public delegate void MenuItemsChangedEvent(object sender, EventArgs e);


		/// <summary>
		/// Event arguments for MenuItemsChanged event.
		/// </summary>
		public class MenuItemsChangedEventArgs : EventArgs
		{
			private MenuButtonItem _parentItem;

			/// <summary>
			/// Constructs the arguments class.
			/// </summary>
			/// <param name="parentItem">Parent item for which child items changed.</param>
			public MenuItemsChangedEventArgs(MenuButtonItem parentItem)
			{
				_parentItem = parentItem;
			}


			/// <summary>
			/// Gets the parent item for which child items changed.
			/// </summary>
			public MenuButtonItem ParentItem
			{
				get
				{
					return _parentItem;
				}
			}
		}


		/// <summary>
		/// Occurs when the recent file list is changed.
		/// </summary>
		public event PrimaryMenus.MenuItemsChangedEvent RecentFileListChanged
		{
			add
			{
				_recentFileListChanged += value;
			}
			remove 
			{
				_recentFileListChanged -= value;
			}
		}


		/// <summary>
		/// Raises the RecentFileListChanged event.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		protected void OnRecentFileListChanged(MenuItemsChangedEventArgs e)
		{
			try
			{
				if (_recentFileListChanged != null)
					_recentFileListChanged(this, e);
			}
			catch {}
		}

		#endregion

		/// <summary>
		/// Constructs the object with all menus.
		/// </summary>
		/// <param name="fileMenu">File menu.</param>
		/// <param name="viewMenu">View menu.</param>
		/// <param name="qSetMenu">Q Set menu.</param>
		/// <param name="queueMenu">Queue menu.</param>
		/// <param name="messageMenu">Message menu.</param>
		/// <param name="toolsMenu">Tools menu.</param>
		/// <param name="helpMenu">Help menu.</param>		
		/// <param name="messageBrowserContextMenu">Message browser context menu.</param>
		/// <param name="qSetContextMenu">Q Set context menu.</param>
		public PrimaryMenus(
			MenuBarItem fileMenu,
			MenuBarItem viewMenu,
			MenuBarItem qSetMenu,
			MenuBarItem queueMenu,
			MenuBarItem messageMenu,
			MenuBarItem toolsMenu,
			MenuBarItem helpMenu,
			ContextMenuBarItem messageBrowserContextMenu,
			ContextMenuBarItem qSetContextMenu)
		{
			if (fileMenu == null) throw new ArgumentNullException("fileMenu");
			else if (viewMenu == null) throw new ArgumentNullException("viewMenu");
			else if (qSetMenu == null) throw new ArgumentNullException("qSetMenu");
			else if (queueMenu == null) throw new ArgumentNullException("queueMenu");
			else if (messageMenu == null) throw new ArgumentNullException("messageMenu");
			else if (toolsMenu == null) throw new ArgumentNullException("toolsMenu");
			else if (helpMenu == null) throw new ArgumentNullException("helpMenu");
			else if (messageBrowserContextMenu == null) throw new ArgumentNullException("messageBrowserContextMenu");

			_fileMenu = fileMenu;
			_viewMenu = viewMenu;
			_qSetMenu = qSetMenu;
			_queueMenu = queueMenu;
			_messageMenu = messageMenu;
			_toolsMenu = toolsMenu;
			_helpMenu = helpMenu;
			_messageBrowserContextMenu = messageBrowserContextMenu;
			_qSetContextMenu = qSetContextMenu;
		}


		/// <summary>
		/// Gets the environment File menu.
		/// </summary>
		public MenuBarItem FileMenu
		{
			get
			{
				return _fileMenu;
			}
		}


		/// <summary>
		/// Gets the environment View menu.
		/// </summary>
		public MenuBarItem ViewMenu
		{
			get
			{
				return _viewMenu;
			}
		}


		/// <summary>
		/// Gets the environment Q Set menu.
		/// </summary>
		public MenuBarItem QSetMenu
		{
			get
			{
				return _qSetMenu;
			}
		}


		/// <summary>
		/// Gets the environment Queue menu.
		/// </summary>
		public MenuBarItem QueueMenu
		{
			get
			{
				return _queueMenu;
			}
		}


		/// <summary>
		/// Gets the environment Message menu.
		/// </summary>
		public MenuBarItem MessageMenu
		{
			get
			{
				return _messageMenu;
			}
		}


		/// <summary>
		/// Gets the environment Tools menu.
		/// </summary>
		public MenuBarItem ToolsMenu
		{
			get
			{
				return _toolsMenu;
			}
		}


		/// <summary>
		/// Gets the environment Help menu.
		/// </summary>
		public MenuBarItem HelpMenu
		{
			get
			{
				return _helpMenu;
			}
		}


		/// <summary>
		/// Message browser context menu.
		/// </summary>
		public ContextMenuBarItem MessageBrowserContextMenu
		{
			get
			{
				return _messageBrowserContextMenu;
			}
		}


		/// <summary>
		/// Q Set context menu.
		/// </summary>
		public ContextMenuBarItem QSetContextMenu
		{
			get
			{
				return _qSetContextMenu;
			}
		}


		/// <summary>
		/// Refreshes the recent file list.
		/// </summary>
		/// <param name="recentFiles">List of recent files, chronoligically ordered.</param>
		/// <param name="maximumListSize">The maximum size of the list.</param>
		public void RefreshRecentFilesList(StringCollection recentFiles, int maximumListSize)
		{
			MenuItemBag.FileRecentFileList.Items.Clear();

			for (int file = 0; file < (recentFiles.Count < maximumListSize ? recentFiles.Count : maximumListSize); file ++)
			{
				MenuButtonItem recentFileButton = new MenuButtonItem(string.Format("&{0} {1}", file + 1, recentFiles[file]));
				recentFileButton.Tag = recentFiles[file];
				MenuItemBag.FileRecentFileList.Items.Add(recentFileButton);
			}
			
			MenuItemBag.FileRecentFileList.Visible = (MenuItemBag.FileRecentFileList.Items.Count > 0);

			OnRecentFileListChanged(new PrimaryMenus.MenuItemsChangedEventArgs(MenuItemBag.FileRecentFileList));
		}
	}

	#endregion

	#region internal class PrimaryControls : PrimaryControlsBase

	/// <summary>
	/// Groups together the primary controls of the environment.
	/// </summary>
	/// <remarks>This does not contain menu or toolbar controls.</remarks>
	internal class PrimaryControls : PrimaryControlsBase
	{
		private QSetExplorer _qSetExplorer;
		private QSetMonitor _qSetMonitor;
		private PropertyGrid _propertyGrid;
		private MessageViewer _messageViewer;
		private TD.SandDock.DocumentContainer _documentContainer;
		private MessageBrowserCollection _messageBrowserCollection;
		private WebServiceClientControlCollection _webServiceClientControlCollection;
		private Images _images;

		/// <summary>
		/// Constructs the object with all of the environments primary controls.
		/// </summary>
		/// <param name="qSetExplorer">Primary QSetExplorer.</param>
		/// <param name="qSetMonitor">Primary QSetMonitor.</param>
		/// <param name="propertyGrid">Primary property grid.</param>
		/// <param name="messageViewer">Primary MessageViewer.</param>
		/// <param name="documentContainer">Primary DocumentContainer.</param>
		/// <param name="images">Images component.</param>
		public PrimaryControls(
			QSetExplorer qSetExplorer,
			QSetMonitor qSetMonitor,
			PropertyGrid propertyGrid,
			MessageViewer messageViewer,
			TD.SandDock.DocumentContainer documentContainer,
			Images images)
		{
			_qSetExplorer = qSetExplorer;
			_qSetMonitor = qSetMonitor;
			_propertyGrid = propertyGrid;
			_messageViewer = messageViewer;
			_documentContainer = documentContainer;		
			_images = images;

			_messageBrowserCollection = new MessageBrowserCollection();
			_webServiceClientControlCollection = new WebServiceClientControlCollection();
		}


		/// <summary>
		/// Gets the environment's primary QSetExplorer.
		/// </summary>
		public QSetExplorer QSetExplorer
		{
			get
			{
				return _qSetExplorer;
			}
		}


		/// <summary>
		/// Gets the environment's primary QSetMonitor.
		/// </summary>
		public QSetMonitor QSetMonitor
		{
			get
			{
				return _qSetMonitor;
			}
		}


		/// <summary>
		/// Gets the environment's property grid.
		/// </summary>
		public PropertyGrid PropertyGrid
		{
			get
			{
				return _propertyGrid;
			}
		}


		/// <summary>
		/// Gets the environment's primary MessageViewer.
		/// </summary>
		public MessageViewer MessageViewer
		{
			get
			{
				return _messageViewer;
			}
		}


		/// <summary>
		/// Gets the environment's primary DocumentContainer.
		/// </summary>
		public  TD.SandDock.DocumentContainer DocumentContainer
		{
			get
			{
				return _documentContainer;
			}
		}


		/// <summary>
		/// Gets the main message browser collection.
		/// </summary>
		public MessageBrowserCollection MessageBrowserCollection
		{
			get
			{
				return _messageBrowserCollection;
			}
		}


		/// <summary>
		/// Gets the main web service client control collection.
		/// </summary>
		public WebServiceClientControlCollection WebServiceClientControlCollection
		{
			get
			{
				return _webServiceClientControlCollection;
			}
		}


		/// <summary>
		/// Gets the main application Images component.
		/// </summary>
		public Images Images
		{			
			get 
			{
				return _images;
			}
		}

	}

	#endregion

	#region internal class PrimaryForms : PrimaryControlsBase

	/// <summary>
	/// Groups together persistable forms and dialogues that are not 
	/// </summary>
	internal class PrimaryForms : PrimaryControlsBase
	{
		private QSetEnvironmentForm _environmentForm;
		private QueueSearchForm _QueueSearchForm;

		/// <summary>
		/// Constructs the object with the minumum requirements.
		/// </summary>
		/// <param name="environmentForm">Main environment form.</param>
		/// <param name="QueueSearchForm">Persistant search dialog.</param>
		public PrimaryForms(QSetEnvironmentForm environmentForm, QueueSearchForm QueueSearchForm)
		{
			if (QueueSearchForm == null) throw new ArgumentNullException("QueueSearchForm");
			else if (environmentForm == null) throw new ArgumentNullException("environmentForm");

			_environmentForm = environmentForm;
			_QueueSearchForm = QueueSearchForm;
		}


		/// <summary>
		/// Gets the main environment form.
		/// </summary>
		public QSetEnvironmentForm EnvironmentForm 
		{
			get
			{
				return _environmentForm;
			}			
		}


		/// <summary>
		/// Gets the dialogue use for searching for queues.
		/// </summary>
		public QueueSearchForm QueueSearchForm
		{
			get
			{
				return _QueueSearchForm;
			}
		}

	}

	#endregion
}