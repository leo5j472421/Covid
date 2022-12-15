using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace Covid.Helper
{
    public static class DapperUserDefinedType
    {
        public static DataTable ToDataTable<T>(this List<T> iList)
        {
            var dataTable = new DataTable();
            var propertyDescriptorCollection =
                TypeDescriptor.GetProperties(typeof(T));
            var targetIndexes = new List<int>();
            for (var i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                var propertyDescriptor = propertyDescriptorCollection[i];
                if (propertyDescriptor.Attributes.Contains(new IgnoreWhenDataTable()))
                {
                    targetIndexes.Add(i);
                    continue;
                }

                var type = propertyDescriptor.PropertyType;
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);
                if (type.IsEnum)
                    type = typeof(int);

                dataTable.Columns.Add(propertyDescriptor.Name, type);
            }
            var values = new List<object>();
            foreach (var iListItem in iList)
            {
                for (var i = 0; i < propertyDescriptorCollection.Count; i++)
                {
                    if (!targetIndexes.Contains(i))
                    {
                        values.Add(propertyDescriptorCollection[i].GetValue(iListItem));
                    }
                }
                dataTable.Rows.Add(values.ToArray());
                values = new List<object>();
            }
            return dataTable;
        }
    }

    public class IgnoreWhenDataTable : Attribute
    {
    }
}