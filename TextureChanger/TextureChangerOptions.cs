using System;
using System.Drawing;
using System.Windows.Forms;
using TextureChanger.util;
using System.Text;
using System.IO;
using Win32;

namespace TextureChanger
{
	// TODO : 設定オブジェクトを定義する

	/*
		[WindowPosition]
		upperHigh = 284
		upperLeftWide = 311
		lowerLeftWide = 256
	 */


	public class TextureChangerOptions
	{
		readonly IniFile _iniFile;

		private string _pathToSaiFolder;

		private bool _firstExpandingUseFixed;
		private string _firstExpandingRecentFolder;
		private string _firstExpandingFixedFolder;

		private bool _promptToExitProgram;

		private Rectangle _windowBounds;
		private FormWindowState _windowState;

		private int _splitterDistanceNorthSouth;
		private int _splitterDistanceTreeList;

		private string _lastEditingTextureName;
		private string _lastEditingTextureImagePath;

		#region SAIのフォルダ指定プロパティ
		public string PathToSaiFolder
		{
			get { return _pathToSaiFolder; }
			set { _iniFile["SAI", "folder"] = _pathToSaiFolder = value; }
		}
		#endregion
		
		#region 起動時のフォルダの指定プロパティ
		public bool FirstExpandingUseFixed
		{
			get { return _firstExpandingUseFixed; }
			private set { _iniFile["FirstExpanding", "UseFixed"] = (_firstExpandingUseFixed = value).ToString(); }
		}
		public string FirstExpandingRecentFolder
		{
			get { return _firstExpandingRecentFolder; }
			private set { _iniFile["FirstExpanding", "RecentFolder"] = _firstExpandingRecentFolder = value; }
		}
		public string FirstExpandingFixedFolder
		{
			get { return _firstExpandingFixedFolder; }
			private set { _iniFile["FirstExpanding", "FixedFolder"] = _firstExpandingFixedFolder = value; }
		}
		public void ToggleFirstExpandingUseFixed()
		{
			FirstExpandingUseFixed = !FirstExpandingUseFixed;
		}
		public void SetToUseFirstExpandingRecentFolder(string path = "")
		{
			FirstExpandingUseFixed = false;
			if (path != "")
			{
				FirstExpandingRecentFolder = path;
			}
		}
		public void SetToUseFirstExpandingFixedFolder(string path = "")
		{
			FirstExpandingUseFixed = true;
			if (path != "")
			{
				FirstExpandingFixedFolder = path;
			}
		}
		#endregion

		#region 終了時の問い合わせプロパティ
		public bool PromptToExitProgram
		{
			get { return _promptToExitProgram; }
			set { _iniFile["Settings", "PromptToExitProgram"] = (_promptToExitProgram = value).ToString(); }
		}

		#endregion

		#region ウィンドウ位置保存プロパティ
		public Rectangle WindowBounds
		{
			get { return _windowBounds; }
			private set
			{
				_windowBounds = value;
				string temp = String.Format("{0},{1},{2},{3}"
					, value.X, value.Y, value.Width, value.Height);
				_iniFile["Settings", "WindowBounds"] = temp;
			}
		}
		public FormWindowState WindowState
		{
			get { return _windowState; }
			private set { _iniFile["Settings", "WindowStates"] = (_windowState = value).ToString(); }
		}
		public void SaveWindowConditions(Rectangle bounds, FormWindowState state)
		{
			WindowBounds = bounds;
			WindowState = state;
		}
		public void LoadWindowConditions(out Rectangle bounds, out FormWindowState state)
		{
			bounds = WindowBounds;
			state = WindowState;
		}
		#endregion

		#region スプリッターの表示状態
		public int SplitterDistanceNorthSouth
		{
			get { return _splitterDistanceNorthSouth; }
			private set { _iniFile["Settings", "SplitterDistanceNorthSouth"] = (_splitterDistanceNorthSouth = value).ToString(); }
		}
		public int SplitterDistanceTreeList
		{
			get { return _splitterDistanceTreeList; }
			private set { _iniFile["Settings", "SplitterDistanceTreeList"] = (_splitterDistanceTreeList = value).ToString(); }
		}
		public void SaveSplitterDistances(int northSouth, int treeList)
		{
			SplitterDistanceNorthSouth = northSouth;
			SplitterDistanceTreeList = treeList;
		}
		#endregion

