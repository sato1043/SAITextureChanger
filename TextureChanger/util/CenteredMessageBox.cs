﻿using System;
using System.Windows.Forms;

namespace TextureChanger.util
{
	/// <summary>
	/// オーナーウィンドウの真中に表示される MessageBox
	/// </summary>
	public class CenteredMessageBox
	{

		/// <summary>
		/// 親ウィンドウ
		/// </summary>
		private IWin32Window m_ownerWindow = null;

		/// <summary>
		/// フックハンドル
		/// </summary>
		private IntPtr m_hHook = (IntPtr)0;

		/// <summary>
		/// メッセージボックスを表示する
		/// </summary>
		/// <param name="owner"></param>
		/// <param name="messageBoxText"></param>
		/// <param name="caption"></param>
		/// <param name="button"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		public static DialogResult Show(
			IWin32Window owner,
			string messageBoxText,
			string caption,
			MessageBoxButtons button,
			MessageBoxIcon icon )
		{
			CenteredMessageBox mbox = new CenteredMessageBox( owner );
			return mbox.Show( messageBoxText, caption, button, icon );
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="window">親ウィンドウ</param>
		private CenteredMessageBox( IWin32Window window )
		{
			m_ownerWindow = window;
		}

		/// <summary>
		/// メッセージボックスを表示する
		/// </summary>
		/// <param name="messageBoxText"></param>
		/// <param name="caption"></param>
		/// <param name="button"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		private DialogResult Show(
			string messageBoxText,
			string caption,
			MessageBoxButtons button,
			MessageBoxIcon icon )
		{
			// フックを設定する。
			IntPtr hInstance = Win32Api.GetWindowLong( m_ownerWindow.Handle, Win32Api.GWL_HINSTANCE );
			IntPtr threadId = Win32Api.GetCurrentThreadId( );
			m_hHook = Win32Api.SetWindowsHookEx( Win32Api.WH_CBT, new Win32Api.HOOKPROC( HookProc ), hInstance, threadId );

			return MessageBox.Show( m_ownerWindow, messageBoxText, caption, button, icon );
		}

		/// <summary>
		/// フックプロシージャ
		/// </summary>
		/// <param name="nCode"></param>
		/// <param name="wParam"></param>
		/// <param name="lParam"></param>
		/// <returns></returns>
		private IntPtr HookProc( int nCode, IntPtr wParam, IntPtr lParam )
		{

			if( nCode == Win32Api.HCBT_ACTIVATE )
			{
				Win32Api.RECT rcForm = new Win32Api.RECT( 0, 0, 0, 0 );
				Win32Api.RECT rcMsgBox = new Win32Api.RECT( 0, 0, 0, 0 );

				Win32Api.GetWindowRect( m_ownerWindow.Handle, out rcForm );
				Win32Api.GetWindowRect( wParam, out rcMsgBox );

				// センター位置を計算する。
				int x = ( rcForm.Left + ( rcForm.Right - rcForm.Left ) / 2 ) - ( ( rcMsgBox.Right - rcMsgBox.Left ) / 2 );
				int y = ( rcForm.Top + ( rcForm.Bottom - rcForm.Top ) / 2 ) - ( ( rcMsgBox.Bottom - rcMsgBox.Top ) / 2 );

				Win32Api.SetWindowPos( wParam, 0, x, y, 0, 0, Win32Api.SWP_NOSIZE | Win32Api.SWP_NOZORDER | Win32Api.SWP_NOACTIVATE );

				IntPtr result = Win32Api.CallNextHookEx( m_hHook, nCode, wParam, lParam );

				// フックを解除する。
				Win32Api.UnhookWindowsHookEx( m_hHook );
				m_hHook = (IntPtr)0;

				return result;

			}
			else
			{
				return Win32Api.CallNextHookEx( m_hHook, nCode, wParam, lParam );
			}
		}
	}

}