﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;

namespace Zen.Module.Data.Relational.Mapper
{
    public class ColumnAttributeTypeMapper<T> : FallBackTypeMapper
    {
        public ColumnAttributeTypeMapper()
            : base(new SqlMapper.ITypeMap[]
            {
                new CustomPropertyTypeMap(typeof(T), (type, columnName) =>
                {
                    return type.GetProperties()
                        .FirstOrDefault(prop =>
                        {
                            return prop.GetCustomAttributes(false)
                                .OfType<ColumnAttribute>()
                                .Where(attr => attr.Name!= null)
                                .Any(attr => string.Equals(attr.Name, columnName, StringComparison.OrdinalIgnoreCase));
                        });
                }),
                new DefaultTypeMap(typeof(T))
            }) { }
    }
}