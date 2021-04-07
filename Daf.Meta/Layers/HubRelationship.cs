﻿// SPDX-License-Identifier: MIT
// Copyright © 2021 Oscar Björhn, Petter Löfgren and contributors

using Daf.Meta.JsonConverters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Daf.Meta.Layers
{
	public class HubRelationship : PropertyChangedBaseClass
	{
		public HubRelationship(Hub hub)
		{
			Hub = hub;
		}

		[JsonConverter(typeof(HubConverter))]
		public Hub Hub { get; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "We need at least an init setter in order to support deserialization.")]
		public ObservableCollection<HubMapping> Mappings { get; init; } = new ObservableCollection<HubMapping>();

		[JsonIgnore]
		public List<StagingColumn> AvailableColumns
		{
			get
			{
				List<StagingColumn> availableColumns = new();

				foreach (StagingColumn businessKey in Hub.BusinessKeys)
				{
					availableColumns.Add(businessKey);
				}

				return availableColumns;
			}
		}
	}

	public class HubMapping : PropertyChangedBaseClass
	{
		public HubMapping(StagingColumn hubColumn)
		{
			_hubColumn = hubColumn;
		}

		private StagingColumn? _stagingColumn;

		[JsonConverter(typeof(StagingColumnConverter))]
		public StagingColumn? StagingColumn
		{
			get { return _stagingColumn; }
			set
			{
				if (_stagingColumn != value)
				{
					_stagingColumn = value;

					if (_stagingColumn != null)
						_stagingColumn.Satellite = null;

					NotifyPropertyChanged("StagingColumn");
				}
			}
		}

		private StagingColumn _hubColumn;

		[JsonConverter(typeof(StagingColumnConverter))]
		public StagingColumn HubColumn
		{
			get { return _hubColumn; }
			set
			{
				if (_hubColumn != value)
				{
					_hubColumn = value;

					NotifyPropertyChanged("HubColumn");
				}
			}
		}
	}
}
