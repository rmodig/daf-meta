﻿// SPDX-License-Identifier: MIT
// Copyright © 2021 Oscar Björhn, Petter Löfgren and contributors

using Daf.Meta.JsonConverters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Daf.Meta.Layers
{
	public class LinkRelationship : PropertyChangedBaseClass
	{
		public LinkRelationship(Link link)
		{
			Link = link;
		}

		[JsonConverter(typeof(LinkConverter))]
		public Link Link { get; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "We need at least an init setter in order to support deserialization.")]
		public ObservableCollection<LinkMapping> Mappings { get; init; } = new ObservableCollection<LinkMapping>();

		[JsonIgnore]
		public List<StagingColumn> AvailableColumns
		{
			get
			{
				List<StagingColumn> availableColumns = new();

				foreach (StagingColumn businessKey in Link.BusinessKeys)
				{
					availableColumns.Add(businessKey);
				}

				return availableColumns;
			}
		}
	}

	public class LinkMapping : PropertyChangedBaseClass
	{
		public LinkMapping(StagingColumn linkColumn)
		{
			_linkColumn = linkColumn;
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

		private StagingColumn _linkColumn;

		[JsonConverter(typeof(StagingColumnConverter))]
		public StagingColumn LinkColumn
		{
			get { return _linkColumn; }
			set
			{
				if (_linkColumn != value)
				{
					_linkColumn = value;

					NotifyPropertyChanged("LinkColumn");
				}
			}
		}
	}
}
