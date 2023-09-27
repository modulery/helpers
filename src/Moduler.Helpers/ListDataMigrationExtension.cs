using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Moduler.Helpers
{
    public static class ListDataMigrationExtension
    {
        public static void MigrateListFrom<T>(this List<T> target, List<T> source, Func<T, T, bool> areEqual, bool updateNulls = true, Action<T, T> updateAction = null)
        {
            foreach (T sourceItem in source)
            {
                T matchingTargetItem = target.FirstOrDefault(targetItem => areEqual(targetItem, sourceItem));

                if (matchingTargetItem == null)
                {
                    target.Add(sourceItem);
                    continue;
                }
                if (updateNulls) { matchingTargetItem.UpdateNullFields(sourceItem); }
                if (updateAction != null) { updateAction(matchingTargetItem, sourceItem); }
            }
        }
        public static void UpdateNullFields(this object target, object source)
        {
            Type targetType = target.GetType();
            Type sourceType = source.GetType();

            PropertyInfo[] properties = targetType.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object sourceValue = property.GetValue(source);
                object targetValue = property.GetValue(target);

                if (targetValue == null && sourceValue != null)
                {
                    property.SetValue(target, sourceValue);
                }
            }
        }
    }
}
