using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIBuilderProtoCSharp
{
    internal static class PropertyCopier
    {
        // プロパティをコピーするクラス
        // https://rksoftware.hatenablog.com/entry/2017/08/22/024122
        public static T CopyTo<T>(object src, T dest)
        {
            if (src == null || dest == null) return dest;
            var srcProperties = src.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Where(p => p.CanRead && p.CanWrite);
            var destProperties = dest.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Where(p => p.CanRead && p.CanWrite);
            var properties = srcProperties.Join(destProperties, p => new { p.Name, p.PropertyType }, p => new { p.Name, p.PropertyType }, (p1, p2) => new { p1, p2 });
            foreach (var property in properties)
                property.p2.SetValue(dest, property.p1.GetValue(src));
            return dest;
        }

        public static T ControlCopy<T>(T origin, T target) {
            for (int i = 0; i < typeof(ControlsJson).GetProperties().Length; i++) {
                string propName = typeof(ControlsJson).GetProperties()[i].Name;
                try {
                    target.GetType().GetProperty(propName).SetValue(target, origin.GetType().GetProperty(propName).GetValue(origin));
                } catch (ArgumentException) {
                } catch (NullReferenceException) {
                } catch (System.Reflection.AmbiguousMatchException) {
                    if (target.GetType().Name == nameof(UserCheckedListBox)) {
                        var value = target.GetType().GetProperty(typeof(ControlsJson).GetProperties()[i].Name, typeof(CheckedListBox.ObjectCollection)).GetValue(origin);
                        for (int j = 0; j < ((CheckedListBox.ObjectCollection)value).Count; j++) {
                            ((UserCheckedListBox)(object)target).Items.Add(((CheckedListBox.ObjectCollection)value)[j]);
                        }
                    }
                }
            }
            return target;
        }
    }
}
