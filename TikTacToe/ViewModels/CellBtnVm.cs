using Prism.Commands;
using Prism.Mvvm;
using System;
using TikTacToe.Models;

namespace TikTacToe.ViewModels
{
	public class CellBtnVm: BindableBase
	{
		private CellStatus _status = CellStatus.Empty;
		public CellStatus Status
		{
			get => _status;
			set
			{
				_status = value;
				RaisePropertyChanged(nameof(Status));
			}
		}
		public DelegateCommand<object> SetStatusCommand { get; }
		private Action<CellBtnVm> _updateAppStatus;
		public int Row { get; }
		public int Column { get; }
		public CellBtnVm(int row, int column, Action<CellBtnVm> updateAppStatus)
		{
			Row = row;
			Column = column;
			_updateAppStatus = updateAppStatus;
			SetStatusCommand = new DelegateCommand<object>(SetStatus);
		}
		private void SetStatus(object status)
		{
			if (this.Status != CellStatus.Empty)
			{
				return; 
			}
			Status = (CellStatus)status;
			_updateAppStatus?.Invoke(this);
		}
	}
}
