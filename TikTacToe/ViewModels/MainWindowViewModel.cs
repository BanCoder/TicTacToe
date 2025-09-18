using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TikTacToe.Models;

namespace TikTacToe.ViewModels
{
	public class MainWindowViewModel: BindableBase
	{
		private CellStatus _currentPlayerStatus;
		public CellStatus CurrentPlayerStatus
		{
			get => _currentPlayerStatus;
			private set
			{
				_currentPlayerStatus = value;
				RaisePropertyChanged(nameof(CurrentPlayerStatus));
			}
		}
		public List<List<CellBtnVm>> AllCells { get; } = new List<List<CellBtnVm>>();
		private int _cellRowColumnCount = 3;
		public MainWindowViewModel()
		{
			CurrentPlayerStatus = CellStatus.Cross;
			for (int row = 0; row < _cellRowColumnCount; row++)
			{
				var newRow = new List<CellBtnVm>();
				for (int column = 0; column < _cellRowColumnCount; column++)
				{
					var newCell = new CellBtnVm(row, column, CheckGameStatus);
					newRow.Add(newCell);
				}
				AllCells.Add(newRow);
			}
		}
		private void CheckGameStatus(CellBtnVm lastClickBtn)
		{
			// column check
			var columnCheck = new List<CellBtnVm>();
			for (int i = 0; i < _cellRowColumnCount; i++)
			{
				columnCheck.Add(AllCells[i][lastClickBtn.Column]);
			}

			if (columnCheck.All(x => x.Status == lastClickBtn.Status))
			{
				StopGame(lastClickBtn.Status);
				return;
			}

			//row check
			var rowCheck = new List<CellBtnVm>();
			for (int j = 0; j < _cellRowColumnCount; j++)
			{
				rowCheck.Add(AllCells[lastClickBtn.Row][j]); 
			}

			if (rowCheck.All(x => x.Status == lastClickBtn.Status))
			{
				StopGame(lastClickBtn.Status);
				return;
			}

			//cross1 check
			if(lastClickBtn.Row == lastClickBtn.Column)
			{
				var crossCheck1 = new List<CellBtnVm>();
				for (int i = 0; i < _cellRowColumnCount; i++)
				{
					crossCheck1.Add(AllCells[i][i]); 
				}
				if (crossCheck1.All(x => x.Status == lastClickBtn.Status))
				{
					StopGame(lastClickBtn.Status);
					return;
				}

			}

			//cross2 check
			if (lastClickBtn.Row + lastClickBtn.Column == _cellRowColumnCount-1)
			{
				var crossCheck2 = new List<CellBtnVm>();
				for (int i = 0; i < _cellRowColumnCount; i++)
				{
					crossCheck2.Add(AllCells[i][_cellRowColumnCount-1-i]);
				}
				if (crossCheck2.All(x => x.Status == lastClickBtn.Status))
				{
					StopGame(lastClickBtn.Status);
					return;
				}
			}

			if (AllCells.SelectMany(x => x.Where(c => c.Status == CellStatus.Empty)).Count() == 0)
			{
				StopGame();
				return;
			}
			CurrentPlayerStatus = lastClickBtn.Status == CellStatus.Cross ? CellStatus.Circle : CellStatus.Cross;
		}
		private void StopGame()
		{
			MessageBox.Show("Ничья!", "Draw", MessageBoxButton.OK, MessageBoxImage.Information);
			Reset();
		}
		private void StopGame(CellStatus winnerStatus)
		{
			string winnerName = winnerStatus == CellStatus.Cross ? "Крестик" : "Нолик";
			MessageBox.Show($"Победитель: {winnerName}", "Win", MessageBoxButton.OK, MessageBoxImage.Information);
			Reset();
		}
		private void Reset()
		{
			foreach (var row in AllCells)
			{
				foreach (var cell in row)
				{
					cell.Status = CellStatus.Empty;
				}
			}
		}
	}
}
