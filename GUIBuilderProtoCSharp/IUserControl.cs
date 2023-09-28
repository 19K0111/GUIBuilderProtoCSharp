using System;
using System.Collections.Generic;
using System.Text;

namespace GUIBuilderProtoCSharp {
    internal interface IUserControl<T> {
        // staticが使えない
        public abstract void Init(ref List<T> l, ref bool[] nameManageList);

        public abstract void Add();

        public abstract void Delete();

        // public int Index { get; set; }

        public string Name { get; set; }
    }
}
