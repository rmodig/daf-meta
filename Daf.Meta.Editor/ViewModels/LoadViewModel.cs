// SPDX-License-Identifier: MIT
// Copyright © 2021 Oscar Björhn, Petter Löfgren and contributors

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Daf.Meta.Layers;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace Daf.Meta.Editor.ViewModels
{
	public class LoadViewModel : ObservableObject, IDropTarget
	{
		public RelayCommand AddLoadColumnCommand { get; }
		public RelayCommand DeleteLoadColumnCommand { get; }

		public LoadViewModel()
		{
			AddLoadColumnCommand = new RelayCommand(AddLoadColumn);
			DeleteLoadColumnCommand = new RelayCommand(DeleteLoadColumn, CanDeleteLoadColumn);
			WeakReferenceMessenger.Default.Register<LoadViewModel, RefreshedMetadata>(this, (r, m) => { RefreshedMetadata(); });

			Columns.CollectionChanged += Colums_CollectionChanged;
		}

		private void Colums_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			// When you drag/drop, this gets triggered twice. First the object gets removed from its original place, then it gets reinserted at its new index.

			// When you drag/drop several items at once, it will first remove each item consecutively, then add each item consecutively.
			// So there are n removals followed by n additions.
		}

		private void RefreshedMetadata()
		{
			Columns.Clear();

			if (_selectedDataSource != null)
			{
				foreach (Column column in SelectedDataSource!.LoadTable!.Columns)
				{
					ColumnViewModel columnViewModel = new(column);
					Columns.Add(columnViewModel);
				}
			}
		}

		public ObservableCollection<ColumnViewModel> Columns { get; } = new();

		private List<DataSource>? _selectedDataSources;

		public List<DataSource>? SelectedDataSources
		{
			get { return _selectedDataSources; }
			set
			{
				SetProperty(ref _selectedDataSources, value);
			}
		}

		private DataSource? _selectedDataSource;

		public DataSource? SelectedDataSource
		{
			get
			{
				return _selectedDataSource;
			}
			set
			{
				Columns.Clear();

				SetProperty(ref _selectedDataSource, value);

				if (_selectedDataSource != null)
				{
					foreach (Column column in SelectedDataSource!.LoadTable!.Columns)
					{
						ColumnViewModel columnViewModel = new(column);
						Columns.Add(columnViewModel);
					}
				}
			}
		}

		private List<ColumnViewModel>? _selectedColumns;
		public List<ColumnViewModel>? SelectedColumns
		{
			get { return _selectedColumns; }
			set
			{
				SetProperty(ref _selectedColumns, value);

				DeleteLoadColumnCommand.NotifyCanExecuteChanged();
			}
		}

		private ColumnViewModel? _selectedColumn;
		public ColumnViewModel? SelectedColumn
		{
			get { return _selectedColumn; }
			set
			{
				SetProperty(ref _selectedColumn, value);

				DeleteLoadColumnCommand.NotifyCanExecuteChanged();
			}
		}

		private void AddLoadColumn()
		{
			if (SelectedDataSource == null)
				throw new InvalidOperationException();

			Column column = SelectedDataSource.AddLoadColumn();

			// Create a new view model column and add it to the list.
			ColumnViewModel columnViewModel = new(column);
			Columns.Add(columnViewModel);
		}

		private bool CanDeleteLoadColumn()
		{
			if (SelectedColumn == null)
				return false;
			else
				return true;
		}

		private void DeleteLoadColumn()
		{
			if (SelectedDataSource == null || SelectedColumn == null)
				throw new InvalidOperationException();

			SelectedDataSource.RemoveLoadColumn(SelectedColumn.Column);

			// Remove the view model column from the list.
			Columns.Remove(SelectedColumn);
		}

		//public void DragOver(IDropInfo dropInfo)
		//{
		//	return;
		//}

		public void Drop(IDropInfo dropInfo)
		{
			throw new NotImplementedException();
		}
	}
}
