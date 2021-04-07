﻿// SPDX-License-Identifier: MIT
// Copyright © 2021 Oscar Björhn, Petter Löfgren and contributors

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Daf.Meta.Layers.Connections;

namespace Daf.Meta.JsonConverters
{
	public class RestConnectionConverter : JsonConverter<RestConnection>
	{
		public override RestConnection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			throw new NotImplementedException($"Read is not implemented for {GetType()}.");
		}

		public override void Write(Utf8JsonWriter writer, RestConnection value, JsonSerializerOptions options)
		{
			if (writer == null || value == null)
				throw new ArgumentNullException($"Can't serialize the value if {nameof(Utf8JsonWriter)} or {nameof(RestConnection)} is null.");

			writer.WriteStringValue(value.Name);
		}
	}
}
