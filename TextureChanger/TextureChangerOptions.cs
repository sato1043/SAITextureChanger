﻿using System;
using System.Drawing;
using System.Windows.Forms;
using TextureChanger.util;
using System.Text;
using System.IO;
using Win32;

namespace TextureChanger
{
	public class TextureChangerOptions
	{
		readonly IniFile _iniFile;

		private string _pathToSaiFolder;

		private bool _firstExpandingUseFixed;
		private string _firstExpandingRecentFolder;
		private string _firstExpandingFixedFolder;

		private bool _promptToExitProgram;

		private bool _checkUpdateAtStartUp;

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
		public string FirstExpandingFolder
		{
			get { return _firstExpandingUseFixed ? _firstExpandingFixedFolder : _firstExpandingRecentFolder;  }
		}
		#endregion

		#region 終了時の問い合わせプロパティ
		public bool PromptToExitProgram
		{
			get { return _promptToExitProgram; }
			set { _iniFile["Settings", "PromptToExitProgram"] = (_promptToExitProgram = value).ToString(); }
		}
		#endregion

		#region 起動時の更新確認プロパティ
		public bool CheckUpdateAtStartUp
		{
			get { return _checkUpdateAtStartUp; }
			set { _iniFile["Settings", "CheckUpdateAtStartUp"] = (_checkUpdateAtStartUp = value).ToString(); }
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

		private const int DefaultSplitterDistanceNorthSouth = 270;
		private const int DefaultSplitterDistanceTreeList   = 330;

		private const int DefaultWindowBoundsWide = 998;
		private const int DefaultWindowBoundsHigh = 615;

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

			#region 起動時に更新を確認する
			_checkUpdateAtStartUp = (_iniFile["Settings", "CheckUpdateAtStartUp"] == Boolean.TrueString);
			#endregion

			#region 前回のウィンドウ位置
			try
			{
				string temp = _iniFile["Settings", "WindowBounds"];
				_windowBounds = (Rectangle)new RectangleConverter().ConvertFromString(temp);
			}
			catch
			{
				_windowBounds = new Rectangle( 0, 0, DefaultWindowBoundsWide, DefaultWindowBoundsHigh );
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
				_splitterDistanceNorthSouth = DefaultSplitterDistanceNorthSouth;
			}
			try
			{
				var temp = _iniFile["Settings", "SplitterDistanceTreeList"];
				_splitterDistanceTreeList = int.Parse(temp);
			}
			catch
			{
				_splitterDistanceTreeList = DefaultSplitterDistanceTreeList;
			}
			#endregion

			#region 前回終了時編集中だったテクスチャ
			LastEditingTextureName = _iniFile["Settings", "LastEditingTextureName"];
			LastEditingTextureImagePath = _iniFile["Settings", "LastEditingTextureImagePath"];
			#endregion

			#region 設定をひとつづつ判定して初期値を入れる
			//想定しているのは、ファイルが既にあって特定の設定だけ抜けていた場合の正しい初期値を設定すること。
			//iniFile[]で設定値がなかった場合、値は空文字に設定されるので、それでかまわない項目についてはそのままにしている。
			if (_firstExpandingUseFixed == false
				&& _firstExpandingRecentFolder == "")
			{
				//前回使用フォルダ名が空の場合は初期状態と判断して初期値を設定する
				string temp = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
				FirstExpandingUseFixed = false;
				FirstExpandingRecentFolder = temp;
				FirstExpandingFixedFolder = temp;
			}
			if( _iniFile[ "Settings", "PromptToExitProgram" ] == "" )
			{
				PromptToExitProgram = true;
			}
			if (_iniFile["Settings", "CheckUpdateAtStartUp"] == "")
			{
				CheckUpdateAtStartUp = true;
			}
			if (_iniFile["Settings", "LastEditingTextureName"] == "")
			{
				LastEditingTextureName = TextureManager.BLOTMAP_NAME;
			}
			#endregion

		}

	}
}