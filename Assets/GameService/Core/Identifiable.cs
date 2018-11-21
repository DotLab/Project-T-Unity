using System;
using System.Collections;
using System.Collections.Generic;

namespace GameService.Core {
	public interface IDescribable {
		string Name { get; set; }
		string Description { get; set; }
	}

	public interface IIdentifiable : IDescribable {
		int ID { get; }
	}
	
	public interface IReadonlyIdentifiedObjectList<T> : IEnumerable<T>, IEnumerable {
		T this[int id] { get; }
		bool Contains(int id);
		bool Contains(T item);
		bool TryGetValue(int id, out T value);
	}

	public sealed class IdentifiedObjectList<T> : IEnumerable<T>, IEnumerable, IReadonlyIdentifiedObjectList<T> where T : IIdentifiable {
		private Dictionary<int, T> _table;

		public int Count { get { return _table.Count; } }

		public T this[int id] { get { return _table[id]; } }

		public IdentifiedObjectList() {
			_table = new Dictionary<int, T>();
		}

		public IdentifiedObjectList(IEnumerable<T> list) :
			this() {
			foreach (T e in list) {
				_table.Add(e.ID, e);
			}
		}

		public void Clear() {
			_table.Clear();
		}

		public void Add(T obj) {
			_table.Add(obj.ID, obj);
		}

		public bool Remove(T obj) {
			return _table.Remove(obj.ID);
		}

		public bool Remove(int id) {
			return _table.Remove(id);
		}

		public bool Contains(int id) {
			return _table.ContainsKey(id);
		}

		public bool Contains(T item) {
			return this.Contains(item.ID);
		}

		public void ForEach(Action<T> action) {
			foreach (T e in _table.Values) {
				action(e);
			}
		}

		public bool TryGetValue(int id, out T value) {
			return _table.TryGetValue(id, out value);
		}

		public T[] ToArray() {
			T[] ret = new T[_table.Count];
			_table.Values.CopyTo(ret, 0);
			return ret;
		}

		public IEnumerator<T> GetEnumerator() {
			return ((IEnumerable<T>)_table.Values).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return this.GetEnumerator();
		}
	}
}