		#region 編集状態保存プロパティ
		public string LastEditingTextureName
		{
			get { return _lastEditingTextureName; }
			private set { _iniFile["Settings", "LastEditingTextureName"] = _lastEditingTextureName = value; }
		}
		public string LastEditingTextureImagePath
		{
			get { return _lastEditingTextureImagePath; }
			set { _iniFile["Settings", "LastEditingTextureImagePath"] = _lastEditingTextureImagePath = value; }
		}
		public void SaveLastEditings(string textureName, string imagePath)
		{
			if (string.IsNullOrEmpty(textureName))
			{
				throw new ArgumentNullException();
			}
			LastEditingTextureName = textureName;
			LastEditingTextureImagePath = imagePath; //imagePathは空文字を許可
		}
		#endregion


		public TextureChangerOptions()
		{
			_iniFile = new IniFile( );

			#region SAIのフォルダ指定
			_pathToSaiFolder = _iniFile["SAI", "folder"];
			#endregion

			#region エクスプローラの初期表示フォルダの設定
			_firstExpandingUseFixed = (_iniFile["FirstExpanding", "UseFixed"] == Boolean.TrueString);
			_firstExpandingRecentFolder = _iniFile["FirstExpanding", "RecentFolder"];
			_firstExpandingFixedFolder  = _iniFile["FirstExpanding", "FixedFolder" ];
			#endregion

			#region 終了時にそれを確認する
			_promptToExitProgram = (_iniFile["Settings", "PromptToExitProgram"] == Boolean.TrueString);
			#endregion

			#region 前回のウィンドウ位置
			try
			{
				string temp = _iniFile["Settings", "WindowBounds"];
				_windowBounds = (Rectangle)new RectangleConverter().ConvertFromString(temp);
			}
			catch
			{
				_windowBounds = new Rectangle(0,0,998,615);
			}
			try
			{
				string temp = _iniFile["Settings", "WindowStates"];
				_windowState = (FormWindowState)Enum.Parse(typeof(FormWindowState), temp);
			}
			catch
			{
				_windowState = FormWindowState.Normal;
			}
			#endregion

			#region 前回のスプリッター状態
			try
			{
				var temp = _iniFile["Settings", "SplitterDistanceNorthSouth"];
				_splitterDistanceNorthSouth = int.Parse(temp);
			}
			catch
			{
				_splitterDistanceNorthSouth = 270; //TODO const
			}
			try
			{
				var temp = _iniFile["Settings", "SplitterDistanceTreeList"];
				_splitterDistanceTreeList = int.Parse(temp);
			}
			catch
			{
				_splitterDistanceTreeList = 330; //TODO const
			}
			#endregion

			#region 前回終了時編集中だったテクスチャ
			LastEditingTextureName = _iniFile["Settings", "LastEditingTextureName"];
			LastEditingTextureImagePath = _iniFile["Settings", "LastEditingTextureImagePath"];
			#endregion

			#region 前回使用フォルダ名が空の場合は初期状態と判断して初期値を設定する
			if (_firstExpandingUseFixed == false
				&& _firstExpandingRecentFolder == "")
			{
				
				//TODO 設定をひとつづつ判定して初期値を入れる

				string temp = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
				FirstExpandingUseFixed = false;
				FirstExpandingRecentFolder = temp;
				FirstExpandingFixedFolder = temp;
				PathToSaiFolder = "";
				PromptToExitProgram = true;
				WindowBounds = new Rectangle(0, 0, 998, 615);//TODO const
				WindowState = FormWindowState.Normal;
				SplitterDistanceNorthSouth = 270; //TODO const
				SplitterDistanceTreeList = 330; //TODO const
				LastEditingTextureName = TextureManager.BLOTMAP_NAME;
				LastEditingTextureImagePath = "";
			}
			#endregion

		}


	}
}